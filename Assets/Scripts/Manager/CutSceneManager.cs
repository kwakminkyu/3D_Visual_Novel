using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    public static bool isFinished = false;
    private SplashManager _splashManager;
    private CameraController _cameraController;

    [SerializeField] private Image img_CutScene;

    private void Start()
    {
        _splashManager = FindAnyObjectByType<SplashManager>();
        _cameraController= FindAnyObjectByType<CameraController>();
    }

    public bool CheckCutScene()
    {
        return img_CutScene.gameObject.activeSelf;
    }

    public IEnumerator CutSceneCoroutine(string cutSceneName, bool isShow)
    {
        SplashManager.isFinished = false;
        StartCoroutine(_splashManager.FadeOut(true, false));
        yield return new WaitUntil(()=> SplashManager.isFinished);

        if (isShow)
        {
            Sprite _sprite = Resources.Load<Sprite>("CutScenes/" + cutSceneName);
            if (_sprite != null)
            {
                img_CutScene.gameObject.SetActive(true);
                img_CutScene.sprite = _sprite;
                _cameraController.CameraTargetting(null, 0.1f, true, false);
            }
            else
            {
                Debug.Log("잘못된 컷신 파일입니다.");
            }
        }
        else
        {
            img_CutScene.gameObject.SetActive(false);
        }
        
        SplashManager.isFinished = false;
        StartCoroutine(_splashManager.FadeIn(true, false));
        yield return new WaitUntil( ()=> SplashManager.isFinished);

        yield return new WaitForSeconds(0.5f);
        isFinished = true;
    }
}
