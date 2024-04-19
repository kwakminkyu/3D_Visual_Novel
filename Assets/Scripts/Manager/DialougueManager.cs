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

    private InteractionController _interactionController;

    private void Awake()
    {
        _interactionController = FindAnyObjectByType<InteractionController>();
    }
     
    public void ShowDialogue(Dialogue[] dialogues)
    {
        txtDialogue.text = "";
        txtName.text = "";
        _interactionController.HideUI();
        this.dialogues = dialogues;

        SettingUI(true);
    }

    private void SettingUI(bool flag)
    {
        dialogueBar.SetActive(flag);
        dialogueNameBar.SetActive(flag);
    }
}
