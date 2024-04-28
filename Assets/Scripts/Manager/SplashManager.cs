using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashManager : MonoBehaviour
{
    [SerializeField] private Image targetImage;

    [SerializeField] private Color colorWhite;
    [SerializeField] private Color colorBlack;

    [SerializeField] private float fadeSpeed;
    [SerializeField] private float fadeSlowSpeed;

    public static bool isFinished = false;

    public IEnumerator Splash()
    {
        isFinished = false;
        StartCoroutine(FadeOut(true, false));
        yield return new WaitUntil(()=> isFinished);
        isFinished = false;
        StartCoroutine(FadeIn(true, false));
    }

    public IEnumerator FadeOut(bool isWhite, bool isSlow)
    {
        Color _color = isWhite ? colorWhite : colorBlack;
        _color.a = 0;

        targetImage.color = _color;

        while(_color.a < 1)
        {
            _color.a += isSlow ? fadeSlowSpeed : fadeSpeed;
            targetImage.color = _color;
            yield return null;
        }

        isFinished = true;
    }

    public IEnumerator FadeIn(bool isWhite, bool isSlow)
    {
        Color _color = isWhite ? colorWhite : colorBlack;
        _color.a = 1;

        targetImage.color = _color;

        while(_color.a > 0)
        {
            _color.a -= isSlow ? fadeSlowSpeed : fadeSpeed;
            targetImage.color = _color;
            yield return null;
        }

        isFinished = true;
    }
}
