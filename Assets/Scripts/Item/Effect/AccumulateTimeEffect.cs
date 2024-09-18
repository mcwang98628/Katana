

public class AccumulateTimeEffect : ItemEffect
{
    
    private float Value;
    AttributeBonus attributeBonus;
    public AccumulateTimeEffect(float value)
    {
        Value = value;
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = AttributeType.AccumulateTime;
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
        roleController.RemoveAttributeBonus(attributeBonus);
    }

}
