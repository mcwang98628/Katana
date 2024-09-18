using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_MoveSpeed_Effect : BuffEffect
{
    private float moveSpeed;

    AttributeBonus attributeBonus;
    public Buff_MoveSpeed_Effect(float value)
    {
        moveSpeed = value;
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = AttributeType.MoveSpeed;
        attributeBonus.Value = 0;
    }
    public override void Awake()
    {
        base.Awake();
        roleBuff.roleController.AddAttributeBonus(attributeBonus);
    }
    public override void TriggerEffect()
    {
        base.TriggerEffect();

        attributeBonus.Value += moveSpeed;
    }

    public override void Destroy()
    {
        base.Destroy();
        roleBuff.roleController.RemoveAttributeBonus(attributeBonus);
    }
}
