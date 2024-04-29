using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideManager : MonoBehaviour
{
    [SerializeField] private Transform slideTransform;
    [SerializeField] private Image slideCG;
    [SerializeField] private Animator anim;

    public static bool isFinished= false;
    public static bool isChanged= false;

    public IEnumerator AppearSlide(string slideName)
    {
        Sprite _sprite = Resources.Load<Sprite>("Slide_Image/" + slideName);
        if (_sprite !=  null )
        {
            slideCG.gameObject.SetActive(true);
            slideCG.sprite = _sprite;
            anim.SetTrigger("Appear");
        }
        else
        {
            Debug.Log(slideName + " �����̵� �̹��� ������ �����ϴ�");
        }

        yield return new WaitForSeconds(0.5f);
        isFinished = true;
    }

    public IEnumerator DisappearSlide()
    {
        anim.SetTrigger("Disappear");
        yield return new WaitForSeconds(0.5f);
        slideCG.gameObject.SetActive(false);
        isFinished = true;
    }

    public IEnumerator ChangeSlide(string sildeName)
    {
        isFinished = false;
        StartCoroutine(DisappearSlide());
        yield return new WaitUntil(() => isFinished);

        isFinished = false;
        StartCoroutine(AppearSlide(sildeName));
        yield return new WaitUntil(() => isFinished);

        isChanged = true;
    }
}
