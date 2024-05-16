using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    public static bool isInteract = false;

    [SerializeField] private Camera cam;
    private RaycastHit hitInfo;

    [SerializeField] private GameObject normalCrosshair;
    [SerializeField] private GameObject InteractiveCrosshair;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject cursor;
    [SerializeField] private GameObject fieldCursor;
    [SerializeField] private GameObject targetNameBar;
    [SerializeField] private TextMeshProUGUI targetName;

    private bool isContact;

    [SerializeField] private ParticleSystem questionEffect;
    [SerializeField] private Image img_Interaction;
    [SerializeField] private Image img_InteractionEffect;

    private DialougueManager _dialougueManager;

    public void SettingUI(bool flag)
    {
        crosshair.SetActive(flag);
        if (!flag)
        {
            StopCoroutine("Interaction");
            Color color = img_Interaction.color;
            color.a = 0;
            img_Interaction.color = color;
            targetNameBar.SetActive(false);
            cursor.SetActive(false);
            fieldCursor.SetActive(false);
        }
        else
        {
            if (CameraController.onlyView)
                cursor.SetActive(true);
            else
                fieldCursor.SetActive(true);
            normalCrosshair.SetActive(true);
            InteractiveCrosshair.SetActive(false);
        }
        isInteract = !flag;
    }

    private void Awake()
    {
        _dialougueManager = FindAnyObjectByType<DialougueManager>();
    }

    private void Update()
    {
        if (!isInteract)
        {
            CheckObject();
            ClickLeftButton();
        }
    }

    private void CheckObject()
    {
        if (CameraController.onlyView)
        {
            Vector3 t_MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

            if (Physics.Raycast(cam.ScreenPointToRay(t_MousePos), out hitInfo, 100f))
                Contact();
            else
                NotContact();
        }
        else
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, 3f))
                Contact();
            else
                NotContact();
        }
    }

    private void Contact()
    {
        if (hitInfo.transform.CompareTag("Interaction"))
        {
            targetNameBar.SetActive(true);
            targetName.text = hitInfo.transform.GetComponent<InteractionType>().GetName();
            if (!isContact)
            {
                isContact = true;
                InteractiveCrosshair.SetActive(true);
                normalCrosshair.SetActive(false);
                if (!CameraController.onlyView)
                {
                    StopCoroutine("Interaction");
                    StopCoroutine("InteractionEffect");
                    StartCoroutine("Interaction", true);
                    StartCoroutine("InteractionEffect");
                }
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
            targetNameBar.SetActive(false);
            InteractiveCrosshair.SetActive(false);
            normalCrosshair.SetActive(true);
            if (!CameraController.onlyView)
            {
                StopCoroutine("Interaction");
                StartCoroutine("Interaction", false);
            }
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

        StopCoroutine("Interaction");
        Color color = img_Interaction.color;
        color.a = 0;
        img_Interaction.color = color;

        questionEffect.gameObject.SetActive(true);
        Vector3 targetPos = hitInfo.transform.position;
        questionEffect.GetComponent<QuestionEffect>().SetTarget(targetPos);
        questionEffect.transform.position = cam.transform.position;

        StartCoroutine(WaitCollision());
    }

    private IEnumerator WaitCollision()
    {
        yield return new WaitUntil(()=> QuestionEffect.isCollide);
        QuestionEffect.isCollide = false;

        yield return new WaitForSeconds(0.5f);

        InteractionEvent targetEvent = hitInfo.transform.GetComponent<InteractionEvent>();

        if (hitInfo.transform.GetComponent<InteractionType>().isObject)
            DialogueCall(targetEvent);
        else
            TransferCall();
    }

    private void TransferCall()
    {
        InteractionDoor door = hitInfo.transform.GetComponent<InteractionDoor>();
        string callSceneName = door.GetSceneName();
        string callLocationName = door.GetLocationName();
        StartCoroutine(FindAnyObjectByType<TransferManager>().Transfer(callSceneName, callLocationName));
    }

    private void DialogueCall(InteractionEvent callEvent)
    {
        _dialougueManager.SetNextEvent(callEvent.GetNextEvent());

        if (callEvent.GetAppearType() == AppearType.Appear)
            _dialougueManager.SetAppearObjects(callEvent.GetTargets());
        else if (callEvent.GetAppearType() == AppearType.Disappear)
            _dialougueManager.SetDisappearObjects(callEvent.GetTargets());
        _dialougueManager.ShowDialogue(callEvent.GetDialogue());
    }

    private IEnumerator Interaction(bool appear)
    {
        Color color = img_Interaction.color;
        if (appear)
        {
            color.a = 0;
            while (color.a < 1)
            {
                color.a += 0.1f;
                img_Interaction.color = color;
                yield return null;
            }
        }
        else
        {
            while (color.a > 0)
            {
                color.a -= 0.1f;
                img_Interaction.color = color;
                yield return null;
            }
        }
    }

    private IEnumerator InteractionEffect()
    {
        while (isContact && !isInteract)
        {
            Color color = img_InteractionEffect.color;
            color.a = 0.5f;
            
            img_InteractionEffect.transform.localScale = new Vector3(1, 1, 1);
            Vector3 scale = new Vector3(1, 1, 1);

            while (color.a > 0)
            {
                color.a -= 0.01f;
                img_InteractionEffect.color = color;
                scale.Set(scale.x + Time.deltaTime, scale.y + Time.deltaTime, scale.z + Time.deltaTime);
                img_InteractionEffect.transform.localScale = scale;
                yield return null;
            }
            yield return null;
        }
    }
}
