using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    private Transform tf_target;

    private bool spin = false;
    public static bool isFinished = true;

    private void Start()
    {
        tf_target = PlayerController.instance.transform;    
    }

    private void Update()
    {
        if (tf_target != null)
        {
            if (!spin)
            {
                Quaternion t_rotation = Quaternion.LookRotation(tf_target.position - transform.position);
                Vector3 t_euler = new Vector3(0, t_rotation.eulerAngles.y, 0);
                transform.eulerAngles = t_euler;
            }
            else
            {
                transform.Rotate(0, 90 * Time.deltaTime * 8, 0);
            }
        }
    }

    public IEnumerator SetAppearOrDisappear(bool flag)
    {
        spin = true;

        SpriteRenderer[] _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        Color frontColor = _spriteRenderers[0].color;
        Color backColor = _spriteRenderers[1].color;

        if (flag)
        {
            frontColor.a = 0;
            backColor.a = 0;
            _spriteRenderers[0].color = frontColor;
            _spriteRenderers[1].color = backColor;
        }
        float fadeSpeed = flag ? 0.01f : -0.01f;

        yield return new WaitForSeconds(0.3f);

        while (true)
        {
            if (flag && frontColor.a >= 1)
                break;
            else if (!flag && frontColor.a <= 0)
                break;

            frontColor.a += fadeSpeed;
            backColor.a += fadeSpeed;
            _spriteRenderers[0].color = frontColor;
            _spriteRenderers[1].color = backColor;
            yield return null;
        }

        spin = false;
        isFinished = true;
        gameObject.SetActive(flag);
    }
}
