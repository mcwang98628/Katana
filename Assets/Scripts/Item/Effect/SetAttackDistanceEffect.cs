using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAttackDistanceEffect : ItemEffect
{
    private float Value;
    AttributeBonus attributeBonus;
    public SetAttackDistanceEffect(float value)
    {
        Value = value;
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = AttributeType.AttackDistance;
        attributeBonus.Value = 0;
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        roleController.AddAttributeBonus(attributeBonus);
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        attributeBonus.Value += Value;
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        roleController.AddAttributeBonus(attributeBonus);
    }
 
}