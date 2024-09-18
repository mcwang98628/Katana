using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


// [CreateAssetMenu(menuName = "GameItem/AttackTrigger")]
public class AttackTrigger : ItemEffectTrigger
{
    private int times;//计数
    private int Value;

    private int MinOffset;
    private int MaxOffset;
    public AttackTrigger(TriggerType type, int value,int minOffset=0,int maxOffset=0) : base(type)
    {
        Value = value;
        MinOffset = minOffset;
        MaxOffset = maxOffset;
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.OnRoleAttack,OnPlayerAttack);
    }
    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveEvent(EventName.OnRoleAttack,OnPlayerAttack);
    }
    private void OnPlayerAttack(string arg1, object id)
    {
        if (BattleManager.Inst.CurrentPlayer.TemporaryId != (string)id)
        {
            return;
        }

        switch (triggerType)
        {
            case TriggerType.Probability:
                int value = Random.Range(0, 100);
                if (value <= Value)
                {
                    roleItemController.AddCurrentFrameTrigger(()=>{Root.itemEffect.TriggerEffect(null);});
                }
                break;
            case TriggerType.Times:
                times++;
                if (times>=Value+Random.Range(MinOffset,MaxOffset))
                {
                    times -= Value;
                    roleItemController.AddCurrentFrameTrigger(()=>{Root.itemEffect.TriggerEffect(null);});
                }
                break;
        }
    }
}
