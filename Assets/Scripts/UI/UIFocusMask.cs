using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIFocusMask : MonoBehaviour
{
    [SerializeField]
    private RectTransform _canvasTransform;
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private RectTransform _round;
    [SerializeField]
    private RectTransform _clickMask;


    public void Focus(RectTransform uiTransform)
    {
        var targetValue = uiTransform.sizeDelta.x;
        if (targetValue < uiTransform.sizeDelta.y)
        {
            targetValue = uiTransform.sizeDelta.y;
        }
        
        Focus(targetValue,uiTransform.position);
    }

    public void Focus(float targetSize,Vector2 v2pos)
    {
        this.gameObject.SetActive(true);
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1, 0.3f);
        var value = _canvasTransform.sizeDelta.x;
        if (value < _canvasTransform.sizeDelta.y)
        {
            value = _canvasTransform.sizeDelta.y;
        }

        _clickMask.transform.position = v2pos;
        _clickMask.sizeDelta = Vector2.one * (targetSize*0.9f);
        _round.transform.position = v2pos;
        _round.sizeDelta = Vector2.one * value;
        _round.DOSizeDelta(Vector2.one * targetSize, 1.0f);//.SetLoops(-1,LoopType.Restart);
        
    }

    public void ShowClickMask(float targetSize,Vector2 v2pos)
    {
        _clickMask.transform.position = v2pos;
        _clickMask.sizeDelta = Vector2.one * (targetSize*0.9f);
    }
    

    public void Hide()
    {
        _canvasGroup.DOFade(0, 0.3f).OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }
    
}
