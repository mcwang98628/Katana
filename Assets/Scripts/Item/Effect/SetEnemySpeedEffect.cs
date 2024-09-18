public class SetEnemySpeedEffect:ItemEffect
{
    private float Value;

    public SetEnemySpeedEffect(float value)
    {
        Value = value;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        if (value != null)
        {
            if (!string.IsNullOrEmpty(value.Value.TargetId) && BattleManager.Inst.EnemyTeam.ContainsKey(value.Value.TargetId))
            {
                RoleController target = BattleManager.Inst.EnemyTeam[value.Value.TargetId];
                target.SetMultiplyMoveSpeed(Value);
                // target.Animator.speed = this.Value;
            }
        }
    }
    
}