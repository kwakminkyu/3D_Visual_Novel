using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Location
{
    public string name;
    public Transform tf_Spawn;
}
public class TransferSpawnManager : MonoBehaviour
{
    [SerializeField] private Location[] locations;
    Dictionary<string, Transform> locationDic = new Dictionary<string, Transform>();

    public static bool spawnTiming = false;

    private void Start()
    {
        for (int i = 0; i < locations.Length; i++)
        {
            locationDic.Add(locations[i].name, locations[i].tf_Spawn);
        }

        if (spawnTiming)
        {
            TransferManager _transferManager = FindAnyObjectByType<TransferManager>();
            string locationName = _transferManager.GetLocationName();
            Transform spawn = locationDic[locationName];
            PlayerController.instance.transform.position = spawn.position;
            PlayerController.instance.transform.rotation = spawn.rotation;
            Camera.main.transform.localPosition = new Vector3(0,1,0);
            Camera.main.transform.localEulerAngles = Vector3.zero;
            PlayerController.instance.ResetCam();

            StartCoroutine(_transferManager.Done());
            spawnTiming = false;
        }
    }
}
