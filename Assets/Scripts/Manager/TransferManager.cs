using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferManager : MonoBehaviour
{
    private string transferLocationName;

    public IEnumerator Transfer(string sceneName, string locationName)
    {
        transferLocationName = locationName;
        TransferSpawnManager.spawnTiming = true;
        SceneManager.LoadScene(sceneName);
        yield return null;
    }

    public string GetLocationName()
    {
        return transferLocationName;
    }
}
