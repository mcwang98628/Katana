using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerShieldMove : PlayerMove
{
    //[SerializeField][LabelText("举盾移速")]
    //private float shieldMoveSpeed;

    //[SerializeField][LabelText("举盾转速")]
    //private float HoldingTurningAngSpeed;

    
    //public override float CurrentMoveSpeed => roleController.EnemyTarget!=null && _dirValue.magnitude > 0 &&
    //                                          (_joyStatus == UIJoyStatus.Draging) 
    //                                           //|| _joyStatus == UIJoyStatus.HoldDraging)
    //    ? shieldMoveSpeed
    //    : base.CurrentMoveSpeed;

    //public override float TurningAngSpeed => roleController.EnemyTarget!=null
    //    ? HoldingTurningAngSpeed
    //    : base.TurningAngSpeed;

    protected override void OnInput(JoyStatusData statusData)
    {
        _joyStatus = statusData.JoyStatus;
        _dirValue = statusData.JoyValue;
        switch (_joyStatus)
        {
            case UIJoyStatus.Idle:
            case UIJoyStatus.OnPressDown:
            case UIJoyStatus.Pressing:
            case UIJoyStatus.OnPressUp:
            case UIJoyStatus.OnDragStart:
            case UIJoyStatus.OnDragEnd:
            case UIJoyStatus.OnSlide:
            case UIJoyStatus.OnHoldStart:
            case UIJoyStatus.OnHoldEnd:
            case UIJoyStatus.Holding:
            case UIJoyStatus.OnHoldDragStart:
            case UIJoyStatus.OnHoldDragEnd:
            case UIJoyStatus.OnHoldSlide:
                roleController.InputMove(Vector2.zero);
                break;
            case UIJoyStatus.Draging:
            case UIJoyStatus.HoldDraging:
                roleController.InputMove(InputManager.Inst.GetCameraDir(_dirValue));
                break;
        }
    }

    public void SetShieldMoveSpeed(float speed)
    {
        //shieldMoveSpeed = speed;
    }
}
