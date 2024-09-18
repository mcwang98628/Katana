


using Sirenix.OdinInspector;

public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.TagEffect)]
    [LabelText("Tag")]
    public string TagStr;
}
public class TagEffect:ItemEffect
{
    private string tag;
    public TagEffect(string tagStr)
    {
        tag = tagStr;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        roleController.AddTag(tag);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        roleController.RemoveTag(tag);
    }
}