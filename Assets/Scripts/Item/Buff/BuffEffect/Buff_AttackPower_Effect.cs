
public class Buff_AttackPower_Effect : BuffEffect
{
    private int attackValue;


    AttributeBonus attributeBonus;
    public Buff_AttackPower_Effect(int value)
    {
        attackValue = value;
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = AttributeType.AttackPower;
        attributeBonus.Value = 0;
    }
    public override void Awake()
    {
        base.Awake();
        roleBuff.roleController.AddAttributeBonus(attributeBonus);
    }
    public override void TriggerEffect()
    {
        base.TriggerEffect();
        attributeBonus.Value += attackValue;
    }

    public override void Destroy()
    {
        base.Destroy();
        roleBuff.roleController.RemoveAttributeBonus(attributeBonus);
    }
}

public class Buff_AttackSpeed_Effect : BuffEffect
{
    private float attackSpeed;

    AttributeBonus attributeBonus;
    public Buff_AttackSpeed_Effect(float speed)
    {
        attackSpeed = speed;
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = AttributeType.AttackSpeed;
        attributeBonus.Value = 0;
    }
    public override void Awake()
    {
        base.Awake();
        roleBuff.roleController.AddAttributeBonus(attributeBonus);
    }
    public override void TriggerEffect()
    {
        base.TriggerEffect();
        attributeBonus.Value += attackSpeed;
    }

    public override void Destroy()
    {
        base.Destroy();
        roleBuff.roleController.RemoveAttributeBonus(attributeBonus);
    }
}