
using Sirenix.OdinInspector;

public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.MagicPower)]
    [LabelText("法强")]
    public float MagicPower;
}
public class MagicPowerEffect:ItemEffect
{
    
    private float Value;
    AttributeBonus attributeBonus;
    public MagicPowerEffect(float value)
    {
        Value = value;
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = AttributeType.MagicPower;
        attributeBonus.Value = 0;
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        roleController.AddAttributeBonus(attributeBonus);
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        attributeBonus.Value += Value;
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        roleController.AddAttributeBonus(attributeBonus);
    }

}