using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public static bool isInteract = false;

    [SerializeField] private Camera cam;
    private RaycastHit hitInfo;

    [SerializeField] private GameObject normalCrosshair;
    [SerializeField] private GameObject InteractiveCrosshair;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject cursor;

    private bool isContact;

    [SerializeField] private ParticleSystem questionEffect;

    private DialougueManager _dialougueManager;

    public void HideUI()
    {
        crosshair.SetActive(false);
        cursor.SetActive(false);
    }

    private void Awake()
    {
        _dialougueManager = FindAnyObjectByType<DialougueManager>();
    }

    private void Update()
    {
        CheckObject();
        ClickLeftButton();
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

    private void ClickLeftButton()
    {
        if (!isInteract)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isContact)
                {
                    Interact();
                }
            }
        }
    }

    private void Interact()
    {
        isInteract = true;

        questionEffect.gameObject.SetActive(true);
        Vector3 targetPos = hitInfo.transform.position;
        questionEffect.GetComponent<QuestionEffect>().SetTarget(targetPos);
        questionEffect.transform.position = cam.transform.position;

        StartCoroutine(WaitCollision());
    }

    private IEnumerator WaitCollision()
    {
        yield return new WaitUntil(()=>QuestionEffect.isCollide);
        QuestionEffect.isCollide = false;

        _dialougueManager.ShowDialogue();
    }
}
