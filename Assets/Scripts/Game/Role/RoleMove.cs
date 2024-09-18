using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class RoleMove : MonoBehaviour
{
    [LabelText("使用加速度")]
    [SerializeField]
    protected bool isUseAcceleration;
    [ShowIf("isUseAcceleration")]
    [LabelText("Start加速度")]
    [SerializeField]
    protected float startAcceleration;
    [ShowIf("isUseAcceleration")]
    [LabelText("End加速度")]
    [SerializeField]
    protected float stopAcceleration;

    //[ShowInInspector]
    [LabelText("当前移动速度")]
    public virtual float CurrentMoveSpeed => currentMoveSpeed;
    //[ShowInInspector]
    [LabelText("当前加速度")]
    public float MoveAcceleration => moveAcceleration;

    [SerializeField] [LabelText("转向速度")] 
    protected float turningAngSpeed;

    public virtual float TurningAngSpeed => turningAngSpeed;// + roleController.GetAttributeBonusValue(AttributeType.TurningAngSpeed);

    public float MinMoveSpeed
    {
        get
        {
            float value = minMoveSpeed + roleController.GetAttributeBonusValue(AttributeType.MoveSpeed);
            if (value<1f)
            {
                value = 1f;
            }

            return value;
        }
    }
    public float MaxMoveSpeed
    {
        get
        {
            float value = maxMoveSpeed + roleController.GetAttributeBonusValue(AttributeType.MoveSpeed);
            if (value<1f)
            {
                value = 1f;
            }

            return value;
        }
    }
    [SerializeField]
    private float minMoveSpeed;
    [SerializeField]
    private float maxMoveSpeed;


    protected static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");

    protected RoleController roleController;

    protected Vector2 inputV2;
    public void InputMoveDir(Vector2 value)
    {
        inputV2 = value;
    }

    public void StopMove()
    {
        inputV2=Vector2.zero;
    }

    private void Awake()
    {
        roleController = GetComponent<RoleController>();
    }


    public void SetAcceleration(bool isUseAccel, float startAccel, float stopAccel)
    {
        isUseAcceleration = isUseAccel;
        startAcceleration = startAccel;
        stopAcceleration = stopAccel;
    }
    public void SetMultiplyMoveSpeed(float value)
    {
        minMoveSpeed *= value;
        maxMoveSpeed *= value;
        turningAngSpeed *= value;
    }
    public void SetExceptMoveSpeed(float value)
    {
        minMoveSpeed /= value;
        maxMoveSpeed /= value;
        turningAngSpeed /= value;
    }

    protected virtual void Update()
    {
        if ((!roleController.IsFastMoving && !roleController.IsMoving) || roleController.IsFreeze)
        {
            roleController.Rigidbody.velocity = Vector3.zero;
        }
        Move(inputV2);
    }

    protected float currentMoveSpeed;
    protected float moveAcceleration;

    protected virtual void Move(Vector2 dirV2)
    {
        if (dirV2.magnitude > 0.1)
        {
            roleController.SetIsMoving(true);
        }

        var dir = Vector3.zero;
        if (dirV2 != Vector2.zero)
        {
            var forward = roleController.Animator.transform.forward;
            float ang = Vector3.Angle(forward, new Vector3(dirV2.x, 0, dirV2.y));
            float value = 1;
            if (Mathf.Abs(ang)>0 && TurningAngSpeed>0)
            {
                value =(TurningAngSpeed*Time.deltaTime)/ang;
                if (value>1)
                {
                    value = 1;
                }
            }
            dir = Vector3.SlerpUnclamped(forward, new Vector3(dirV2.x, 0, dirV2.y), value);
        }
        MoveTransform(dir);
        MoveAnim(dir);
    }

    protected virtual void MoveTransform(Vector3 dir)
    {
        if (!roleController.IsCanMove)
        {
            
            roleController.Rigidbody.velocity = Vector3.zero;
            return;
        }
        if (dir == Vector3.zero)
        {
            ClearMoveSpeed();
            if (roleController.Rigidbody.velocity.magnitude > 0.1f && isUseAcceleration)
            {
                var v3 = stopAcceleration * Time.deltaTime * roleController.Rigidbody.velocity.normalized;
                if (v3.magnitude > roleController.Rigidbody.velocity.magnitude)
                {
                    v3 = v3.normalized * roleController.Rigidbody.velocity.magnitude;
                }
                roleController.Rigidbody.velocity -= v3;
            }
            else
            {
                roleController.Rigidbody.velocity = Vector3.zero;
                roleController.SetIsMoving(false);
            }
        }
        else
        {

            if (roleController.IsAttacking)
            {
                roleController.BackMoveAnim();
            }

            if (isUseAcceleration)
            {
                currentMoveSpeed = MinMoveSpeed + moveAcceleration;
                moveAcceleration += startAcceleration * Time.deltaTime;
            }
            else
            {
                currentMoveSpeed = MaxMoveSpeed;
            }

            if (currentMoveSpeed > MaxMoveSpeed)
            {
                currentMoveSpeed = MaxMoveSpeed;
            }
            
            roleController.Rigidbody.velocity = dir.normalized * CurrentMoveSpeed;
        }
    }

    protected void ClearMoveSpeed()
    {
        currentMoveSpeed = 0;
        moveAcceleration = 0;
    }

    protected virtual void MoveAnim(Vector3 dirV2)
    {
        if (!roleController.IsCanMove)
        {
            roleController.Animator.SetFloat(MoveSpeed, 0);
            if (roleController.Animator2)
                roleController.Animator2.SetFloat(MoveSpeed, 0);
            return;
        }
        
        if (roleController.IsDie)
        {
            roleController.Animator.SetFloat(MoveSpeed, 0);
            if (roleController.Animator2)
                roleController.Animator2.SetFloat(MoveSpeed, 0);
            return;
        }


        if (CurrentMoveSpeed > 0)
        {
            float value = 0;
            //最大最小速度差大于0.5时候才用这个方式设置移动速度
            if (maxMoveSpeed - minMoveSpeed > 0.5f)
                value = (CurrentMoveSpeed - MinMoveSpeed) / (MaxMoveSpeed - MinMoveSpeed) + 0.01f;
            else
                value = CurrentMoveSpeed / maxMoveSpeed;
            roleController.Animator.SetFloat(MoveSpeed, value);
            if (roleController.Animator2)
                roleController.Animator2.SetFloat(MoveSpeed, value);
        }
        else
        {
            roleController.Animator.SetFloat(MoveSpeed, 0);
            if (roleController.Animator2)
                roleController.Animator2.SetFloat(MoveSpeed, 0);
        }
        if (roleController.Rigidbody.velocity != Vector3.zero)
        {
            if (dirV2 != Vector3.zero && roleController.IsCanMove)
            {
                roleController.Animator.transform.forward = dirV2;
            }
        }
        
    }

}
