using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattlePanel_ItemBtn_Joy : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    public ItemUseType ItemUseType { get; set; }//有些Item不需要摇杆触发
    [SerializeField]
    private Transform joyPoint;

    [SerializeField]
    private float joyPointDis = 100f;

    private Vector2 joyValue
    {
        get
        {
            return new Vector2(joyPoint.localPosition.x,joyPoint.localPosition.y);
        }
    }

    [Serializable]
    public class JoyClickEvent : UnityEvent
    {}
    [Serializable]
    public class JoyTouchEvent : UnityEvent<bool>
    {}
    [Serializable]
    public class JoyDragEvent : UnityEvent<bool,Vector2>
    {}

    //只有不使用Joy的Item才需要使用 ClickEvent。
    public JoyClickEvent ClickEvent;
    public JoyTouchEvent TouchEvent;
    public JoyDragEvent DragEvent;
    
    public bool IsDown { get; private set; }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        IsDown = true;
        switch (ItemUseType)
        {
            case ItemUseType.Click:
                break;
            case ItemUseType.Hold:
                TouchEvent.Invoke(IsDown);
                break;
            case ItemUseType.Drag:
                joyPoint.gameObject.SetActive(true);
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsDown = false;
        switch (ItemUseType)
        {
            case ItemUseType.Click:
                ClickEvent.Invoke();
                break;
            case ItemUseType.Hold:
                TouchEvent.Invoke(IsDown);
                break;
            case ItemUseType.Drag:
                joyPoint.gameObject.SetActive(false);
                DragEvent.Invoke(IsDown,joyValue);
                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ItemUseType != ItemUseType.Drag)
        {
            return;
        }
        
        joyPoint.position = eventData.position;
        if (joyPoint.localPosition.magnitude>joyPointDis)
        {
            joyPoint.localPosition = joyPoint.localPosition.normalized * joyPointDis;
        }
        DragEvent.Invoke(IsDown,joyValue);
    }
}
