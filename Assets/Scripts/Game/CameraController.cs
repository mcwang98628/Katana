using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static CameraController inst_;
    public static CameraController Inst => inst_;

    [SerializeField] public float Distance;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 shakeOffset;
    [SerializeField] private Vector3 zoomOffset;

    [SerializeField] private Transform target;
    private Vector3 initAngle;
    private bool isFollowPlayer = true;
    [HideInInspector]
    public float PosLerp = 1;

    public Vector3 Offset
    {
        get => offset;
        set => offset = value;
    }
    public Vector3 ZoomOffset
    {
        get => zoomOffset;
        set => zoomOffset = value;
    }

    public void SetTarget(Transform target_)
    {
        target = target_;
        PosLerp = 1;
        Update();
    }
    private void Awake()
    {
        inst_ = this;
        initAngle = transform.eulerAngles;
    }

    private bool _cameraLerpMove = false;
    private void Update()
    {
        if (_cameraLerpMove)
            UpdateLerpCamreaPosition();
        else
            UpdateCamreaPosition();
    }

    public void StartCameraLerpMove()
    {
        _cameraLerpMove = true;
    }
    void UpdateCamreaPosition()
    {
        if (target == null)
        {
            return;
        }
        if (isFollowPlayer)
        {
            Vector3 targetPos = target.position - transform.forward * Distance + offset + shakeOffset + zoomOffset;
            transform.position = targetPos;
        }
    }
    void UpdateLerpCamreaPosition()
    {
        if (target == null)
        {
            return;
        }
        if (isFollowPlayer)
        {
            Vector3 targetPos = target.position - transform.forward * Distance + offset + shakeOffset + zoomOffset;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime*10f);
            if ((transform.position - targetPos).magnitude <= 0.1f)
                _cameraLerpMove = false;
        }
    }


    public void LookAtPoint(Vector3 pos, float delayTime, float enterTime, float stayTime, float exitTime)
    {
        isFollowPlayer = false;
        StartCoroutine(DelayResetFollowPlayer(delayTime+enterTime + stayTime + exitTime));

        EventManager.Inst.DistributeEvent(EventName.OnMoveCamera,true);
        Vector3 targetPos = pos - transform.forward * Distance + offset + shakeOffset + zoomOffset;
        var sequence = DOTween.Sequence();
        sequence.AppendInterval(delayTime)
            .Append(transform.DOMove(targetPos, enterTime))
            .AppendInterval(stayTime).OnComplete(() =>
            {
                transform.DOMove(target.position - transform.forward * Distance + offset + shakeOffset + zoomOffset,
                    exitTime).OnComplete(() =>
                {
                    EventManager.Inst.DistributeEvent(EventName.OnMoveCamera,false);
                });
            });
    }
    IEnumerator DelayResetFollowPlayer(float time)
    {
        yield return new WaitForSeconds(time);
        isFollowPlayer = true;
    }


    public void DoShakeOffset(float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool snapping = false, bool fadeOut = true)
    {
        DOTween.Shake((() => shakeOffset), (x => shakeOffset = x), duration, strength, vibrato, randomness, fadeOut)
            .SetSpecialStartupMode(SpecialStartupMode.SetShake)
            .SetOptions(snapping)
            .OnComplete(() => { shakeOffset = Vector3.zero; })
            .SetEase(Ease.Linear);
    }

    public void DoShakeRotation(float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
    {
        transform.DOShakeRotation(duration, strength, vibrato, randomness, fadeOut).OnComplete(
            () => transform.eulerAngles = initAngle);
    }
    public void DoZoom(float value, float time_)
    {
        DOTween.To(() => zoomOffset, x => zoomOffset = x, transform.forward * value, time_).SetEase(Ease.Linear);//.SetEase(zoomCurve);
    }
    public void DoZoom(Vector3 value, float time_)
    {
        DOTween.To(() => zoomOffset, x => zoomOffset = x, value, time_).SetEase(Ease.Linear);//.SetEase(zoomCurve);
    }


}
