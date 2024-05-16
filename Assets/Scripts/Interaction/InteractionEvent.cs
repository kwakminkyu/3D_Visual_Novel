using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] private bool isAutoEvent;
    [SerializeField] private DialogueEvent dialogueEvent;

    private void Start()
    {
        bool flag = CheckEvent();
        gameObject.SetActive(flag);
    }

    private bool CheckEvent()
    {
        bool flag = true;

        // 등장 조건과 일치하지 않을 경우, 등장 시키지 않음
        for (int i = 0; i < dialogueEvent.eventTiming.eventConditions.Length; i++)
        {
            if (DataManager.instance.eventFlags[dialogueEvent.eventTiming.eventConditions[i]]
                != dialogueEvent.eventTiming.conditionFlag)
            {
                flag = false;
                break;
            }
        }

        // 등장 조건과 관계없이, 퇴장 조건과 일치할 경우, 무조건 등장 시키지 않음
        if (DataManager.instance.eventFlags[dialogueEvent.eventTiming.eventEndNum])
        {
            flag = false;
        }

        return flag;
    }

    public Dialogue[] GetDialogue()
    {
        DataManager.instance.eventFlags[dialogueEvent.eventTiming.eventNum] = true;
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

    public AppearType GetAppearType()
    {
        return dialogueEvent.appearType;
    }

    public GameObject[] GetTargets()
    {
        return dialogueEvent.targets;
    }

    public GameObject GetNextEvent()
    {
        return dialogueEvent.nextEvent;
    }

    private void Update()
    {
        if (isAutoEvent && DataManager.isFinish && TransferManager.isFinished)
        {
            DialougueManager _dialougueManager = FindAnyObjectByType<DialougueManager>();
            DialougueManager.isWaiting = true;
            if (GetAppearType() == AppearType.Appear)
            {
                _dialougueManager.SetAppearObjects(GetTargets());
            }
            else if (GetAppearType() == AppearType.Disappear)
            {
                _dialougueManager.SetDisappearObjects(GetTargets());
            }
            _dialougueManager.SetNextEvent(GetNextEvent());
            _dialougueManager.ShowDialogue(GetDialogue());

            gameObject.SetActive(false);
        }
    }
}
