using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeTrigger : ItemEffectTrigger
{
    public DodgeTrigger(TriggerType type) : base(type) {}

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.OnRoleDodgeInjured,OnDodge);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveEvent(EventName.OnRoleDodgeInjured,OnDodge);
    }

    private void OnDodge(string arg1, object arg2)
    {
        RoleInjuredInfo rii = (RoleInjuredInfo) arg2;
        if (rii.RoleId == roleItemController.roleController.TemporaryId)
        {
            roleItemController.AddCurrentFrameTrigger(()=>{Root.itemEffect.TriggerEffect(null);});
        }
    }
}
