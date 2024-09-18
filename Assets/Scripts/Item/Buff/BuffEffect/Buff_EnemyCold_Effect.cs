using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_EnemyCold_Effect : BuffEffect
{
    private float AnimSpeed;
    private float MoveMagnification;

    private float speedValue;
    public Buff_EnemyCold_Effect(float animSpeed,float moveMagnification)
    {
        AnimSpeed = animSpeed;
        MoveMagnification = moveMagnification;
    }

    public override void Awake()
    {
        base.Awake();
        speedValue = 0;
    }
    public override void TriggerEffect()
    {
        base.TriggerEffect();
        roleBuff.roleController.Animator.speed = AnimSpeed;
        
        roleBuff.roleController.SetMultiplyMoveSpeed(MoveMagnification);
        speedValue += MoveMagnification;

    }

    public override void Update()
    {
        base.Update();
        if (!roleBuff.roleController.IsFreeze)
        {
            roleBuff.roleController.Animator.speed = AnimSpeed;
        }
    }

    public override void Destroy()
    {
        base.Destroy();
        roleBuff.roleController.Animator.speed = 1;
        if (speedValue!=0)
        {
            roleBuff.roleController.SetExceptMoveSpeed(speedValue);
        }

    }
}
