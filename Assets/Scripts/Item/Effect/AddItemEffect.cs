



using Sirenix.OdinInspector;

public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.AddItem)]
    [LabelText("添加道具")]
    public ItemScriptableObject ItemObj;
}

public class AddItemEffect:ItemEffect
{
    private ItemScriptableObject _itemObj;
    public AddItemEffect(ItemScriptableObject itemObj)
    {
        _itemObj = itemObj;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        roleController.roleItemController.AddItem(DataManager.Inst.ParsingItemObj(_itemObj),null);
    }
}