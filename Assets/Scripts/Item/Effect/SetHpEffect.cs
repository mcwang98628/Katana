using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHpEffect : ItemEffect
{
    public float Value;
    private float CurrentHpPercentage;
    private ParticleSystem Particle;
    private ParticleSystem ParticleGo;
    public SetHpEffect(float value,float currentHpPercentage, ParticleSystem particleSystem)
    {
        Value = value;
        CurrentHpPercentage = currentHpPercentage;
        Particle = particleSystem;
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        if (Particle != null)
        {
            ParticleGo = GameObject.Instantiate(Particle);
            ParticleGo.Stop();
        }

        Value += CurrentHpPercentage * roleController.OriginalMaxHp;
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        if (ParticleGo != null)
        {
            GameObject.Destroy(ParticleGo);
        }
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        if (Value > 0)
        {
            TreatmentData td = new TreatmentData(Value, roleController.TemporaryId);
            roleController.HpTreatment(td);
        }
        else
        { 
            DamageInfo dmg = new DamageInfo(roleController.TemporaryId,Mathf.Abs(Value), roleController, roleController.transform.position, DmgType.Unavoidable,
                false, false, 0, 0);
            roleController.HpInjured(dmg);
        }

        if (ParticleGo != null)
        {
            ParticleGo.transform.position = roleController.Animator.transform.position;
            ParticleGo.Play();
        }
    }

    public override void Update(RoleItemController rpe)
    {
        base.Update(rpe);

        if (ParticleGo != null)
        {
            ParticleGo.transform.position = roleController.transform.position + new Vector3(0, 0.5f, 0);
        }
    }
 
}


public class SetMaxHpEffect : ItemEffect
{
    public float Value;
    private float HpPercentage;
    AttributeBonus attributeBonus;
    private float currentHpPercent;
    public SetMaxHpEffect(float value,float Percentage)
    {
        Value = value;
        HpPercentage = Percentage;
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = AttributeType.MaxHp;
        attributeBonus.Value = 0;
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        roleController.AddAttributeBonus(attributeBonus);
        Value += roleController.OriginalMaxHp * HpPercentage;
    } 

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        currentHpPercent = roleController.CurrentHp / roleController.MaxHp;
        attributeBonus.Value += Value;
        roleController.HpTreatment(new TreatmentData(roleController.MaxHp*currentHpPercent - roleController.CurrentHp, roleController.TemporaryId));
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        roleController.RemoveAttributeBonus(attributeBonus);
    }
 
}
public class SetTreatParaEffect : ItemEffect
{
    public float Value;
    AttributeBonus attributeBonus;
    public SetTreatParaEffect(float value)
    {
        Value = value;
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = AttributeType.HpTreatMultiplier;
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



public class SetMaxPowerEffect : ItemEffect
{
    public float Value;
    AttributeBonus attributeBonus;
    public SetMaxPowerEffect(float value)
    {
        Value = value;
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = AttributeType.MaxPower;
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

public class SetAttackPowerRecoveryEffect : ItemEffect
{
    public float Value;
    AttributeBonus attributeBonus;
    public SetAttackPowerRecoveryEffect(float value)
    {
        Value = value;
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = AttributeType.AttackPowerRecovery;
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

public class SetPowerRecoveryEffect : ItemEffect
{
    public float Value;
    AttributeBonus attributeBonus;
    public SetPowerRecoveryEffect(float value)
    {
        Value = value;
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = AttributeType.PowerRecovery;
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