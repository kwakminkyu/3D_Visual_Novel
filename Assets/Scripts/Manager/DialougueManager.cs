using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialougueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBar;
    [SerializeField] private GameObject dialogueNameBar;

    [SerializeField] private TextMeshProUGUI txtDialogue;
    [SerializeField] private TextMeshProUGUI txtName;

    Dialogue[] dialogues;

    private bool isDialogue = false;
    private bool isNext = false;

    [Header("텍스트 출력 딜레이")]
    [SerializeField] private float textDelay;
    
    private int lineCount;
    private int contextCount;

    private InteractionController _interactionController;

    private void Awake()
    {
        _interactionController = FindAnyObjectByType<InteractionController>();
    }

    private void Update()
    {
        if (isDialogue)
        {
            if (isNext)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isNext = false;
                    txtDialogue.text = "";
                    if (++contextCount < dialogues[lineCount].contexts.Length)
                        StartCoroutine(TypeWriter());
                    else
                    {
                        contextCount = 0;
                        if (++lineCount < dialogues.Length)
                            StartCoroutine(TypeWriter());
                        else
                            EndDialogue();
                    }
                }
            }
        }
    }

    public void ShowDialogue(Dialogue[] dialogues)
    {
        isDialogue = true;
        txtDialogue.text = "";
        txtName.text = "";
        _interactionController.SettingUI(false);
        this.dialogues = dialogues;

        StartCoroutine(TypeWriter());
    }

    private void EndDialogue()
    {
        isDialogue = false;
        contextCount = 0;
        lineCount = 0;
        dialogues = null;
        isNext = false;
        _interactionController.SettingUI(true);
        SettingUI(false);
    }

    private IEnumerator TypeWriter()
    {
        SettingUI(true);

        string replaceText = dialogues[lineCount].contexts[contextCount];
        replaceText = replaceText.Replace("`", ",");

        txtName.text = dialogues[lineCount].name;
        for (int i = 0; i < replaceText.Length; i++)
        {
            txtDialogue.text += replaceText[i];
            yield return new WaitForSeconds(textDelay);
        }

        isNext = true;
        yield return null;
    }

    private void SettingUI(bool flag)
    {
        dialogueBar.SetActive(flag);
        dialogueNameBar.SetActive(flag);
    }
}
