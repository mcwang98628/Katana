using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Hp_Effect : BuffEffect
{
    private float hpValue;

    public Buff_Hp_Effect(float value)
    {
        hpValue = value;
    }
    public override void TriggerEffect()
    {
        base.TriggerEffect();
        if (hpValue > 0)
        {
            TreatmentData td = new TreatmentData(hpValue,roleBuff.roleController.TemporaryId);
            roleBuff.roleController.HpTreatment(td);
        }
        else
        {
            if (roleBuff.Adder == null)
            {
                return;
            }
            
            DamageInfo dmg = new DamageInfo(roleBuff.roleController.TemporaryId,Mathf.Abs(hpValue), roleBuff.Adder, roleBuff.Adder.transform.position);

            roleBuff.roleController.HpInjured(dmg);
        }
    }
}
public class Buff_MaxHp_Effect : BuffEffect
{
    private float hpValue;
    AttributeBonus attributeBonus;
    public Buff_MaxHp_Effect(float value)
    {
        hpValue = value;
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = AttributeType.MaxHp;
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
        attributeBonus.Value += hpValue;
    }
    public override void Destroy()
    {
        base.Destroy();
        roleBuff.roleController.RemoveAttributeBonus(attributeBonus);
    }
}