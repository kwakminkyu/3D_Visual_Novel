using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private Transform tf_target;

    private void Update()
    {
        if (tf_target != null)
        {
            Quaternion t_rotation = Quaternion.LookRotation(tf_target.position - transform.position);
            Vector3 t_euler = new Vector3(0, t_rotation.eulerAngles.y, 0);
            transform.eulerAngles = t_euler;
        }
    }
}
