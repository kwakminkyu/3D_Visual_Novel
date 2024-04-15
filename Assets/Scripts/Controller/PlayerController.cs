using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform tf_Crosshair;

    [SerializeField] Transform tf_Cam;

    [SerializeField] float sightSensitivity;
    [SerializeField] float lookLimitX;
    [SerializeField] float lookLimitY;
    float currentAngleX;
    float currentAngleY;

    private void Update()
    {
        CrosshairMoving();
        ViewMoving();
    }

    private void ViewMoving()
    {
        if (tf_Crosshair.localPosition.x > (Screen.width / 2) - 100 || tf_Crosshair.localPosition.x < (-Screen.width / 2) + 100)
        {
            currentAngleY += (tf_Crosshair.localPosition.x > 0) ? sightSensitivity : -sightSensitivity;
            currentAngleY = Mathf.Clamp(currentAngleY, -lookLimitX, lookLimitX);
        }

        if (tf_Crosshair.localPosition.y > (Screen.height / 2) - 100 || tf_Crosshair.localPosition.y < (-Screen.height / 2) + 100)
        {
            currentAngleX += (tf_Crosshair.localPosition.y > 0) ? -sightSensitivity : sightSensitivity;
            currentAngleX = Mathf.Clamp(currentAngleX, -lookLimitY, lookLimitY);
        }

        tf_Cam.localEulerAngles = new Vector3(currentAngleX, currentAngleY, tf_Cam.localEulerAngles.z);
    }

    private void CrosshairMoving()
    {
        tf_Crosshair.localPosition = new Vector2(Input.mousePosition.x - (Screen.width / 2),
                                                 Input.mousePosition.y - (Screen.height / 2));

        float cursorPosX = tf_Crosshair.localPosition.x;
        float cursorPosY = tf_Crosshair.localPosition.y;

        cursorPosX = Mathf.Clamp(cursorPosX, (-Screen.width / 2) + 25, (Screen.width / 2) - 25);
        cursorPosY = Mathf.Clamp(cursorPosY, (-Screen.height / 2) + 25, (Screen.height / 2) - 25);

        tf_Crosshair.localPosition = new Vector2 (cursorPosX, cursorPosY);
    }
}
