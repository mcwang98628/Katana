//被悬赏的敌人死亡时
public class KillRewardEnemyDeadTrigger:ItemEffectTrigger
{
    public KillRewardEnemyDeadTrigger(TriggerType type) : base(type)
    {
        
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.OnRoleDead,OnEnemyDead);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveEvent(EventName.OnRoleDead,OnEnemyDead);
    }

    private void OnEnemyDead(string arg1, object arg2)
    {
        RoleDeadEventData deadEventData = (RoleDeadEventData) arg2;
        if (deadEventData.DeadRole.roleTeamType == RoleTeamType.Player)
            return;
        int tagCount = deadEventData.DeadRole.GetTagCount(RoleTagName.KillReward);
        if (tagCount <= 0)
            return;
        deadEventData.DeadRole.RemoveTag(RoleTagName.KillReward);
        Root.itemEffect.TriggerEffect(new ItemEffectTriggerValue()
        {
            TargetId = deadEventData.DeadRole.TemporaryId,
            TargetPosition = deadEventData.DeadRole.transform.position,
            TargetRole = deadEventData.DeadRole
        });
        
    }
    
    
}