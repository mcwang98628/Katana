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
#if UNITY_ANDROID || UNITY_IOS
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
#else

    protected virtual void OnInput()
    {
        Vector2 moveVec = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveVec = Quaternion.Euler(0, 0, 45) * moveVec;
        roleController.InputMove(moveVec);
    }
#endif

    //暂时只有katana用，直接修改加速度
    public void SetMoveSpeedMax()
    {
        moveAcceleration = 9999;
        currentMoveSpeed = MaxMoveSpeed;
        roleController.Animator.SetFloat(MoveSpeed, 1);
    }
}
