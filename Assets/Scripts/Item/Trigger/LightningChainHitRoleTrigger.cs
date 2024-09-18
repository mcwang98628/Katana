using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningChainHitRoleTrigger : ItemEffectTrigger
{
    public LightningChainHitRoleTrigger(TriggerType type) : base(type)
    {
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.OnLightningChainHitRole,OnHitRole);
    }

    private void OnHitRole(string arg1, object arg2)
    {
        string id = (string) arg2;
        roleItemController.AddCurrentFrameTargetIdTrigger(id,()=>{Root.itemEffect.TriggerEffect(new ItemEffectTriggerValue(){TargetId = id});});
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.AddEvent(EventName.OnLightningChainHitRole,OnHitRole);
    }
}
