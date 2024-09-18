using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldEffect : ItemEffect
{
    private int goldValue;

    public GoldEffect(int value)
    {
        goldValue = value;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        for (int i = 0; i < goldValue; i++)
        {
            if (value.HasValue && value.Value.TargetPosition!=Vector3.zero)
            {
                DropManager.Inst.SpwanCoinAt(value.Value.TargetPosition);
            }
            else
            {
                DropManager.Inst.SpwanCoinAt(roleController.transform.position);
            }
        }
    }
}
