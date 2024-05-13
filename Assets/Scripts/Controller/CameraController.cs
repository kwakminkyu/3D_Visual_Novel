using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 originPos;
    private Quaternion originRot;

    private PlayerController _playerController;
    private InteractionController _interactionController;

    private Coroutine _coroutine;

    private void Start()
    {
        _playerController = FindAnyObjectByType<PlayerController>();
        _interactionController = FindAnyObjectByType<InteractionController>();
    }

    public void CamOriginSetting()
    {
        originPos = transform.position;
        originRot = Quaternion.Euler(0,0,0);
    }

    public void CameraTargetting(Transform target, float camSpeed = 0.1f, bool isReset = false, bool isFinish = false)
    {
        if (!isReset)
        {
            if (target != null)
            {
                StopAllCoroutines();
                _coroutine = StartCoroutine(CameraTargettingCoroutine(target, camSpeed));
            }
        }
        else
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            StartCoroutine(CameraResetCoroutine(camSpeed, isFinish));
        }
    }
    private IEnumerator CameraTargettingCoroutine(Transform target, float camSpeed)
    {
        Vector3 targetPos = target.position;
        Vector3 targetFrontPos = target.position + (target.forward * 1.2f);
        Vector3 direction = (targetPos - targetFrontPos).normalized;

        while(transform.position != targetFrontPos || Quaternion.Angle(transform.rotation, Quaternion.LookRotation(direction)) >= 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetFrontPos, camSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), camSpeed);
            yield return null;
        }
    }

    private IEnumerator CameraResetCoroutine(float camSpeed = 0.1f, bool isFinish = false)
    {
        yield return new WaitForSeconds(0.5f);
        
        while (transform.position != originPos || Quaternion.Angle(transform.rotation, originRot) >= 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originPos, camSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, originRot, camSpeed);
            yield return null;
        }
        transform.position = originPos;

        if (isFinish)
        {
            _playerController.ResetCam();
            InteractionController.isInteract = false;
        }
    }
}
