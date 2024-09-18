

using Sirenix.OdinInspector;
using UnityEngine;
    
    
    
public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.RoleShadowEffect)]
    [LabelText("暗影材质")]
    public Material ShadowMaterial;
}
public class RoleShadowEffect:ItemEffect
{
    private Material _material;
    public RoleShadowEffect(Material material)
    {
        _material = material;
    }

    private Animator _animator;
    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        _animator = GameObject.Instantiate(roleController.Animator,roleController.Animator.transform);
        _animator.transform.localPosition = new Vector3(0, 0, -0.5f);
        _animator.transform.forward = roleController.Animator.transform.forward;
        _animator.transform.localScale = Vector3.one;
        var meshList = _animator.GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < meshList.Length; i++)
        {
            meshList[i].material = _material;
        }
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        GameObject.Destroy(_animator.gameObject);
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        roleController.Animator2 = _animator;
    }
}