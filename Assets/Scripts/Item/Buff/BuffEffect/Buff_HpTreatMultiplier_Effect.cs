using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_HpTreatMultiplier_Effect : BuffEffect
{
    private float HpTreatMultiplier;


    AttributeBonus attributeBonus;
    public Buff_HpTreatMultiplier_Effect(float value)
    {
        HpTreatMultiplier = value;
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = AttributeType.HpTreatMultiplier;
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
        attributeBonus.Value += HpTreatMultiplier;
    }

    public override void Destroy()
    {
        base.Destroy();
        roleBuff.roleController.RemoveAttributeBonus(attributeBonus);
    }
    
}
