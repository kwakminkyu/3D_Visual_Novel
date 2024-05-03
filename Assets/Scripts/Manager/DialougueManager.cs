using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialougueManager : MonoBehaviour
{
    public static bool isWaiting = false;
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

    // 이벤트 끝날때 등장/퇴장 시킬 오브젝트
    private GameObject[] targetGameObjects;
    private byte appearTypeNumber;
    private const byte NONE = 0, APPEAR = 1, DISAPPEAR = 2;

    public void SetAppearObjects(GameObject[] target)
    {
        targetGameObjects = target;
        appearTypeNumber = APPEAR;
    }
    
    public void SetDisappearObjects(GameObject[] target)
    {
        targetGameObjects = target;
        appearTypeNumber = DISAPPEAR;
    }

    private CameraController _cameraController;
    private InteractionController _interactionController;
    private SpriteManager _spriteManager;
    private SplashManager _splashManager;
    private CutSceneManager _cutSceneManager;
    private SlideManager _slideManager;

    private void Awake()
    {
        _cameraController = FindAnyObjectByType<CameraController>();
        _interactionController = FindAnyObjectByType<InteractionController>();
        _spriteManager = FindAnyObjectByType<SpriteManager>();
        _splashManager = FindAnyObjectByType<SplashManager>();
        _cutSceneManager = FindAnyObjectByType<CutSceneManager>();
        _slideManager = FindAnyObjectByType<SlideManager>();
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
                        {
                            StartCoroutine(CameraTargettingType());
                        }
                        else
                            StartCoroutine(EndDialogue());
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
        StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        if (isWaiting)
            yield return new WaitForSeconds(0.5f);
        isWaiting = false;
        _cameraController.CamOriginSetting();
        StartCoroutine(CameraTargettingType());
    }

    private IEnumerator EndDialogue()
    {
        if (_cutSceneManager.CheckCutScene())
        {
            SettingUI(false);
            CutSceneManager.isFinished = false;
            StartCoroutine(_cutSceneManager.CutSceneCoroutine(null, false));
            yield return new WaitUntil(() => CutSceneManager.isFinished);
            _cameraController.CameraTargetting(dialogues[lineCount].tf_target);
        }
        AppearOrDisappearObjects();

        yield return new WaitUntil(() => Spin.isFinished);

        isDialogue = false;
        contextCount = 0;
        lineCount = 0;
        dialogues = null;
        isNext = false;
        _cameraController.CameraTargetting(null, 0.05f, true, true);
        SettingUI(false);
    }

    private void AppearOrDisappearObjects()
    {
        if (targetGameObjects != null)
        {
            Spin.isFinished = false;
            for (int i = 0; i < targetGameObjects.Length; i++)
            {
                if (appearTypeNumber == APPEAR)
                {
                    targetGameObjects[i].SetActive(true);
                    StartCoroutine(targetGameObjects[i].GetComponent<Spin>().SetAppearOrDisappear(true));
                }
                else if (appearTypeNumber == DISAPPEAR)
                    StartCoroutine(targetGameObjects[i].GetComponent<Spin>().SetAppearOrDisappear(false));
            }
        }
        targetGameObjects = null;
        appearTypeNumber = NONE;
    }

    private void PlaySound()
    {
        if (dialogues[lineCount].voiceName[contextCount] != "")
        {
            SoundManager.instance.PlaySound(dialogues[lineCount].voiceName[contextCount], 2);
        }
    }

    private IEnumerator CameraTargettingType()
    {
        switch (dialogues[lineCount].cameraType)
        {
            case CameraType.ObjectFront:
                _cameraController.CameraTargetting(dialogues[lineCount].tf_target);
                break;
            case CameraType.Reset:
                _cameraController.CameraTargetting(null, 0.05f, true);
                break;
            case CameraType.FadeIn:
                SettingUI(false);
                SplashManager.isFinished = false;
                StartCoroutine(_splashManager.FadeIn(false, true));
                yield return new WaitUntil(()=> SplashManager.isFinished);
                break;
            case CameraType.FadeOut:
                SettingUI(false);
                SplashManager.isFinished = false;
                StartCoroutine(_splashManager.FadeOut(false, true));
                yield return new WaitUntil(()=> SplashManager.isFinished);
                break;
            case CameraType.FlashIn:
                SettingUI(false);
                SplashManager.isFinished = false;
                StartCoroutine(_splashManager.FadeIn(true, true));
                yield return new WaitUntil(()=> SplashManager.isFinished);
                break;
            case CameraType.FlashOut:
                SettingUI(false);
                SplashManager.isFinished = false;
                StartCoroutine(_splashManager.FadeOut(true, true));
                yield return new WaitUntil(()=> SplashManager.isFinished);
                break;
            case CameraType.ShowCutScene:
                SettingUI(false);
                CutSceneManager.isFinished = false;
                StartCoroutine(_cutSceneManager.CutSceneCoroutine(dialogues[lineCount].spriteName[contextCount], true));
                yield return new WaitUntil(()=> CutSceneManager.isFinished);
                break;
            case CameraType.HideCutScene:
                SettingUI(false);
                CutSceneManager.isFinished = false;
                StartCoroutine(_cutSceneManager.CutSceneCoroutine(null, false));
                yield return new WaitUntil(() => CutSceneManager.isFinished);
                _cameraController.CameraTargetting(dialogues[lineCount].tf_target);
                break;
            case CameraType.AppearSlideCG:
                SlideManager.isFinished = false;
                StartCoroutine(_slideManager.AppearSlide(SplitSlideCGName()));
                yield return new WaitUntil(()=> SlideManager.isFinished);
                _cameraController.CameraTargetting(dialogues[lineCount].tf_target);
                break;
            case CameraType.DisappearSlideCG:
                SlideManager.isFinished = false;
                StartCoroutine(_slideManager.DisappearSlide());
                yield return new WaitUntil(() => SlideManager.isFinished);
                _cameraController.CameraTargetting(dialogues[lineCount].tf_target);
                break;
            case CameraType.ChangeSlideCG:
                SlideManager.isChanged = false;
                StartCoroutine(_slideManager.ChangeSlide(SplitSlideCGName()));
                yield return new WaitUntil(() => SlideManager.isChanged);
                _cameraController.CameraTargetting(dialogues[lineCount].tf_target);
                break;
        }
        StartCoroutine(TypeWriter());
    }

    private IEnumerator TypeWriter()
    {
        SettingUI(true);
        ChangeSprite();
        PlaySound();

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
                case '①': StartCoroutine(_splashManager.Splash()); SoundManager.instance.PlaySound("Emotion0", 1); ignore = true; break;
                case '②': StartCoroutine(_splashManager.Splash()); SoundManager.instance.PlaySound("Emotion1", 1); ignore = true; break;
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

    private void ChangeSprite()
    {
        if (dialogues[lineCount].tf_target != null)
        {
            if (dialogues[lineCount].spriteName[contextCount] != "")
            {
                StartCoroutine(_spriteManager.SpriteChangeCoroutine(
                    dialogues[lineCount].tf_target,
                    dialogues[lineCount].spriteName[contextCount].Split(new char[] { '/' })[0]
                    ));
            }
        }
    }

    private string SplitSlideCGName()
    {
        string _text = dialogues[lineCount].spriteName[contextCount];
        string[] arr = _text.Split(new char[] { '/' });
        if (arr.Length <= 1)
        {
            return arr[0];
        }
        else
        {
            return arr[1];
        }
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
        else
        {
            dialogueNameBar.SetActive(false);
        }
    }
}
