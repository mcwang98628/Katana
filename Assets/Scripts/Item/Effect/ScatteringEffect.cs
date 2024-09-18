using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.Scattering)]
    [LabelText("散射物体")]
    public Bullet ScatteringObject;
    [ShowIf("EffectType", EffectType.Scattering)]
    [LabelText("数量")]
    public int ScatteringObjectCount;
    [ShowIf("EffectType", EffectType.Scattering)]
    [LabelText("Offset")]
    public Vector3 ScatteringOffset;
}
//散射
public class ScatteringEffect:ItemEffect
{
    private Bullet _prefab;
    private int _count;
    private Vector3 _offset;
    public ScatteringEffect(Bullet prefab,int count,Vector3 offset)
    {
        _prefab = prefab;
        _count = count;
        _offset = offset;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        RoleController targetRole = null;
        if (value != null)
        {
            targetRole = value.Value.TargetRole;
        }
        if (targetRole == null)
            targetRole = roleController;
        int itemCount = roleController.roleItemController.GetItemCount(Root.item.ID);
        int triggerTimes = roleController.roleItemController.GetCurrentFrameItemTriggerTimes(Root.item.ID)-1;
        float itemAng = 360f / _count;//道具内部角度
        float itemCountAng = itemAng / itemCount;//多道具旋转角度
        float triggerTimesAng = triggerTimes * itemCountAng;

        var forwardDir = targetRole.Animator.transform.forward;

        for (int i = 0; i < _count; i++)
        {
            var ang = Rotate(forwardDir, Vector3.up, itemAng*i + triggerTimesAng);
            var prefab = GameObject.Instantiate(_prefab);
            prefab.transform.position = targetRole.transform.position + _offset;
            prefab.transform.forward = ang;
            prefab.transform.localScale = Vector3.one;
            prefab.Init(roleController,prefab.transform.forward);
        }

    }
    
    Vector3 Rotate(Vector3 source, Vector3 axis, float angle)
    {
        Quaternion q = Quaternion.AngleAxis(angle, axis);// 旋转系数
        return q * source;// 返回目标点
    }
    
}