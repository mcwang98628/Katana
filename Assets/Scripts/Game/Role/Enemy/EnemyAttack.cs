    using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.Events;


public class EnemyAttack : RoleAttack
{
    private bool isCanAttack => !roleController.IsDizziness && !roleController.IsDie;

    private float testTime;
    bool LookAtTarget = false;

    [Header("Effect")]
    //怪物攻击时会使用的效果
    public ParticleSystem StartAttackParticle;
    public ParticleSystem AttackDamgeParticle;
    public FeedBackObject FeedBack;
    float IniRotSpeed;
    EnemyMove _move;
    
    public AudioClip AttackRoar;


    protected  virtual void Start()
    {
        _move = GetComponent<EnemyMove>();
        if(_move!=null)
        {
            IniRotSpeed = _move.RotateSpeed;
        }
        attackPower = (int)(attackPower);

    }

    public override void AttackFunc()
    {
        if (!IsAcceptInput)
        {
            return;
        }

        if (!isCanAttack)
        {
            return;
        }

        //新加：
        //攻击时自动停下（从行为树里面摘过来的）
        roleController.InputMove(Vector2.zero);
        roleController.SetIsAttacking(true);

        roleController.Animator.SetInteger(AttackStatus,currentAttackStatus);
        roleController.Animator.SetTrigger(Attack);
    }

    protected override void Update()
    {
        base.Update();
        if (LookAtTarget)
        {
            if (BattleManager.Inst.CurrentPlayer == null)
            {
                return;
            }
            Transform target= BattleManager.Inst.CurrentPlayer.transform;
            if (target == null)
            {
                return;
            }
            if (roleController.IsFreeze)
            {
                return;
            }
            float RotSpeed = (roleController as EnemyController).GetRotateSpeed();

            Vector3 dir = target.position - gameObject.transform.position;
            dir.y = 0;
            int rotDir = Vector3.Cross(roleController.Animator.transform.forward, dir).y > 0 ? 1 : -1;
            float angle = Vector3.Angle(dir, roleController.Animator.transform.forward);
            if(angle> RotSpeed*0.5f)
                roleController.Animator.transform.forward = Quaternion.Euler(0, rotDir * RotSpeed, 0) * roleController.Animator.transform.forward;
        }

    }

    // IEnumerator AnimTimeScaling()
    // {
    //     roleController.Animator.speed = 0.05f;
    //     yield return new WaitForSeconds(testTime);
    //     roleController.Animator.speed = 1f;
    // }
    
    protected Transform UpdateTarget()
    {
        if (BattleManager.Inst.CurrentPlayer == null)
        {
            return null;
        }
        Vector3 playerPos = BattleManager.Inst.CurrentPlayer.transform.position;
        Vector3 v3 = playerPos - roleController.transform.position;
        if (v3.magnitude <=999)
        {
            return BattleManager.Inst.CurrentPlayer.transform;
        }
        return null;
    }

    protected override void DamageCalculation(AttackInfo info)
    {
        //Debug.Log("StartCalcu");
        if (roleController == null || BattleManager.Inst.CurrentPlayer == null)
        {
            dmging = false;
            return;
        } 
        Vector3 v3;   
        switch (info.AttackType)
        {
            case AttackType.Round:
                v3 = BattleManager.Inst.CurrentPlayer.transform.position - roleController.transform.position;
                if (v3.magnitude <= info.AttackRadius)
                {
                    TargetDmg(BattleManager.Inst.CurrentPlayer,info);
                }
                break;
            case AttackType.Sector:
                v3 = BattleManager.Inst.CurrentPlayer.transform.position - 
                     roleController.transform.position;
                if (v3.magnitude <= info.AttackRadius)
                {
                    float angle = Vector3.Angle(roleController.Animator.transform.forward, v3);
                    if (angle <= info.AttackAngle*0.5f)
                    {
                        TargetDmg(BattleManager.Inst.CurrentPlayer,info);
                    }
                }
                break;
        }
    }



    protected override void AnimEvent(GameObject go, string eventName)
    {
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
        base.AnimEvent(go,eventName); 
        if (eventName.Contains(AnimatorEventName.ShowFeedBack))
        {
            if (FeedBack)
            {
                FeedbackManager.Inst.UseFeedBack(roleController, FeedBack);
            }
        }
        else if (eventName.Contains(AnimatorEventName.StartAttack_))
        {
            ShowStartAttackParticle();
            ShowStartAttackParticle();
        }
        else if (eventName.Contains(AnimatorEventName.DmgStart_))
        {
            ShowAttackDamageParticle(); 
        }
        else if (eventName.Contains(AnimatorEventName.AnimTimeScaleStart_))
        {
            eventName = eventName.Replace(AnimatorEventName.AnimTimeScaleStart_, "");
            float time = float.Parse(eventName);
            roleController.Animator.speed = time;
        }
        else if (eventName.Contains(AnimatorEventName.AnimTimeScaleEnd))
        {
            roleController.Animator.speed = 1;
        }
        else if (eventName.Contains(AnimatorEventName.LookAtTargetStart))
        {
            LookAtTarget = true;
        }
        else if (eventName.Contains(AnimatorEventName.LookAtTargetEnd))
        {
            LookAtTarget = false;
        }
        else if (eventName.Contains(AnimatorEventName.ShowRectIndicate_))
        {
            eventName = eventName.Replace(AnimatorEventName.ShowRectIndicate_, "");
            var values = eventName.Split('_');
            var l = float.Parse(values[0]);
            var w = float.Parse(values[1]);
            var t = float.Parse(values[2]);
            IndicatorManager.Inst.ShowRectangleIndicator().Show(roleController, new Vector3(0, 0.1f, 0), roleController.Animator.transform.forward, new Vector3(w, 1, 0), new Vector3(w, 1, l), t);
        }
        else if (eventName.Contains(AnimatorEventName.ShowSectorIndicate_))
        {
            eventName = eventName.Replace(AnimatorEventName.ShowSectorIndicate_, "");
            var values = eventName.Split('_');
            var r = float.Parse(values[0]);
            var ang = float.Parse(values[1]);
            var t = float.Parse(values[2]);
            IndicatorManager.Inst.ShowAttackIndicator().Show(roleController, roleController.transform.position + Vector3.up * 0.1f, ang, r, t, Color.red);
        }
        else if(eventName.Contains(AnimatorEventName.ChangeRotSpeed_))
        {
            eventName = eventName.Replace(AnimatorEventName.ChangeRotSpeed_, "");
            float value;
            float.TryParse(eventName,out value);
            _move.RotateSpeed = value;
        }
        else if(eventName.Contains(AnimatorEventName.ResetRotSpeed))
        {
            _move.RotateSpeed = IniRotSpeed;
        }
        else if (eventName.Contains("PlayRoarSound"))
        {
            if (AttackRoar != null)
            {
                AudioManager.Inst.PlaySource(AttackRoar);
            }
        }
    }
    public virtual void ShowStartAttackParticle()
    {
        if (StartAttackParticle != null)
        {
            StartAttackParticle.Play();

        }
    }
    public virtual void ShowAttackDamageParticle()
    {
        if (AttackDamgeParticle != null)
        {
            AttackDamgeParticle.Play();

        }
    }

}
