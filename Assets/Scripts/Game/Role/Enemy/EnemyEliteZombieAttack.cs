using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class EnemyEliteZombieAttack : EnemyAttack
{

    protected override void AnimEvent(GameObject go, string eventName)
    {
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
        base.AnimEvent(go, eventName);
        if (eventName.Contains("LookRotate"))
        {
            LookRotate();
        }
        
    }
    void LookRotate()
    {
            roleController.Animator.transform.forward = UpdateTarget().transform.position - transform.position;
            
    }


}
