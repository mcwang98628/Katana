

public class CreateEnemyTrigger:ItemEffectTrigger
{
    public CreateEnemyTrigger(TriggerType type) : base(type)
    {
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.OnRoleRegistered,OnEnemyRegistered);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.AddEvent(EventName.OnRoleRegistered,OnEnemyRegistered);
    }

    private void OnEnemyRegistered(string arg1, object arg2)
    {
        RoleController targetRole = (RoleController) arg2;
        if (targetRole == null || targetRole.roleTeamType == RoleTeamType.Player)
            return;
        
        
        Root.itemEffect.TriggerEffect(new ItemEffectTriggerValue()
        {
            TargetId = targetRole.TemporaryId,
        });
        
    }
}