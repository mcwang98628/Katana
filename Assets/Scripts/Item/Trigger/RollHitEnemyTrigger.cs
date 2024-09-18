
public class RollHitEnemyTrigger:ItemEffectTrigger
{
    public RollHitEnemyTrigger(TriggerType type) : base(type)
    {
    }
    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.OnPlayerAttackHitEnemy,OnPlayerAttack);
    }

    private void OnPlayerAttack(string arg1, object arg2)
    {
        DamageInfo dmgInfo = (DamageInfo) arg2;
        if (dmgInfo.AttackType == DamageAttackType.RollAttack)
        {
            string enemyId = dmgInfo.HitRoleId;
            if (BattleManager.Inst.CurrentPlayer.TemporaryId == enemyId)
            {
                return;
            }
            roleItemController.AddCurrentFrameTargetIdTrigger(enemyId,() =>
            {
                Root.itemEffect.TriggerEffect(new ItemEffectTriggerValue()
                    {
                        TargetId = enemyId,
                        DamageInfo = dmgInfo
                    }
                ); 
            });
        }
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveEvent(EventName.OnPlayerAttackHitEnemy,OnPlayerAttack);
    }
}
