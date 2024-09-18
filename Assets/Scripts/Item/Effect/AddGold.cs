using UnityEngine;

public class AddGold:ItemEffect
{
    private float goldRatio;

    public AddGold(float ratio)
    {
        goldRatio = ratio;
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        var gold = BattleManager.Inst.RuntimeData.CurrentGold * goldRatio;
        BattleManager.Inst.RuntimeData.AddGold(Mathf.FloorToInt(gold));
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
    }
}