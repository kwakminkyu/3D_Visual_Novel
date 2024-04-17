using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform tf_Crosshair;

    [SerializeField] Transform tf_Cam;

    [SerializeField] Vector2 camBoundary;
    [SerializeField] float sightMoveSpeed;
    [SerializeField] float sightSensitivity;
    [SerializeField] float lookLimitX;
    [SerializeField] float lookLimitY;
    float currentAngleX;
    float currentAngleY;

    private void Update()
    {
        CrosshairMoving();
        ViewMoving();
        KeyViewMoving();
        CameraLimit();
    }

    private void CameraLimit()
    {
        if (tf_Cam.localPosition.x >= camBoundary.x)
            tf_Cam.localPosition = new Vector3(camBoundary.x, tf_Cam.localPosition.y, tf_Cam.localPosition.z);
        else if (tf_Cam.localPosition.x <= camBoundary.x)
            tf_Cam.localPosition = new Vector3(-camBoundary.x, tf_Cam.localPosition.y, tf_Cam.localPosition.z);

        if (tf_Cam.localPosition.y >= 1 + camBoundary.y)
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, 1 + camBoundary.y, tf_Cam.localPosition.z);
        else if (tf_Cam.localPosition.y <= 1 - camBoundary.y)
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, 1 -camBoundary.y, tf_Cam.localPosition.z);
    }

    private void KeyViewMoving()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            currentAngleY += sightSensitivity * Input.GetAxisRaw("Horizontal");
            currentAngleY = Mathf.Clamp(currentAngleY, -lookLimitX, lookLimitX);
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x + sightMoveSpeed * Input.GetAxisRaw("Horizontal"), tf_Cam.localPosition.y, tf_Cam.localPosition.z);
        }

        if (Input.GetAxisRaw("Vertical") != 0)
        {
            currentAngleX += sightSensitivity * -Input.GetAxisRaw("Vertical");
            currentAngleX = Mathf.Clamp(currentAngleX, -lookLimitY, lookLimitY);
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, tf_Cam.localPosition.y + sightMoveSpeed * Input.GetAxisRaw("Vertical"), tf_Cam.localPosition.z);
        }

        tf_Cam.localEulerAngles = new Vector3(currentAngleX, currentAngleY, tf_Cam.localEulerAngles.z);
    }

    private void ViewMoving()
    {
        if (tf_Crosshair.localPosition.x > (Screen.width / 2) - 100 || tf_Crosshair.localPosition.x < (-Screen.width / 2) + 100)
        {
            currentAngleY += (tf_Crosshair.localPosition.x > 0) ? sightSensitivity : -sightSensitivity;
            currentAngleY = Mathf.Clamp(currentAngleY, -lookLimitX, lookLimitX);

            float t_applySpeed = (tf_Crosshair.localPosition.x > 0) ? sightMoveSpeed : -sightMoveSpeed;
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x + t_applySpeed, tf_Cam.localPosition.y, tf_Cam.localPosition.z);
        }

        if (tf_Crosshair.localPosition.y > (Screen.height / 2) - 100 || tf_Crosshair.localPosition.y < (-Screen.height / 2) + 100)
        {
            currentAngleX += (tf_Crosshair.localPosition.y > 0) ? -sightSensitivity : sightSensitivity;
            currentAngleX = Mathf.Clamp(currentAngleX, -lookLimitY, lookLimitY);

            float t_applySpeed = (tf_Crosshair.localPosition.y > 0) ? sightMoveSpeed : -sightMoveSpeed;
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, tf_Cam.localPosition.y + t_applySpeed, tf_Cam.localPosition.z);
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

        tf_Crosshair.localPosition = new Vector2(cursorPosX, cursorPosY);
    }
}
