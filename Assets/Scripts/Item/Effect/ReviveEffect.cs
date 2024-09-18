using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveEffect : ItemEffect
{
    private float Value;
    public ReviveEffect(float value)
    {
        Value = value;
    }


    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        roleController.StartCoroutine(waitResurrection());
    }

    IEnumerator waitResurrection()
    {
        yield return new WaitForSecondsRealtime(1f);
        if (BattleManager.Inst.Resurrection(Value))
        {
            roleController.roleItemController.ReMoveItem(Root.item);
        }
    }
 
}
