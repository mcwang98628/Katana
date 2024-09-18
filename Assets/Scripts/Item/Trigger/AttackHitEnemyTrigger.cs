using Random = UnityEngine.Random;


public class AttackHitEnemyTrigger : ItemEffectTrigger
{
    private int times;//计数
    private int Value;

    public AttackHitEnemyTrigger(TriggerType type, int value) : base(type)
    {
        Value = value;
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.OnPlayerAttackHitEnemy,OnPlayerAttack);
    }
    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveEvent(EventName.OnPlayerAttackHitEnemy,OnPlayerAttack);
    }
    private void OnPlayerAttack(string arg1, object dmgInfo)
    {
        var dmgData = ((DamageInfo) dmgInfo);
        string enemyId = dmgData.HitRoleId;
        if (BattleManager.Inst.CurrentPlayer.TemporaryId == enemyId)
        {
            return;
        }

        switch (triggerType)
        {
            case TriggerType.Probability:
                int value = Random.Range(0, 100);
                if (value <= Value)
                {
                    roleItemController.AddCurrentFrameTargetIdTrigger(enemyId,() =>
                    {
                        Root.itemEffect.TriggerEffect(new ItemEffectTriggerValue()
                            {
                                TargetId = enemyId,
                                DamageInfo = dmgData
                            }
                        ); 
                    });
                }
                break;
            case TriggerType.Times:
                times++;
                if (times>=Value)
                {
                    times -= Value;
                    roleItemController.AddCurrentFrameTargetIdTrigger(enemyId,() =>
                    {
                        Root.itemEffect.TriggerEffect(new ItemEffectTriggerValue()
                            {
                                TargetId = enemyId,
                                DamageInfo = dmgData
                            }
                        ); 
                    });
                }
                break;
        }
    }
}
