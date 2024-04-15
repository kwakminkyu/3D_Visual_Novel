using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform tf_Crosshair;

    private void Update()
    {
        CrosshairMoving();
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
