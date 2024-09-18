using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMoveSpeedEffect : ItemEffect
{
    private float Value;

    private bool isUseAccel;
    private float startAccel;
    private float stopAccel;
    private bool setAcceleratopn;

    AttributeBonus attributeBonus;

    public SetMoveSpeedEffect(float value)
    {
        Value = value;

        setAcceleratopn = false;
    }
    public SetMoveSpeedEffect(float _moveSpeed, bool _isUseAccel, float _startAccel, float _stopAccel)
    {
        Value = _moveSpeed;
        isUseAccel = _isUseAccel;
        stopAccel = _startAccel;
        stopAccel = _stopAccel;
        setAcceleratopn = true;
    }
    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = AttributeType.MoveSpeed;
        attributeBonus.Value = 0;
        roleController.AddAttributeBonus(attributeBonus);
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        if (setAcceleratopn)
        {
            roleController.SetAcceleration(isUseAccel, startAccel, stopAccel);
        }

        attributeBonus.Value += Value;
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        roleController.RemoveAttributeBonus(attributeBonus);

        //todo 删除时候要还原加速度
    }
 
}