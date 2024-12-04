using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PanelBase : MonoBehaviour
{
    protected CanvasScaler _canvasScaler;
    protected CanvasGroup _canvasGroup;
    public bool IsShow { get; protected set; }
    public virtual void Show()
    {
        if (gameObject==null)
        {
            return;
        }
        gameObject.SetActive(true);
        IsShow = true;
        if (_canvasGroup == null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        if (_canvasScaler == null)
        {
            _canvasScaler = GetComponent<CanvasScaler>();
        }

        if (_canvasScaler != null)
        {
            float value = (float) Screen.width / (float) Screen.height; 
            _canvasScaler.matchWidthOrHeight = value>0.57f ? 1 : 0;
        }

        _canvasGroup.interactable = false;
        _canvasGroup.alpha = 0;
        _canvasGroup.DOKill(true);
        _canvasGroup.DOFade(1, 0.3f).SetEase(Ease.OutQuart).OnComplete(() =>
        {
            _canvasGroup.interactable = true;
            // GameManager.Inst.StartCoroutine(waitInteractable());
        });
        OnPause();
    }

    protected IEnumerator waitInteractable()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        _canvasGroup.interactable = true;
    }

    public virtual void Hide(Action callBack)
    {
        IsShow = false;
        _canvasGroup.interactable = false;
        _canvasGroup.DOKill(true);
        _canvasGroup.DOFade(0, 0.2f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            // callBack.Invoke();
        });
        OnUnPause();
    }
    protected virtual void OnPause()
    {
        TimeManager.Inst.Pause();
        // Debug.LogError("Pause:"+gameObject.name);
    }
    protected virtual void OnUnPause()
    {
        TimeManager.Inst.UnPause();
        // Debug.LogError("UnPause:"+gameObject.name);
    }
}
