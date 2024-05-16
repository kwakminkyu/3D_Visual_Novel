using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferManager : MonoBehaviour
{
    private string transferLocationName;

    private SplashManager _splashManager;
    InteractionController _interactionController;

    public static bool isFinished = true;

    private void Start()
    {
        _splashManager = FindAnyObjectByType<SplashManager>();
        _interactionController = FindAnyObjectByType<InteractionController>();
    }

    public IEnumerator Transfer(string sceneName, string locationName)
    {
        isFinished = false;
        _interactionController.SettingUI(false);
        SplashManager.isFinished = false;
        StartCoroutine(_splashManager.FadeOut(false, true));
        yield return new WaitUntil(()=> SplashManager.isFinished);

        transferLocationName = locationName;
        TransferSpawnManager.spawnTiming = true;
        SceneManager.LoadScene(sceneName);
        yield return null;
    }

    public IEnumerator Done()
    {
        SplashManager.isFinished = false;
        StartCoroutine(_splashManager.FadeIn(false, true));
        yield return new WaitUntil(() => SplashManager.isFinished);

        isFinished = true;
        yield return new WaitForSeconds(0.3f);

        if (!DialougueManager.isWaiting)
            _interactionController.SettingUI(true);
    }

    public string GetLocationName()
    {
        return transferLocationName;
    }
}
