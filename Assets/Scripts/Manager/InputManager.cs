using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Inst { get; private set; }
    private JoyStatusData currentJoyStatusdata;
    [SerializeField]
    private float AutoWaitTime = 1;
    

    private void Awake()
    {
        Inst = this;
        EventManager.Inst.AddEvent(EventName.JoyStatus, OnJoyStatus);
        // EventManager.Inst.AddEvent(EventName.JoyTouch, OnJoyTouch);
        // EventManager.Inst.AddEvent(EventName.JoyValue, OnJoyValue);
        // EventManager.Inst.AddEvent(EventName.JoySlide, OnJoySlide);
        // EventManager.Inst.AddEvent(EventName.JoyClick, OnJoyClick);
        // EventManager.Inst.AddEvent(EventName.JoyLongPress, JoyLongPress);
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.JoyStatus, OnJoyStatus);
        // EventManager.Inst.RemoveEvent(EventName.JoyTouch, OnJoyTouch);
        // EventManager.Inst.RemoveEvent(EventName.JoyValue, OnJoyValue);
        // EventManager.Inst.RemoveEvent(EventName.JoySlide, OnJoySlide);
        // EventManager.Inst.RemoveEvent(EventName.JoyClick, OnJoyClick);
        // EventManager.Inst.RemoveEvent(EventName.JoyLongPress, JoyLongPress);
    }


    private void OnJoyStatus(string arg1, object value)
    {
        currentJoyStatusdata = (JoyStatusData) value;
    }

    private void Update()
    {
        if (GameManager.Inst == null ||
            BattleManager.Inst == null ||
            BattleManager.Inst.CurrentPlayer == null ||
            BattleManager.Inst.CurrentPlayer.IsDie)
        {
            return;
        }
#if UNITY_ANDROID || UNITY_IOS

        if (currentJoyStatusdata.JoyStatus == UIJoyStatus.OnPressUp)
        {
            InteractManager.Inst.TryInteract();
        }

        if (Time.frameCount - currentJoyStatusdata.SendFrameCount > 1)
        {
            currentJoyStatusdata.JoyStatus = UIJoyStatus.Idle;
        }
        
        BattleManager.Inst.CurrentPlayer.InputAction.OnInput(currentJoyStatusdata);
#else
        if (!Input.anyKeyDown)
        {
            InteractManager.Inst.TryInteract();
        }

        if (Time.frameCount - currentJoyStatusdata.SendFrameCount > 1)
        {
            currentJoyStatusdata.JoyStatus = UIJoyStatus.Idle;
        }

        BattleManager.Inst.CurrentPlayer.InputAction.OnInput();
#endif
        
        // switch (currentJoyStatusdata.JoyStatus)
        // {
        //     case UIJoyStatus.Idle:
        //     case UIJoyStatus.OnDragEnd:
        //     case UIJoyStatus.Pressing:
        //     case UIJoyStatus.OnPressDown:
        //         BattleManager.Inst.CurrentPlayer.InputMove(Vector2.zero);
        //         break;
        //     case UIJoyStatus.OnPressUp:
        //         if (!InteractManager.Inst.TryInteract())//捡东西
        //         {
        //             BattleManager.Inst.CurrentPlayer.InputAttack();
        //         }
        //         break;
        //     case UIJoyStatus.OnDragStart:
        //     case UIJoyStatus.Draging:
        //         BattleManager.Inst.CurrentPlayer.InputMove(GetCameraDir(currentJoyStatusdata.JoyValue));
        //         break;
        //     case UIJoyStatus.OnSlide:
        //         BattleManager.Inst.CurrentPlayer.InputRoll(GetCameraDir(currentJoyStatusdata.JoyValue));
        //         break;
        //     case UIJoyStatus.OnHoldStart:
        //         BattleManager.Inst.CurrentPlayer.InputAttackB();
        //         break;
        //     case UIJoyStatus.Holding:
        //         break;
        //     case UIJoyStatus.OnHoldEnd:
        //         break;
        //     case UIJoyStatus.OnHoldDragStart:
        //     case UIJoyStatus.HoldDraging:
        //         break;
        //     case UIJoyStatus.OnHoldDragEnd:
        //         break;
        //     case UIJoyStatus.OnHoldSlide:
        //         BattleManager.Inst.CurrentPlayer.InputRoll(GetCameraDir(currentJoyStatusdata.JoyValue));
        //         break;
        // }
        //
    }


    public Vector2 GetCameraDir(Vector2 v2)
    {
        if (Camera.main == null)
        {
            return Vector2.zero;
        }
        Vector3 targetDirection = new Vector3(v2.x, 0, v2.y);
        float y = Camera.main.transform.rotation.eulerAngles.y;
        targetDirection = Quaternion.Euler(0, y, 0) * targetDirection;
        Vector2 dir = new Vector2(targetDirection.x,targetDirection.z);
        return dir;
    }

}
