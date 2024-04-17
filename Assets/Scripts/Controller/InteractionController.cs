using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] Camera cam;
    RaycastHit hitInfo;

    [SerializeField] GameObject normalCrosshair;
    [SerializeField] GameObject InteractiveCrosshair;

    private bool isContact;

    private void Update()
    {
        CheckObject();
    }

    private void CheckObject()
    {
        Vector3 t_MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

        if (Physics.Raycast(cam.ScreenPointToRay(t_MousePos), out hitInfo, 100f))
        {
            Contact();
        }
        else
        {
            NotContact();
        }
    }

    private void Contact()
    {
        if (hitInfo.transform.CompareTag("Interaction"))
        {
            if (!isContact)
            {
                isContact = true;
                InteractiveCrosshair.SetActive(true);
                normalCrosshair.SetActive(false);
            }
        }
        else
        {
            NotContact();
        }
    }

    private void NotContact()
    {
        if (isContact)
        {
            isContact = false;
            InteractiveCrosshair.SetActive(false);
            normalCrosshair.SetActive(true);
        }
    }
}
