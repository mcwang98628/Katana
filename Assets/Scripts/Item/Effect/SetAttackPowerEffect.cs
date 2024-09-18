using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//攻击力
public class SetAttackPowerEffect : ItemEffect
{
    private int Value;
    private float percentage;
    AttributeBonus attributeBonus;
    public SetAttackPowerEffect(int value,float percentage)
    {
        Value = value;
        this.percentage = percentage;
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = AttributeType.AttackPower;
        attributeBonus.Value = 0;
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        Value += (int)(roleController.OriginalAttackPower*percentage);
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
        roleController.RemoveAttributeBonus(attributeBonus);
    } 
}
//暴击概率
public class SetCriticalProbabilityEffect : ItemEffect
{
    private float Value;
    AttributeBonus attributeBonus;
    public SetCriticalProbabilityEffect(float value)
    {
        Value = value;
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = AttributeType.CriticalProbability;
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
        roleController.RemoveAttributeBonus(attributeBonus);
    }
 
}
//暴击倍率
public class SetCriticalMultiplierEffect : ItemEffect
{    
    private float Value;
    AttributeBonus attributeBonus;
    public SetCriticalMultiplierEffect(float value)
    {
        Value = value;
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = AttributeType.CriticalMultiplier;
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
        roleController.RemoveAttributeBonus(attributeBonus);
    } 
}
//扩散效果
public class SetSplashDamageEffect : ItemEffect
{    
    private float Value;
    public SetSplashDamageEffect(float value)
    {
        Value = value;
    }
    private float effectValue;
    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        roleController.SetSplashDamage(Value);
        effectValue += Value;
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        roleController.SetSplashDamage(-effectValue);
    }
 
}