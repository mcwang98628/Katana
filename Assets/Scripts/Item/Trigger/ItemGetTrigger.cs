
public class ItemGetTrigger : ItemEffectTrigger
{
    public ItemGetTrigger(TriggerType type) : base(type)
    {
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        
        Root.itemEffect.TriggerEffect(null);
        
    }
}
public class ReviveTrigger : ItemEffectTrigger
{
    public ReviveTrigger(TriggerType type) : base(type)
    {
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.PlayerRevive,OnPlayerRevive);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveEvent(EventName.PlayerRevive,OnPlayerRevive);
    }

    private void OnPlayerRevive(string arg1, object arg2)
    {
        Root.itemEffect.TriggerEffect(null);
    }
}

