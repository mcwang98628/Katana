using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_EnterRoom_Trigger : BuffTrigger
{
    public Buff_EnterRoom_Trigger()
    {
    }

    public override void Awake()
    {
        base.Awake();
        EventManager.Inst.AddEvent(EventName.EnterNextRoom,OnEnterRoom);
    }

    public override void Destroy()
    {
        base.Destroy();
        EventManager.Inst.RemoveEvent(EventName.EnterNextRoom, OnEnterRoom);
    }

    private void OnEnterRoom(string arg1, object value)
    {
        roleBuff.TriggerEffect();
    }
}

public class Buff_EnterFightRoom_Trigger : BuffTrigger
{
    public Buff_EnterFightRoom_Trigger()
    {
    }

    public override void Awake()
    {
        base.Awake();
        EventManager.Inst.AddEvent(EventName.EnterFightRoom, OnEnterRoom);
    }

    public override void Destroy()
    {
        base.Destroy();
        EventManager.Inst.RemoveEvent(EventName.EnterFightRoom, OnEnterRoom);
    }

    private void OnEnterRoom(string arg1, object value)
    {
        roleBuff.TriggerEffect();
    }
}