using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_MainPanel_HeroPanel_UpgradeBtn : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{

    [Serializable]
    public class UpgradeEvent : UnityEvent<int>
    {
    }

    [SerializeField]
    public UpgradeEvent OnUpgrade;
 
    [SerializeField]
    private AnimationCurve _curve;
    [SerializeField]
    private float _maxTime;

    
    private void Awake()
    {
        EventManager.Inst.AddEvent(EventName.HeroUpgradede,OnHeroUpgrade);
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.HeroUpgradede,OnHeroUpgrade);
    }

    private bool _isDown;
    private float _downTime;
    private void OnHeroUpgrade(string arg1, object arg2)
    {
        _isDown = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDown = true;
        _downTime = Time.time;
        _timer = _downTime;
        OnUpgrade.Invoke(1);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDown = false;
    }

    [SerializeField] private float _minInterval;
    [SerializeField] private float _maxInterval;
    [ShowInInspector] [ReadOnly]private float _interval;
    private float _timer;
    
    private void Update()
    {
        if (_isDown)
        {
            if (_timer+_interval< Time.time)
            {
                _timer = Time.time;
                OnUpgrade.Invoke(1);
            }

            float t = Mathf.Clamp01((Time.time - _downTime) / _maxTime);
            t = _curve.Evaluate(t);
            _interval = Mathf.Lerp(_maxInterval, _minInterval, t);
            

            // float value = (Time.time - _downTime) / _maxTime;
            // value = _curve.Evaluate(value) * _maxMagnification;
            // if (value < 1)
            //     value = 1;
            // OnUpgrade.Invoke((int)value);
        }
    }

    [SerializeField]
    private AudioClip _audioClip;
    [SerializeField]
    private AnimationCurve _scaleCurve;

    public void UpgradeBtnEffect()
    {
        DOTween.Kill(transform);
        transform.localScale = Vector3.one;
        transform.DOScale(Vector3.one * 1.04f, _interval).SetEase(_scaleCurve);
        AudioManager.Inst.PlaySource(_audioClip);
    }


}
