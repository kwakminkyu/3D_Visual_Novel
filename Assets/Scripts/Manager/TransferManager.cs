using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferManager : MonoBehaviour
{
    public IEnumerator Transfer(string sceneName, string locationName)
    {
        SceneManager.LoadScene(sceneName);
        yield return null;
    }
}
