using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelfThrowBomb : EnemyAttack
{
    public float DashSpeed = 15;
    public float DashTime = 0.5f;
    public AnimationCurve DashCurve;
    public GameObject SpwanOnStart;
    public DmgBuffOnTouch damageOnTouch;
    public Vector2 RandomOffset;
    bool startAttack = false;

    public override void AttackFunc()
    {
        if (!IsAcceptInput)
        {
            return;
        }
        if (startAttack)
        {
            return;
        }
        
        roleController.InputMove(Vector2.zero);
        roleController.SetIsAttacking(true);

        
        roleController.Animator.SetInteger(AttackStatus, currentAttackStatus);
        roleController.Animator.SetTrigger(Attack);
        
       
        startAttack = true;
    }

    void StartDash()
    {
        if (SpwanOnStart)
        {
            Instantiate(SpwanOnStart, roleController.Animator.transform.position, roleController.Animator.transform.rotation).transform.SetParent(roleController.Animator.transform);
        }
        //roleController.gameObject.layer = LayerMask.NameToLayer("PlayerRoll");
        GetComponent<Collider>().isTrigger = true;

        Vector3 des = Vector3.zero;
        Transform player = BattleManager.Inst.CurrentPlayer.transform;
        if (player != null && Vector3.Angle(player.position-transform.position,transform.forward)<60)
        {
            des = player.transform.position;
        }
        else
        {
            des = transform.position + roleController.Animator.transform.forward * DashSpeed * DashTime;
        }
        des += new Vector3(RandomOffset.x*(Random.value-.5f), 0, RandomOffset.y*(Random.value-.5f));
        float dashSpeed = Vector3.Distance(transform.position, des)/DashTime;
        Vector3 dashDir = (des - transform.position).normalized;

        roleController.FastMove(DashTime, dashSpeed, dashDir, EndDash, DashCurve);
        IndicatorManager.Inst.ShowAttackIndicator().Show(roleController,des,360,1.6f,1.6f, Color.red);
        // IndicatorManager.Inst.ShowAttackIndicator().SetEnable(true).SetAngle(360).SetSize(1.6f).SetTime(1.6f).SetPosition(des);
    }

    void EndDash()
    {
        if (damageOnTouch)
        {
            //            Instantiate(SpwanOnEnd, roleController.Animator.transform.position + Vector3.up*0.5f, roleController.Animator.transform.rotation);
            var dmg = Instantiate(damageOnTouch);
            dmg.gameObject.transform.position = transform.position;
            dmg.gameObject.SetActive(true);
            dmg.Init(roleController,AttackPower);
        }
        //之后改成造成伤害
    }


    protected override void DamageCalculation(AttackInfo info)
    {
    }



    protected override void AnimEvent(GameObject go, string eventName)
    {
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
        base.AnimEvent(go, eventName);
        if (eventName.Contains("StartJump"))
        {
            StartDash();
        }
    }

    void ShowWarnPoint()
    {
    }
}
