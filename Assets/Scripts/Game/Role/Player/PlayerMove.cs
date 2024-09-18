using UnityEngine;

public class PlayerMove : RoleMove
{
    void Start()
    {
        ((PlayerController) roleController).InputAction.OnInputEvent += OnInput;
    }

    
    private void OnDestroy()
    {
        // ReSharper disable once DelegateSubtraction
        ((PlayerController) roleController).InputAction.OnInputEvent -= OnInput;
    }

    protected UIJoyStatus _joyStatus;
    protected Vector2 _dirValue;
    protected virtual void OnInput(JoyStatusData statusData)
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
            case UIJoyStatus.OnHoldDragEnd:
            case UIJoyStatus.OnHoldSlide:
                roleController.InputMove(Vector2.zero);
                break;
            case UIJoyStatus.Draging:
            case UIJoyStatus.HoldDraging:
            case UIJoyStatus.OnHoldDragStart:
                roleController.InputMove(InputManager.Inst.GetCameraDir(_dirValue));
                break;
        }
    }

    //暂时只有katana用，直接修改加速度
    public void SetMoveSpeedMax()
    {
        moveAcceleration = 9999;
        currentMoveSpeed = MaxMoveSpeed;
        roleController.Animator.SetFloat(MoveSpeed, 1);
    }
}
