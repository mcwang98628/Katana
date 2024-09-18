using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackTypeEffect : ItemEffect
{
    private int _attackType;
    public PlayerAttackTypeEffect(int attackType)
    {
        _attackType = attackType;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        // ((PlayerAttack)roleController.roleAttack).SetAttackType(_attackType);
    }
}
public class PlayerAccumulateAttackTypeEffect : ItemEffect
{
    private int _attackType;
    public PlayerAccumulateAttackTypeEffect(int attackType)
    {
        _attackType = attackType;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        // ((PlayerAttack)roleController.roleAttack).SetAccumulateAttackType(_attackType);
    }
}
public class PlayerRollTypeEffect : ItemEffect
{
    private int _rollType;
    public PlayerRollTypeEffect(int rollType)
    {
        _rollType = rollType;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        ((PlayerRoll)roleController.roleRoll).SetRollType(_rollType);
    }
}
