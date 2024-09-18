

using UnityEngine;

public class BattleObjectDestroy:ItemEffectTrigger
{
    public BattleObjectDestroy(TriggerType type) : base(type)
    {
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.OnBattleObjectDestroy,OnBattleObjectDestroy);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveEvent(EventName.OnBattleObjectDestroy,OnBattleObjectDestroy);
    }

    private void OnBattleObjectDestroy(string arg1, object arg2)
    {
        Vector3 pos = (Vector3) arg2;
        ItemEffectTriggerValue triggerValue = new ItemEffectTriggerValue(){TargetPosition = pos};
        Root.itemEffect.TriggerEffect(triggerValue);
    }
    
    
    
    
}