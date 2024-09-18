


//已经废弃
public class PlayerShieldHoldingTrigger : ItemEffectTrigger
{
    private bool isHolding;//举盾触发 还是 放下盾触发
    private bool currentIsHolding;
    public PlayerShieldHoldingTrigger(TriggerType type,bool isHolding) : base(type)
    {
        this.isHolding = isHolding;
        currentIsHolding = !isHolding;
    }
     
    private void OnShieldHolding(string arg1, object arg2)
    {
        bool value = (bool) arg2;
        if (currentIsHolding == value)
        {
            return;
        }

        currentIsHolding = value;
        if (isHolding == value)
        {
            Root.itemEffect.TriggerEffect(null);
        }
    }
    
}
