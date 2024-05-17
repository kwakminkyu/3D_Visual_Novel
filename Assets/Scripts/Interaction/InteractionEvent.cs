using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] private bool isAutoEvent;
    [SerializeField] private DialogueEvent[] dialogueEvent;
    private int currentCount;

    private void Start()
    {
        bool flag = CheckEvent();
        gameObject.SetActive(flag);
    }

    private bool CheckEvent()
    {
        bool flag = true;

        for (int i = 0; i < dialogueEvent.Length; i++)
        {
            flag = true;
            // ���� ���ǰ� ��ġ���� ���� ���, ���� ��Ű�� ����
            for (int j = 0; j < dialogueEvent[i].eventTiming.eventConditions.Length; j++)
            {
                if (DataManager.instance.eventFlags[dialogueEvent[i].eventTiming.eventConditions[j]]
                    != dialogueEvent[i].eventTiming.conditionFlag)
                {
                    flag = false;
                    break;
                }
            }

            // ���� ���ǰ� �������, ���� ���ǰ� ��ġ�� ���, ������ ���� ��Ű�� ����
            if (DataManager.instance.eventFlags[dialogueEvent[i].eventTiming.eventEndNum])
            {
                flag = false;
            }

            if (flag)
            {
                currentCount = i;
                break;
            }
        }
        return flag;
    }

    public Dialogue[] GetDialogue()
    {
        if (DataManager.instance.eventFlags[dialogueEvent[currentCount].eventTiming.eventEndNum])
        {
            return null;
        }

        // ��ȣ�ۿ� �� ��ȭ
        if (!DataManager.instance.eventFlags[dialogueEvent[currentCount].eventTiming.eventNum] || dialogueEvent[currentCount].isSame)
        {
            DataManager.instance.eventFlags[dialogueEvent[currentCount].eventTiming.eventNum] = true;
            dialogueEvent[currentCount].dialogues = SettingDialogue(dialogueEvent[currentCount].dialogues
                                                                    ,(int)dialogueEvent[currentCount].line.x
                                                                    ,(int)dialogueEvent[currentCount].line.y);
            return dialogueEvent[currentCount].dialogues;
        }
        // ��ȣ�ۿ� �� ��ȭ
        else
        {
            dialogueEvent[currentCount].dialogues2nd = SettingDialogue(dialogueEvent[currentCount].dialogues2nd
                                                        ,(int)dialogueEvent[currentCount].line2nd.x
                                                        ,(int)dialogueEvent[currentCount].line2nd.y);
            return dialogueEvent[currentCount].dialogues2nd;
        }
    }

    private Dialogue[] SettingDialogue(Dialogue[] dialogues, int lineX, int lineY)
    {
        Dialogue[] targetDialogues = DataManager.instance.GetDialogue(lineX, lineY);
        for (int i = 0; i < dialogueEvent[currentCount].dialogues.Length; i++)
        {
            targetDialogues[i].tf_target = dialogues[i].tf_target;
            targetDialogues[i].cameraType = dialogues[i].cameraType;
        }
        return targetDialogues;
    }

    public AppearType GetAppearType()
    {
        return dialogueEvent[currentCount].appearType;
    }

    public GameObject[] GetTargets()
    {
        return dialogueEvent[currentCount].targets;
    }

    public GameObject GetNextEvent()
    {
        return dialogueEvent[currentCount].nextEvent;
    }

    public int GetEventNumber()
    {
        CheckEvent();
        return dialogueEvent[currentCount].eventTiming.eventNum;
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
