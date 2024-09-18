

public class ShowEnemyHpTextEffect:ItemEffect
{
    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        BattleManager.Inst.RuntimeData.ShowEnemyHpText = true;
    }
}


