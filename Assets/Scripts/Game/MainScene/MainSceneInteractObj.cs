using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class MainSceneInteractObj : MonoBehaviour
{
    public Transform Model;
    public Transform Halo;
    
    public AnimationCurve XCurve;
    public AnimationCurve YCurve;
    
    [SerializeField]
    public UnityEvent InteractEvent = new UnityEvent();
    [SerializeField]
    public UnityEvent OnSelectEvent = new UnityEvent();
    [SerializeField]
    public UnityEvent OnUnSelectEvent = new UnityEvent();
    
    public void OnSelect()
    {
        SelectAnim();
        OnSelectEvent.Invoke();
    }

    public void OnUnSelect()
    {
        UnSelectAnim();
        OnUnSelectEvent.Invoke();
    }

    private Tweener ModelX;
    private Tweener ModelY;
    private Tweener HaloScale;

    void killAllTweener()
    {
        if (ModelX!=null)
        {
            ModelX.Kill(true);
        }
        if (ModelY!=null)
        {
            ModelY.Kill(true);
        }
        if (HaloScale!=null)
        {
            HaloScale.Kill(true);
        }
    }
    void SelectAnim()
    {
        killAllTweener();
        ModelX = Model.DOScaleX(1.5f, 0.4f).SetEase(XCurve);
        ModelY = Model.DOScaleY(1.5f, 0.4f).SetEase(YCurve);
        Halo.gameObject.SetActive(true);
        Halo.localScale = Vector3.one * 1.5f;
        HaloScale = Halo.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
    }

    void UnSelectAnim()
    {
        killAllTweener();
        Halo.localScale = Vector3.one;
        HaloScale = Halo.DOScale(Vector3.one * 1.5f, 0.1f).SetEase(Ease.Linear).OnComplete(() => { 
            Halo.gameObject.SetActive(false);
        });
    }
    
}
