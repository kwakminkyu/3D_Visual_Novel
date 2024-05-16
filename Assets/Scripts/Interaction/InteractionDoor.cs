using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDoor : MonoBehaviour
{
    [SerializeField] private bool isOnlyView;
    [SerializeField] private string sceneName;
    [SerializeField] private string locationName;

    public string GetSceneName()
    {
        CameraController.onlyView = isOnlyView;
        return sceneName;
    }

    public string GetLocationName()
    {
        return locationName;
    }
}
