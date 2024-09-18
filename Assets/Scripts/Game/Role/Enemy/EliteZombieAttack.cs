using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EliteZombieAttack : EnemyAttack
{
    bool AttackRotating = false;

    public float AttackRotSpeed;
    public float NearDistance;
    public float FarDistance;
    public GameObject NearAttackParticles;
    public GameObject FarAttackParticles;
    public GameObject SelectedAttackParticles;

    public override void AttackFunc()
    {
        if (! AttackRotating )
        {
            //Debug.Log("TryAttack");
             float Dis = Vector3.Distance(BattleManager.Inst.CurrentPlayer.transform.position, transform.position);
            //if (Dis < NearDistance)
            //{
            //    roleController.Animator.SetBool("NearAttack", true);
            //    roleController.Animator.SetBool("FarAttack", false);
            //    //roleController.Animator.SetFloat("AttackSelector", 0.7f);
            //    SelectedAttackParticles = NearAttackParticles;
            //    Debug.Log("SetNearAttack");
            //}
            //else if (Dis < FarDistance)
            //{
            //    roleController.Animator.SetBool("NearAttack",false);
            //    roleController.Animator.SetBool("FarAttack",true);
            //    //roleController.Animator.SetFloat("AttackSelector", 0.2f);
            //    SelectedAttackParticles = FarAttackParticles;
            //    Debug.Log("SetFarAttack");
            //}
                //改为只能近战挠人
            roleController.Animator.SetBool("NearAttack", true);
            roleController.Animator.SetBool("FarAttack", false);
            SelectedAttackParticles = NearAttackParticles;
        }
        AttackRotating = true;
        roleController.InputMove(Vector2.zero);
        roleController.SetIsAttacking(true);
        
        roleController.Animator.SetBool("Attack",true);
        
    }
    protected override void Update()
    {
        base.Update();
        if (AttackRotating)
        {
            roleController.Animator.transform.forward = Vector3.Lerp(roleController.Animator.transform.forward, (UpdateTarget().position - transform.position).normalized, AttackRotSpeed * Time.deltaTime).normalized;
        }
    }
    protected override void AnimEvent(GameObject go, string eventName)
    {
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
        base.AnimEvent(go, eventName);
        if(eventName.Contains(AnimatorEventName.StartAttack_))
        {
            //float Dis = Vector3.Distance(BattleManager.Inst.CurrentPlayer.transform.position, transform.position);
            //if (Dis < NearDistance)
            //{
            //    if (NearAttackParticles != null)
            //        SelectedAttackParticles = NearAttackParticles;
            //}
            //else if (Dis < FarDistance)
            //{
            //    if (FarAttackParticles != null)
            //        SelectedAttackParticles = FarAttackParticles;
            //}
        }
         if (eventName.Contains(AnimatorEventName.DmgEnd_))
        {
            AttackRotating = false;
            roleController.Animator.SetBool("NearAttack", false);
            roleController.Animator.SetBool("FarAttack", false);
        }
         if(eventName.Contains(AnimatorEventName.DmgStart_))
        {
            if (SelectedAttackParticles!=null)
            Instantiate(SelectedAttackParticles, transform.position+new Vector3(0,0.2f,0),roleController.Animator.transform.rotation);
        }

    }
}
