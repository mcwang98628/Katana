using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoleAttack : MonoBehaviour
{
    protected RoleController roleController;
    protected static readonly int AttackStatus = Animator.StringToHash("AttackStatus");
    protected static readonly int Attack = Animator.StringToHash("Attack");
    protected static readonly int Attack_B = Animator.StringToHash("Attack-B");
    [SerializeField]
    protected int attackPower;
    [ShowInInspector][ReadOnly]protected float attackSpeed = 1;    
    [ShowInInspector][ReadOnly]protected float magicPower = 1;

    public bool IsRollAttacking => rollAttackTimes > 0;
    protected int rollAttackTimes;
    public bool IsNormalAttacking => rollNormalAttackTimes > 0;
    protected int rollNormalAttackTimes;
    
    //破防等级
    public int AntiDefenseLevel => antiDefenseLevel + (int)roleController.GetAttributeBonusValue(AttributeType.AntiDefenseLevel);
    [SerializeField][LabelText("破防等级")]
    protected int antiDefenseLevel;
    
    //攻击距离
    private float attackDistance = 0;
    public float AttackDistance => attackDistance + roleController.GetAttributeBonusValue(AttributeType.AttackDistance);

    public int AttackPower
    {
        get
        {
            int value = attackPower + (int)roleController.GetAttributeBonusValue(AttributeType.AttackPower);
            if (value<1)
            {
                value = 1;
            }
            return value;
        }
    }
    public int OriginalAttackPower => attackPower + (int)roleController.GetPeripheryAttributeBonusValue(AttributeType.AttackPower);

    /// <summary>
    /// 法术强度
    /// </summary>
    public float MagicPower => magicPower + roleController.GetAttributeBonusValue(AttributeType.MagicPower);
    
    public float AttackSpeed
    {
        get
        {
            float value = attackSpeed + roleController.GetAttributeBonusValue(AttributeType.AttackSpeed);
            if (value<0.1f)
            {
                value = 0.1f;
            }
            return value;
        }
    }
    public float OriginalAttackSpeed => attackSpeed;

    [SerializeField] protected int currentAttackStatus;
    
    [SerializeField] private List<AudioClip> attackAudio;

    [ShowInInspector]
    public bool IsDmging => dmging;
    protected bool dmging;
    protected AttackInfo currentDmgAttackInfo;
    protected List<RoleController> dmgList = new List<RoleController>();
    protected List<BreakableObj> breakableObjList = new List<BreakableObj>();




    protected virtual void Awake()
    {
        roleController = GetComponent<RoleController>();
        EventManager.Inst.AddAnimatorEvent(AnimEvent);
    }

    protected virtual void OnDestroy()
    {
        EventManager.Inst.RemoveAnimatorEvent(AnimEvent);
    }

    public virtual void Init(int _attackPower,float magic)
    {
        attackPower = _attackPower; //(int)(_attackPower * BattleManager.Inst.Difficulity);
        magicPower = magic;
        // Debug.LogError(AttackPower);
    }





    //是否接收输入
    public bool IsAcceptInput => roleController.IsAcceptInput;
    /// <summary>
    /// 攻击
    /// </summary>
    public virtual void AttackFunc()
    {
        if (!IsAcceptInput || !roleController.IsCanAttack)
        {
            return;
        }
        if (roleController.EnemyTarget != null)
        {
            var dir = roleController.EnemyTarget.transform.position - roleController.transform.position;
            dir.y = 0;
            roleController.Animator.transform.forward = dir.normalized;
        }
        roleController.Animator.SetTrigger(Attack);
        
        if (roleController.Animator2 != null)
            roleController.Animator2.SetTrigger(Attack);
    }


    public virtual void AttackFunc(int attackStatus)
    {
        //Debug.Log(attackStatus);
        currentAttackStatus = attackStatus;
        AttackFunc();
    }

    //停止攻击
    public void StopAttack()
    {
        roleController.SetIsAttacking(false);
        ReAttack();
        roleController.SetAcceptInput(true);

        // HideAttackerDebug();
        ClearDmg();
    }

    public virtual void BackMoveAnim()
    {
    }

    public void ReAttack()
    {
        roleController.Animator.ResetTrigger(Attack);
        roleController.Animator.ResetTrigger(Attack_B);


        if (roleController.Animator2 != null)
        {
            roleController.Animator2.ResetTrigger(Attack);
            roleController.Animator2.ResetTrigger(Attack_B);
        }
    }
    public void ReAttackA()
    {
        roleController.Animator.ResetTrigger(Attack);
    }
    public void ReAttackB()
    {
        roleController.Animator.ResetTrigger(Attack_B);
    }





    protected virtual void Update()
    {
        if (IsDmging)
        {
            if (currentDmgAttackInfo != null && !roleController.IsDie && roleController.IsAttacking)
            {
                DamageCalculation(currentDmgAttackInfo);
            }
            
        }
    }




    #region 伤害处理
    //选出攻击的目标
    protected virtual void DamageCalculation(AttackInfo info)
    {
        List<BreakableObj> breakableObjs = null;
        switch (info.AttackType)
        {
            case AttackType.Round:
                breakableObjs = BreakableObjManager.Inst.BreakObjsInRange(roleController, info.AttackRadius + AttackDistance, 360);

                foreach (KeyValuePair<string, RoleController> enemy in BattleManager.Inst.EnemyTeam)
                {
                    if (enemy.Value.IsDie)
                    {
                        continue;
                    }
                    Vector3 v3 = enemy.Value.transform.position - roleController.transform.position;
                    if (v3.magnitude <= info.AttackRadius + AttackDistance)
                    {
                        TargetDmg(enemy.Value, info);
                    }
                }
                break;

            case AttackType.Sector:
                breakableObjs = BreakableObjManager.Inst.BreakObjsInRange(roleController, info.AttackRadius + AttackDistance, 360);

                foreach (KeyValuePair<string, RoleController> enemy in BattleManager.Inst.EnemyTeam)
                {
                    if (enemy.Value.IsDie)
                    {
                        continue;
                    }
                    Vector3 v3 = enemy.Value.transform.position - roleController.transform.position;
                    if (v3.magnitude > info.AttackRadius + AttackDistance)
                    {
                        continue;
                    }

                    float angle = Vector3.Angle(roleController.Animator.transform.forward, v3);
                    if (angle > info.AttackAngle * 0.5f)
                    {
                        continue;
                    }

                    TargetDmg(enemy.Value, info);
                }
                break;
        }

        foreach (BreakableObj obj in breakableObjs)
        {
            if (breakableObjList.Contains(obj))
            {
                continue;
            }
            breakableObjList.Add(obj);
            obj.BreakObj();
        }

    }

    //对目标造成伤害
    protected virtual void TargetDmg(RoleController target, AttackInfo info)
    {
        if (dmgList.Contains(target))
        {
            return;
        }
        dmgList.Add(target);
        /*
        //普通单位不暴击
        //暴击率
        float criticalMultiplier = 1f;
        int probability = Random.Range(0, 101);
        if (probability <= CriticalProbability)
        {
            criticalMultiplier = CriticalMultiplier;
        }
        */


        /*DamageInfo dmg = new DamageInfo();
        dmg.DmgValue = info.AttackMagnification * attackPower * criticalMultiplier;
        dmg.IsUseMove = info.isUseRepel;
        dmg.Interruption = info.isInterruption;
        dmg.Attacker = roleController;
        dmg.AttackPoint = roleController.transform.position;
        
        if (info.isUseRepel)
        {
            dmg.MoveTime = info.RepelTime;
            dmg.MoveSpeed = info.RepelSpeed;
            //            Vector2 v2 = roleController.GetWorldDir(info.RepelMove);
            //            dmg.dmgV3 = new Vector3(v2.x, 0, v2.y);
        } */
        float damageValue = info.AttackMagnification * AttackPower + Random.Range(-4f, 4f);
        
        BuffScriptableObject buff = null;
        if (info.isUseBuff)
        {
            // info.BuffObj.LifeCycle = info.BuffLifeCycle;
            buff = info.BuffObj;
        }
        DamageInfo dmg = new DamageInfo(target.TemporaryId,damageValue,
            roleController, roleController.transform.position, DmgType.Physical,
            info.isInterruption, info.isUseRepel, info.RepelTime, info.RepelSpeed,
            false,null,buff,info.BuffLifeCycle,false,
            IsRollAttacking?DamageAttackType.RollAttack:DamageAttackType.None);
        dmg.SetFeedBack(currentDmgAttackInfo.HitFeedBack);
        target.HpInjured(dmg);
    }
    public void ClearDmg()
    {
        dmging = false;
        currentDmgAttackInfo = null;
        dmgList.Clear();
        breakableObjList.Clear();
    }
    #endregion

    #region AttackDebug

    // protected void ShowAttackerDebug(AttackType attackType, float attackRadius, float attackAngle)
    // {
    //     if (isUseDebug && roleAttackerDebug != null)
    //     {
    //         roleAttackerDebug.SetEnable(true);
    //         roleAttackerDebug.SetValue(attackType, attackRadius, attackAngle);
    //     }
    // }
    // public void HideAttackerDebug()
    // {
    //     if (isUseDebug && roleAttackerDebug != null)
    //     {
    //         roleAttackerDebug.SetEnable(false);
    //     }
    // }


    #endregion

    protected int currentAttackIndex;
    /// <summary>
    /// 动画事件处理
    /// </summary>
    /// <param name="go"></param>
    /// <param name="eventName"></param>
    protected virtual void AnimEvent(GameObject go, string eventName)
    {
        //Debug.Log(eventName);
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
        

        if (eventName.Contains(AnimatorEventName.StartAttack_))
        {
            eventName = eventName.Replace(AnimatorEventName.StartAttack_, "");
            currentAttackIndex = int.Parse(eventName);
            roleController.SetIsAttacking(true);
        }
        else if (eventName.Contains(AnimatorEventName.EndAttack_))
        {
            eventName = eventName.Replace(AnimatorEventName.EndAttack_, "");
            int index = int.Parse(eventName);
            if (index == currentAttackIndex)
            {
                roleController.SetIsAttacking(false);
            }
            ClearDmg();
        }
        else if (AnimatorEventName.ReAttack == eventName)
        {
            ReAttack();
        }
        else if (AnimatorEventName.ReAttackA == eventName)
        {
            ReAttackA();
        }
        else if (AnimatorEventName.ReAttackB == eventName)
        {
            ReAttackB();
        }
        else if (AnimatorEventName.RollAttackStart == eventName)
        {
            rollAttackTimes++;
        }
        else if (AnimatorEventName.RollAttackEnd == eventName)
        {
            rollAttackTimes--;
        }
        else if (AnimatorEventName.RollNormalAttackStart == eventName)
        {
            rollNormalAttackTimes++;
        }
        else if (AnimatorEventName.RollNormalAttackEnd == eventName)
        {
            rollNormalAttackTimes--;
        }else if (AnimatorEventName.BigSkillStart==eventName)
        {
            roleController.SetIsBigSkill(true);
        }
        else  if(AnimatorEventName.BigSkillEnd==eventName)
        {
            roleController.SetIsBigSkill(false);
        }
        else if (eventName.Contains(AnimatorEventName.DmgStart_))
        {
            ClearDmg();
          
            eventName = eventName.Replace(AnimatorEventName.DmgStart_, "");


            // AttackInfo ai = DataManager.Inst.GetAttackInfo(eventName);
            DataManager.Inst.LoadAttackInfo(eventName, delegate(AttackInfo ai)
            {
                if (ai == null)
                {
                    PlayAttackAudio();
                    return;
                }
                DoSomeThingBeforeDamage(ai);
                currentDmgAttackInfo = ai;
            
                if (currentDmgAttackInfo != null)
                {
                    if (currentDmgAttackInfo.isUseMove && currentDmgAttackInfo.isUseRepel)//使用击退的时候才位移
                    {
                        Vector2 v2 = BattleTool.GetWorldDir(roleController.Animator.transform,currentDmgAttackInfo.AttackMove);
                        roleController.FastMove(currentDmgAttackInfo.MoveTime, currentDmgAttackInfo.MoveSpeed, new Vector3(v2.x, 0, v2.y),null, currentDmgAttackInfo.MoveCurve);
                    }
                    //                ShowAttackerDebug(currentDmgAttackInfo.AttackType, currentDmgAttackInfo.AttackRadius, currentDmgAttackInfo.AttackAngle);
                    if (ai.isPlayAudio)
                    {
                        PlayAttackAudio();
                    }
                }
                dmging = true;

                EventManager.Inst.DistributeEvent(EventName.OnRoleAttack, roleController.TemporaryId);
            });
        }
        else if (eventName.Contains(AnimatorEventName.DmgEnd_))
        {
            // HideAttackerDebug();
            ClearDmg();
        }
        else if (eventName.Contains(AnimatorEventName.MoveAction_))
        {
            eventName = eventName.Replace(AnimatorEventName.MoveAction_, "");
            DataManager.Inst.LoadMoveInfo(eventName, delegate(MoveInfo mi)
            {
                if (mi != null)
                {
                    Vector2 v2 = BattleTool.GetWorldDir(roleController.Animator.transform,mi.AttackMove);
                    roleController.FastMove(mi.MoveTime, mi.MoveSpeed, new Vector3(v2.x, 0, v2.y), null,mi.MoveCurve);
                }
            });
        }
        // else if (eventName == AnimatorEventName.EndInput)
        // {
        //     roleController.SetAcceptInput(false);
        // }
        // else if (eventName == AnimatorEventName.StartInput)
        // {
        //     roleController.SetAcceptInput(true);
        // }
    }
    public virtual void DoSomeThingBeforeDamage(AttackInfo ai)
    {
    }

   

    public virtual void PlayAttackAudio()
    {
        if (attackAudio != null && attackAudio.Count > 0)
        {
            AudioManager.Inst.PlaySource(attackAudio[Random.Range(0, attackAudio.Count)]);
        }
    }

}
