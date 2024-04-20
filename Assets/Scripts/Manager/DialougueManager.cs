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
        replaceText = replaceText.Replace("\\n", "\n");

        bool white = false, yellow = false, syan = false;
        bool ignore = false;

        for (int i = 0; i < replaceText.Length; i++)
        {
            switch (replaceText[i])
            {
                case 'ⓦ': white = true; yellow = false; syan = false; ignore = true; break;
                case 'ⓨ': white = false; yellow = true; syan = false; ignore = true; break;
                case 'ⓒ': white = false; yellow = false; syan = true; ignore = true; break;
            }
            
            string letter = replaceText[i].ToString();

            if (!ignore)
            {
                if (white) { letter = "<color=#ffffff>" + letter + "</color>"; }
                else if (yellow) { letter = "<color=#ffff00>" + letter + "</color>"; }
                else if (syan) { letter = "<color=#42dee3>" + letter + "</color>"; }
                txtDialogue.text += letter;
            }
            ignore = false;

            yield return new WaitForSeconds(textDelay);
        }
        isNext = true;
    }

    private void SettingUI(bool flag)
    {
        dialogueBar.SetActive(flag);
        if (flag)
        {
            if (dialogues[lineCount].name == "")
            {
                dialogueNameBar.SetActive(false);
            }
            else
            {
                dialogueNameBar.SetActive(true);
                txtName.text = dialogues[lineCount].name;
            }
        }
    }
}
