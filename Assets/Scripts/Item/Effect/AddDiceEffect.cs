

using Sirenix.OdinInspector;

public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.AddDice)]
    [LabelText("骰子数量")]
    public int DiceCount;
}
public class AddDiceEffect:ItemEffect
{
    private int _diceValue;
    public AddDiceEffect(int value)
    {
        _diceValue = value;
    }
    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        BattleManager.Inst.RuntimeData.AddDice(_diceValue);
    }
}