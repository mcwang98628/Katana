using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoll : RoleRoll
{
    int layerTemp;
    public override void InputRoll(Vector2 v2)
    {
        if (!roleController.IsCanRoll)
        {
            return;
        }
        if (roleController.IsAttacking)
        {
            return;
        }

        roleController.StopAttack();
        PlayEffect(new Vector3(v2.x, 0, v2.y));
        // roleController.FastMove(RollTime, RollSpeed, new Vector3(v2.x, 0, v2.y), RollBack, rollCurve);
        roleController.Animator.SetTrigger("Roll");

        roleController.SetIsRoll(true);
        roleController.SetIsAttacking(false);
        layerTemp = roleController.gameObject.layer;
        roleController.gameObject.layer = LayerMask.NameToLayer("PlayerRoll");
    }
    protected override void RollBack()
    {
        OnEndRoll();
        base.RollBack();
        roleController.SetIsRoll(false);
        roleController.gameObject.layer = layerTemp;
    }
}
