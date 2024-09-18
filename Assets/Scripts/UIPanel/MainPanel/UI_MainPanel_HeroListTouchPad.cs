using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MainPanel_HeroListTouchPad : MonoBehaviour,IEndDragHandler
{
    private ScrollRect _scrollRect;
    void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }


    private Coroutine _wait;
    public void OnEndDrag(PointerEventData eventData)
    {
        if (_wait!=null)
            StopCoroutine(_wait);
        _wait = StartCoroutine(waitMove());
    }

    IEnumerator waitMove()
    {
        yield return new WaitForSeconds(0.3f);
        var cellX = 1f / _scrollRect.content.childCount;
        var oldValue = _scrollRect.horizontalNormalizedPosition;
        var value1 = oldValue % cellX;
        if (value1 > cellX/2f)
            oldValue += cellX - value1;
        else
            oldValue -= value1;
        if (oldValue>1)
            oldValue = 1;
        else if (oldValue < 0)
            oldValue = 0;
        _scrollRect.DOHorizontalNormalizedPos(oldValue,0.3f);
    }
}
