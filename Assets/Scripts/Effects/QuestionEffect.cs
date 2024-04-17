using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionEffect : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Vector3 targetPos;

    [SerializeField] private ParticleSystem effect;

    public void SetTarget(Vector3 target)
    {
        targetPos = target;
    }

    private void Update()
    {
        if (targetPos != Vector3.zero)
        {
            if ((transform.position - targetPos).sqrMagnitude >= 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed);
            }
            else
            {
                effect.gameObject.SetActive(true);
                effect.transform.position = transform.position;
                effect.Play();
                targetPos = Vector3.zero;
                gameObject.SetActive(false);
            }
        }
    }
}
