using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPathEffect : ItemEffect
{
    public DmgBuffOnTouch PathPointObj;
    private int AttackPower;
    private float attackpowerpercentage;
    public FootPathEffect(DmgBuffOnTouch buffOnTouch,int attackpower,float attackpowerpercentage)
    {
        PathPointObj = buffOnTouch;
        AttackPower = attackpower;
        this.attackpowerpercentage = attackpowerpercentage;
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        AttackPower += (int) (roleController.OriginalAttackPower * attackpowerpercentage);
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        var pathPoint = GameObject.Instantiate(PathPointObj);
        pathPoint.transform.position = roleController.transform.position;
        pathPoint.Init(roleController,AttackPower);
    }
 
}
