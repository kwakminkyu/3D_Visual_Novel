using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinCrosshair : MonoBehaviour
{
    [SerializeField] private float spinSpeed;
    [SerializeField] private Vector3 spinDir;

    private void Update()
    {
        transform.Rotate(spinDir * spinSpeed * Time.deltaTime);
    }
}
