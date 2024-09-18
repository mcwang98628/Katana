using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScaleAnim : MonoBehaviour
{
    [SerializeField]
    private AudioClip soundOnInit;

    [SerializeField] private float initTime=0.25f;
    private CanvasGroup _canvasGroup;

    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
       if (soundOnInit)
            AudioManager.Inst.PlaySource(soundOnInit);
        
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        
        _canvasGroup.interactable = false;
        
        transform.localScale=Vector3.zero;
        transform.DOScale(1, initTime).SetEase(Ease.OutBack);
        StartCoroutine(DelayCanInteract(initTime));
    }

    IEnumerator DelayCanInteract(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        _canvasGroup.interactable = true;
    }
    
}
