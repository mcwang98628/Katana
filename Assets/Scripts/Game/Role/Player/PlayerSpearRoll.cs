using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpearRoll : PlayerRoll
{
    // [SerializeField]
    // private int attackRollNeedPower = 10;
    public override void InputRoll(Vector2 v2)
    {
        
        if (roleController.CurrentSkillPower/(float)roleController.MaxSkillPower >0.8f)
        {
            roleController.SetIsAttacking(false);
            roleController.SetIsRoll(true);
            roleController.gameObject.layer = LayerMask.NameToLayer("PlayerRoll");
            
            roleController.Animator.transform.forward = new Vector3(v2.x,0,v2.y);
            // roleController.AddPower(-attackRollNeedPower);
            roleController.Animator.SetTrigger("Step");
            // roleController.FastMove(RollTime,RollSpeed,new Vector3(v2.x,0,v2.y), RollBack,rollCurve);
        }
        else
        {
            base.InputRoll(v2);
        }
    }
}
