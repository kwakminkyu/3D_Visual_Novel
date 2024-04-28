using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] private DialogueEvent dialogueEvent;

    public Dialogue[] GetDialogue()
    {
        DialogueEvent targetDialogueEvent = new DialogueEvent();
        targetDialogueEvent.dialogues = DataManager.instance.GetDialogue((int)dialogueEvent.line.x, (int)dialogueEvent.line.y);
        for (int i = 0; i < dialogueEvent.dialogues.Length; i++)
        {
            targetDialogueEvent.dialogues[i].tf_target = dialogueEvent.dialogues[i].tf_target;
            targetDialogueEvent.dialogues[i].cameraType = dialogueEvent.dialogues[i].cameraType;
        }
        dialogueEvent.dialogues = targetDialogueEvent.dialogues;

        return dialogueEvent.dialogues;
    }
}
