using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRoomTrigger : ItemEffectTrigger
{
    public NewRoomTrigger(TriggerType type) : base(type){}

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.EnterNextRoom,trigger);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveEvent(EventName.EnterNextRoom,trigger);
    }

    private void trigger(string arg1, object arg2)
    {
        roleItemController.StartCoroutine(waitTrigger());
    }
    IEnumerator waitTrigger()
    {
        yield return new WaitForSeconds(0.5f);
        roleItemController.AddCurrentFrameTrigger(()=>{Root.itemEffect.TriggerEffect(null);});
    }
}


public class EnterFightRoomTrigger : ItemEffectTrigger
{
    public EnterFightRoomTrigger(TriggerType type) : base(type) { }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.EnterFightRoom, trigger);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveEvent(EventName.EnterFightRoom, trigger);
    }

    private void trigger(string arg1, object arg2)
    {
        roleItemController.StartCoroutine(waitTrigger());
    }
    IEnumerator waitTrigger()
    {
        yield return new WaitForSeconds(0.5f);
        roleItemController.AddCurrentFrameTrigger(()=>{Root.itemEffect.TriggerEffect(null);});
    }
}