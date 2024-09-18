using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public enum UIJoyStatus
{
    Idle,//闲置
    
    OnPressDown,//按下 1帧
    OnPressUp,//抬起 1帧
    Pressing,//按下中
    
    OnDragStart,//拖拽开始 1帧
    OnDragEnd,//拖拽结束 1帧
    Draging,//拖拽中
    
    OnSlide,//滑动 1帧
    
    OnHoldStart,//进入长按 1帧
    OnHoldEnd,//长按结束 1帧
    Holding,//长按中
    
    OnHoldDragStart,//长按拖拽开始 1帧
    OnHoldDragEnd,//长按拖拽结束 1帧
    HoldDraging,//长按 拖拽中
    
    OnHoldSlide,//长按滑动 1帧
}

public class UI_Joy : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    [SerializeField]
    [BoxGroup("引用")]
    private Transform root;
    [SerializeField]
    [BoxGroup("引用")]
    private Transform joyPoint;
    // [SerializeField]
    // [BoxGroup("引用")]
    // private RectTransform ui_Canvas;
    
    [SerializeField]
    [LabelText("最大拖拽距离")]
    [BoxGroup("参数")]
    [GUIColor(1,1,0)]
    private float joyMaxDis;
    [SerializeField]
    [LabelText("最大滑动距离")]
    [BoxGroup("参数")]
    [GUIColor(1,1,0)]
    private float slideXDis = 0.15f;
    [SerializeField]
    [LabelText("最大滑动时长")]
    [BoxGroup("参数")]
    [GUIColor(1,1,0)]
    private float slideTime = 0.2f;
    [SerializeField] 
    [LabelText("长按时间")]
    [BoxGroup("参数")]
    [GUIColor(1,1,0)]
    private float LongPressTime = 0.2f;
    // [SerializeField]
    // [LabelText("死区 0f-1f")]
    // [BoxGroup("参数")]
    // [GUIColor(1,1,0)]
    // private float deadZone = 0.18f;


    [LabelText("当前帧状态")] [BoxGroup("状态")] [ReadOnly] [ShowInInspector] [GUIColor(0, 1, 0)]
    public UIJoyStatus CurrentStatus => currentStatus.JoyStatus;
    
    
    public bool IsInDeadZone
    {
        get
        {
            float joyDisValue = value.magnitude / joyMaxDis;
            float deadZone = BattleManager.Inst.CurrentPlayer ? BattleManager.Inst.CurrentPlayer.JoyDeadZone : 0;
            return joyDisValue <= deadZone;
        }
    }
    //当前摇杆状态
    private JoyStatusData currentStatus;
    private List<JoyStatusData> _joyStatusList = new List<JoyStatusData>();
    
    //当前摇杆方向，包含滑动距离。
    private Vector2 value;
    //最终滑动距离
    private float _slideDis;
    //当前本次操作 还能否触发长按 （ 离开死驱之后 本次操作不再可以触发长按 ）
    private bool isNotLeaveDeadZone;
    private void Awake()
    {
        _slideDis = Screen.width * slideXDis;
#if UNITY_EDITOR
        _slideDis = 300;
#endif
        currentStatus = new JoyStatusData()
        {
            FrameCount = Time.frameCount,
            JoyStatus = UIJoyStatus.Idle,
            JoyValue = Vector2.zero,
            Time = Time.time,
            TouchPos = Vector2.zero
        };
        
        EventManager.Inst.AddEvent(EventName.JoyUp,JoyUp);
    }
    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.JoyUp,JoyUp);
    }
    private void OnDisable()
    {
        JoyUp(null,null);
    }

    private void Update()
    {
        UpdateCurrentStatus();
    }
    private void LateUpdate()
    {
        UpdateStatusList();
        LateUpdateSendJoyStatus();
    }

    
    private void JoyUp(string arg1, object arg2)
    {
        root.gameObject.SetActive(false);
        joyPoint.gameObject.SetActive(false);
        // SetCurrentStatus(UIJoyStatus.Idle);
        
        currentStatus.JoyStatus = UIJoyStatus.Idle;
        currentStatus.FrameCount = Time.frameCount;
        
        // isDown = false;
        // EventManager.Inst.DistributeEvent(EventName.JoyTouch,isDown);
        value = Vector2.zero;

        var data = new PointerEventData(EventSystem.current);
        data.pointerId = pointerId;
        data.position = currentStatus.TouchPos;
        OnPointerUp(data);
    }
    
    private int pointerId;//本次按下 触控ID
    private float downTime;//本次按下 时间
    public void OnPointerDown(PointerEventData eventData)
    {
        if (CurrentStatus != UIJoyStatus.Idle)
        {
            return;
        }

        isNotLeaveDeadZone = true;
        pointerId = eventData.pointerId;
        root.gameObject.SetActive(true);
        joyPoint.gameObject.SetActive(true);
        root.position = eventData.position;
        joyPoint.position = eventData.position;
        
        downTime = Time.time;

        currentStatus.Time = Time.time;
        currentStatus.TouchPos = eventData.position;
        currentStatus.JoyValue = Vector2.zero;
        SetCurrentStatus(UIJoyStatus.OnPressDown);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (pointerId!=eventData.pointerId)
        {
            return;
        }
        root.gameObject.SetActive(false);
        joyPoint.gameObject.SetActive(false);
        
        
        currentStatus.Time = Time.time;
        currentStatus.TouchPos = eventData.position;
        currentStatus.JoyValue = value;
        CheckPointerUp(eventData);
        value = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (pointerId != eventData.pointerId)
        {
            return;
        }

        joyPoint.position = eventData.position;
        var v3 = joyPoint.position - root.position;
        float joyDisValue = v3.magnitude / joyMaxDis;
        if (joyDisValue>1f)
        {
            var targetV3 = v3.normalized * joyMaxDis;
            joyPoint.position = root.position + targetV3;
        }
        
        value = new Vector2(v3.x,v3.y);
        
        currentStatus.Time = Time.time;
        currentStatus.TouchPos = eventData.position;
        currentStatus.JoyValue = value;
    }
    void CheckPointerUp(PointerEventData eventData)
    {
        Vector2 checkSlideValue = CheckSlide();
        currentStatus.SlideDir = checkSlideValue;
        switch (CurrentStatus)
        {
            case UIJoyStatus.Idle:
                SetCurrentStatus(UIJoyStatus.Idle);
                break;
            case UIJoyStatus.OnPressUp:
            case UIJoyStatus.OnDragEnd:
            case UIJoyStatus.OnSlide:
            case UIJoyStatus.OnHoldEnd:
            case UIJoyStatus.OnHoldDragEnd:
            case UIJoyStatus.OnHoldSlide:
                Debug.LogError($"#Joy# 错误：摇杆抬起时状态错误:{CurrentStatus}");
                break;
            case UIJoyStatus.OnPressDown://按下
            case UIJoyStatus.Pressing://按下中
                if (checkSlideValue !=Vector2.zero)
                {
                    SetCurrentStatus(UIJoyStatus.OnSlide);
                }
                else
                {
                    if (IsInDeadZone && isNotLeaveDeadZone)
                    {
                        SetCurrentStatus(UIJoyStatus.OnPressUp);
                    }
                    else
                    {
                        //不在死区里就是拖拽了。
                        SetCurrentStatus(UIJoyStatus.OnDragEnd);
                    }
                }
                break;
            case UIJoyStatus.OnDragStart:
            case UIJoyStatus.Draging://拖拽中
                if (checkSlideValue !=Vector2.zero)
                {
                    SetCurrentStatus(UIJoyStatus.OnSlide);
                }
                else
                {
                    SetCurrentStatus(UIJoyStatus.OnDragEnd);
                }
                break;
            case UIJoyStatus.OnHoldStart:
            case UIJoyStatus.Holding://长按中
                if (checkSlideValue !=Vector2.zero)
                {
                    SetCurrentStatus(UIJoyStatus.OnHoldSlide);
                }
                else
                {
                    SetCurrentStatus(UIJoyStatus.OnHoldEnd);
                }
                break;
            case UIJoyStatus.OnHoldDragStart:
            case UIJoyStatus.HoldDraging://长按拖拽
                if (checkSlideValue !=Vector2.zero)
                {
                    SetCurrentStatus(UIJoyStatus.OnHoldSlide);
                }
                else
                {
                    SetCurrentStatus(UIJoyStatus.OnHoldDragEnd);
                }
                break;
        }
        
        _joyStatusList.Clear();
    }



    // Update 更新当前状态 
    private void UpdateCurrentStatus()
    {
        switch (CurrentStatus)
        {
            case UIJoyStatus.Idle:
            case UIJoyStatus.OnDragEnd:
            case UIJoyStatus.OnSlide:
            case UIJoyStatus.OnHoldEnd:
            case UIJoyStatus.OnHoldDragEnd:
            case UIJoyStatus.OnHoldSlide:
            case UIJoyStatus.OnPressUp:
                if (IsInDeadZone)
                {
                    SetCurrentStatus(UIJoyStatus.Idle);
                }
                else
                {
                    SetCurrentStatus(UIJoyStatus.Draging);
                }
                break;
            case UIJoyStatus.OnPressDown:
                SetCurrentStatus(UIJoyStatus.Pressing);
                break;
            case UIJoyStatus.Pressing:
                //计算时长 进入长按
                if (Time.time - downTime >= LongPressTime && IsInDeadZone && isNotLeaveDeadZone)
                {
                    SetCurrentStatus(UIJoyStatus.OnHoldStart);
                }
                else
                {
                    if (IsInDeadZone)
                    {
                        SetCurrentStatus(UIJoyStatus.Pressing);
                    }
                    else
                    {
                        SetCurrentStatus(UIJoyStatus.OnDragStart);
                        isNotLeaveDeadZone = false;
                    }
                }
                break;
            case UIJoyStatus.OnDragStart:
                SetCurrentStatus(UIJoyStatus.Draging);
                break;
            case UIJoyStatus.Draging:
                if (IsInDeadZone)
                {
                    SetCurrentStatus(UIJoyStatus.Idle);
                }
                else
                {
                    SetCurrentStatus(UIJoyStatus.Draging);
                }
                break;
            case UIJoyStatus.OnHoldStart:
                SetCurrentStatus(UIJoyStatus.Holding);
                break;
            case UIJoyStatus.Holding:
                if (IsInDeadZone)
                {
                    SetCurrentStatus(UIJoyStatus.Holding);
                }
                else
                {
                    SetCurrentStatus(UIJoyStatus.OnHoldDragStart);
                }
                break;
            case UIJoyStatus.OnHoldDragStart:
                SetCurrentStatus(UIJoyStatus.HoldDraging);
                break;
            case UIJoyStatus.HoldDraging:
                if (IsInDeadZone)
                {
                    SetCurrentStatus(UIJoyStatus.Holding);
                }
                else
                {
                    SetCurrentStatus(UIJoyStatus.HoldDraging);
                }
                break;
        }
    }
    
    //每一帧结尾 记录摇杆状态
    private void UpdateStatusList()
    {
        _joyStatusList.Add(currentStatus.Copy());
        
        //清除超时的摇杆状态
        List<JoyStatusData> removeIndex = new List<JoyStatusData>();
        for (int i = 0; i < _joyStatusList.Count; i++)
        {
            if (Time.time - _joyStatusList[i].Time > slideTime)
            {
                removeIndex.Add(_joyStatusList[i]);
            }
        }
        for (int i = 0; i < removeIndex.Count; i++)
        {
            _joyStatusList.Remove(removeIndex[i]);
        }
    }

    private void LateUpdateSendJoyStatus()
    {
        currentStatus.SendFrameCount = Time.frameCount;
        EventManager.Inst.DistributeEvent(EventName.JoyStatus,currentStatus);
    }
    
    private void SetCurrentStatus(UIJoyStatus status)
    {
        if (currentStatus.FrameCount == Time.frameCount)
        {
            //当前帧已经设置过了。。
            return;
        }

        if (currentStatus.JoyStatus == status)
        {
            //已经是当前状态
            return;
        }
//        Debug.LogError($"#Joy# 更改状态{status}");
        currentStatus.JoyStatus = status;
        currentStatus.FrameCount = Time.frameCount;
    }


    //检查是否满足滑动，如果满足返回带距离的方向，否则返回Zero
    Vector2 CheckSlide()
    {
        for (int i = _joyStatusList.Count-1; i >= 0; i--)
        {
            var dir = currentStatus.TouchPos - _joyStatusList[i].TouchPos;
            if (dir.magnitude > _slideDis)
            {
                return dir;
            }
        }
        return Vector2.zero;
    }
    
}

public struct JoyStatusData
{
    public UIJoyStatus JoyStatus;//摇杆状态
    public Vector2 JoyValue;//摇杆Value
    public Vector2 TouchPos;//手指位置
    public Vector2 SlideDir;//滑动方向
    public int FrameCount;//第几帧
    public int SendFrameCount;//第几帧 广播出去的数据
    public float Time;//时间

    public JoyStatusData Copy()
    {
        return new JoyStatusData()
        {
            JoyStatus = JoyStatus,
            JoyValue = JoyValue,
            TouchPos = TouchPos,
            FrameCount = FrameCount,
            SendFrameCount = SendFrameCount,
            Time = Time,
            SlideDir=SlideDir,
        };
    }
}
