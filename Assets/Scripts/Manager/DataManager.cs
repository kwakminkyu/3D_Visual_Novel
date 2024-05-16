using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [SerializeField] string csvFileName;

    Dictionary<int, Dialogue> dialogueDic = new Dictionary<int, Dialogue>();

    public bool[] eventFlags = new bool[100];

    public static bool isFinish = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DialogueParser parser = GetComponent<DialogueParser>();
            Dialogue[] dialogues = parser.Parse(csvFileName);
            for (int i = 0; i < dialogues.Length; i++)
            {
                dialogueDic.Add(i + 1, dialogues[i]);
            }
            isFinish = true;
        }
    }

    public Dialogue[] GetDialogue(int startNum, int endNum)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();

        for (int i = 0; i <= endNum - startNum; i++)
        {
            dialogueList.Add(dialogueDic[startNum + i]);
        }
        return dialogueList.ToArray();
    }
}
