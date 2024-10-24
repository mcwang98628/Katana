using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAttack : RoleAttack
{
    
    private static readonly int AttackType = Animator.StringToHash("AttackType");
    private static readonly int AttackTimes = Animator.StringToHash("AttackTimes");
    protected static readonly int Accumulate = Animator.StringToHash("Accumulate");
    protected static readonly int AccumulateAttackType = Animator.StringToHash("AccumulateAttackType");
    protected static readonly int AccumulateAttack = Animator.StringToHash("AccumulateAttack");
    protected static readonly int RunAccumulateAttack = Animator.StringToHash("RunAccumulateAttack");

    private int currentAttackTimes = 0;
    // [SerializeField]
    // [LabelText("攻击Type")]
    // protected int currentAttackType = 1;
    // [SerializeField]
    // [LabelText("蓄力攻击Type")]
    // protected int currentAccumulateAttackType = 1;
    // public int CurrentAttackType => currentAttackType;
    // public int CurrentAccumulateAttackType => currentAccumulateAttackType;

    private bool _isAccumulateing;
    /// <summary>
    /// 蓄力中。。。
    /// </summary>
    public bool IsAccumulateing => _isAccumulateing;
    /// <summary>
    /// 蓄力开始的时间
    /// </summary>
    public float AccumulateingStartTime { get; private set; }

    public bool IsCanAccumulate => !roleController.IsRolling
                                   && !roleController.IsAttacking
                                   && !roleController.IsDie
                                   && !roleController.isHiting
                                   && !roleController.IsBigSkill
                                   && roleController.IsAcceptInput
                                   && !roleController.IsMoving
                                   && ((PlayerController)roleController).CanMoveSkillUsingCount == 0
                                   && ((PlayerController)roleController).CantMoveSkillUsingCount == 0;

    [HideInInspector]
    public GameObject ComboParticlePool;
    [HideInInspector]
    public List<GameObject> ComboParticles;
    public FeedBackObject AttackFeedback;

    private bool AttackIsCrit;
    // private bool isCanDistributeCirtEvent;

    public int ComboNumber { get; private set; }

    // public AudioClip DashAttackAudio;
    // public AudioClip ChargeAttackAudio;
    public DmgType attackDmgType = DmgType.Physical;
    [ShowInInspector] [ReadOnly] public float SplashDamage = 1f;//溅射伤害
    [ShowInInspector] [ReadOnly] private float criticalProbability = 0.2f;//暴击概率
    [ShowInInspector] [ReadOnly] private float criticalMultiplier = 2; //倍率
    public float CriticalProbability => criticalProbability + roleController.GetAttributeBonusValue(AttributeType.CriticalProbability);
    public float CriticalMultiplier => criticalMultiplier + roleController.GetAttributeBonusValue(AttributeType.CriticalMultiplier);

    //目标满血暴击率
    public float TargetFullHpCriticalProbability => roleController.GetAttributeBonusValue(AttributeType.TargetFullHpCriticalProbability);

    protected override void Awake()
    {
        base.Awake();
        ReloadComboParticle();
        ((PlayerController)roleController).InputAction.OnInputEvent += OnInput;
        EventManager.Inst.AddEvent(EventName.OnRoleInjured, OnRoleInjured);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        // ReSharper disable once DelegateSubtraction
        ((PlayerController)roleController).InputAction.OnInputEvent -= OnInput;
        EventManager.Inst.RemoveEvent(EventName.OnRoleInjured, OnRoleInjured);
    }

#if UNITY_ANDROID || UNITY_IOS
    protected virtual void OnInput(JoyStatusData statusData)
    {
        if (statusData.JoyStatus == UIJoyStatus.OnPressUp || statusData.JoyStatus == UIJoyStatus.Holding)
        {
            roleController.InputAttack();
        }
        
    }
#else
    protected virtual void OnInput()
    {
        if (Input.GetMouseButtonUp(0))
        {
            roleController.InputAttack();
        }
    }
#endif

    protected void SetAccumulateing(bool isAccumulateing)
    {
        if (!IsCanAccumulate)
        {
            isAccumulateing = false;
        }
        if (!this._isAccumulateing && isAccumulateing)
        {
            AccumulateingStartTime = Time.time;
            roleController.Animator.ResetTrigger(AccumulateAttack);
        }
        this._isAccumulateing = isAccumulateing;
        roleController.Animator.SetBool(Accumulate, isAccumulateing);
        // if (!roleController.IsRolling)
        // {
        //     roleController.roleRoll.rollSphere.SetActive(isAccumulateing);
        //     roleController.roleRoll.GameModel.SetActive(!isAccumulateing);
        // }
    }
    
    // public void SetAttackType(int type)
    // {
    //     currentAttackType = type;
    // }
    //
    // public void SetAccumulateAttackType(int type)
    // {
    //     currentAccumulateAttackType = type;
    // }

    protected virtual void OnRoleInjured(string arg1, object arg2)
    {
        var data = (RoleInjuredInfo)arg2;
        if (data.Dmg.AttackerRole == BattleManager.Inst.CurrentPlayer &&
            data.Dmg.HitRoleId != BattleManager.Inst.CurrentPlayer.TemporaryId &&
            data.Dmg.DmgType == DmgType.Physical &&
            data.Dmg.DmgValue >= BattleManager.Inst.CurrentPlayer.AttackPower * 0.2f)
        {
            AddComboNumber();
        }
        

        if (data.RoleId == roleController.TemporaryId && data.Dmg.DmgValue > 0)
        {
            if (IsAccumulateing && data.Dmg.Interruption)
            {
                SetAccumulateing(false);
            }
        }
    }


    public void ReloadComboParticle()
    {
        ComboParticles = new List<GameObject>();
        if (ComboParticlePool != null)
        {
            GameObject InsdPool = ParticleToolkit.DuplicatePaticles(ComboParticlePool);
            //InsdPool.transform.rotation = roleController.Animator.transform.rotation;
            InsdPool.transform.SetParent(GetComponentInChildren<Animator>().transform);
            InsdPool.transform.localPosition = Vector3.zero;
            InsdPool.transform.localRotation = Quaternion.Euler(Vector3.zero);
            for (int i = 0; i < InsdPool.transform.childCount; i++)
            {
                ComboParticles.Add(InsdPool.transform.GetChild(i).gameObject);
            }
        }

    }
    public override void AttackFunc()
    {
        if (!IsAcceptInput || !roleController.IsCanAttack)
        {
            return;
        }



        int attackTimeValue = currentAttackTimes >= 3 ? 1 : currentAttackTimes + 1;
        // roleController.Animator.SetInteger(AttackType, currentAttackType);
        roleController.Animator.SetInteger(AttackTimes, attackTimeValue);
        roleController.Animator.SetTrigger(Attack);


        if (roleController.Animator2 != null)
        {
            roleController.Animator2.SetInteger(AttackTimes, attackTimeValue);
            roleController.Animator2.SetTrigger(Attack);
        }
    }



    private float lastComboTime;
    private void AddComboNumber()
    {
        ComboNumber++;
        lastComboTime = Time.time;
    }

    private void UpdateClearComboNumber()
    {
        if (ComboNumber > 0 && (Time.time - lastComboTime) >= 1)
        {
            ComboNumber = 0;
        }
    }
    protected override void Update()
    {
        base.Update();
        UpdateResetAnimIndex();
        UpdateClearComboNumber();
        //TODO 
        if (BattleManager.Inst != null && BattleManager.Inst.CurrentPlayer != null)
        {
            if (BattleManager.Inst.CurrentPlayer.IsAttacking)
            {
                BattleManager.Inst.CurrentPlayer.Animator.speed = AttackSpeed;
            }
            else
            {
                BattleManager.Inst.CurrentPlayer.Animator.speed = 1;
            }
        }
        //-----------------------------------------------------------------------蓄力攻击逻辑
    }

    #region Combo系统重置计时器

    private bool isUseResetAnimIndex = false;
    private float UpdateResetAnimIndexTimer = 0;
    private float UpdateResetAnimIndexTime = 1;

    void UpdateResetAnimIndex()
    {
        if (!isUseResetAnimIndex)
        {
            UpdateResetAnimIndexTimer = 0;
            return;
        }
        else
        {
            UpdateResetAnimIndexTimer += Time.deltaTime;
        }
        if (UpdateResetAnimIndexTimer >= UpdateResetAnimIndexTime)
        {
            isUseResetAnimIndex = false;
            // currentComboIndex = 0;
            UpdateResetAnimIndexTimer = 0;
        }
    }

    #endregion

    protected override void AnimEvent(GameObject go, string eventName)
    {
        
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
        
        
        base.AnimEvent(go, eventName);
        //释放粒子
        if (eventName.Contains(AnimatorEventName.DmgStart_))
        {
            int CurrentParIndex = currentAttackTimes;
            if (AttackFeedback != null)
                FeedbackManager.Inst.UseFeedBack(roleController, AttackFeedback);
            if (ComboParticles.Count > CurrentParIndex)
            {
                //Debug.Log("ParticleIndex_"+(CurrentParIndex+1));
                GameObject CurrentSwordTrail = ParticleToolkit.DuplicatePaticles(ComboParticles[CurrentParIndex]);
                CurrentSwordTrail.transform.SetParent(gameObject.transform);
                CurrentSwordTrail.SetActive(true);
                CurrentSwordTrail.AddComponent<SelfDestruct>();
            }
            //目标满血暴击率
            float fullHpProba = roleController.EnemyTarget != null ?
                    (roleController.EnemyTarget.CurrentHp == roleController.EnemyTarget.MaxHp
                    ? TargetFullHpCriticalProbability
                    : 0)
                : 0;
            AttackIsCrit = Random.Range(0, 1.0f) < CriticalProbability + fullHpProba;
            // isCanDistributeCirtEvent = true;
        }

        if (eventName.Contains(AnimatorEventName.StartAttack_))
        {
            eventName = eventName.Replace(AnimatorEventName.StartAttack_, "");
            var value = int.Parse(eventName);
            if (value > 0)
            {
                currentAttackTimes = value;
            }
            isUseResetAnimIndex = false;
            //测试代码
            if (value > 0)
            {
                if (roleController.EnemyTarget != null)
                {//朝向目标
                    var dir = roleController.EnemyTarget.transform.position - roleController.transform.position;
                    dir.y = 0;
                    roleController.Animator.transform.forward = dir.normalized;
                }
                else if (roleController.FindEnemyTarget.BreakableTarget != null)
                {
                    var dir = roleController.FindEnemyTarget.BreakableTarget.transform.position - roleController.transform.position;
                    dir.y = 0;
                    roleController.Animator.transform.forward = dir.normalized;
                }
            }
        }
        else if (eventName.Contains(AnimatorEventName.EndAttack_))
        {
            eventName = eventName.Replace(AnimatorEventName.EndAttack_, "");

            int index = int.Parse(eventName);

            if (index == currentAttackTimes)
            {
                isUseResetAnimIndex = true;
            }
        }
    }


    //对目标造成伤害
    protected override void TargetDmg(RoleController target, AttackInfo info)
    {
        if (dmgList.Contains(target))
        {
            return;
        }
        DmgType dmgType = attackDmgType;
        bool isCrit = false;
        //暴击率
        float _criticalMultiplier = 1f;
        if (AttackIsCrit)
        {
            isCrit = true;
            _criticalMultiplier = CriticalMultiplier;
        }
        float dmgValue = 0;
        if (roleController.EnemyTarget != null &&
            target == roleController.EnemyTarget)
        {
            dmgValue = info.AttackMagnification * AttackPower * _criticalMultiplier;
        }
        else
        {
            dmgValue = info.AttackMagnification * AttackPower * SplashDamage * _criticalMultiplier;
        }
        dmgValue += Random.Range(-4f, 4f);

        BuffScriptableObject buff = null;
        if (info.isUseBuff)
        {
            // info.BuffObj.LifeCycle = info.BuffLifeCycle;
            buff = info.BuffObj;
        }
        DamageInfo dmg = new DamageInfo(target.TemporaryId, dmgValue, roleController, roleController.transform.position, dmgType,
        info.isInterruption, info.isUseRepel, info.RepelTime, info.RepelSpeed, false, null, buff, info.BuffLifeCycle, isCrit,
        IsNormalAttacking?DamageAttackType.NormalAttack:
        IsRollAttacking?DamageAttackType.RollAttack:DamageAttackType.None);

        dmg.RepelDir = info.RepelMove;
        dmg.SetFeedBack(currentDmgAttackInfo.HitFeedBack);
        
        target.HpInjured(dmg);

        EventManager.Inst.DistributeEvent(EventName.OnPlayerAttackHitEnemy, dmg);
        if (dmg.IsCrit)
        {
            EventManager.Inst.DistributeEvent(EventName.OnPlayerAttackCrit, dmg);
        }
        // if (isCanDistributeCirtEvent)
        // {
        //     isCanDistributeCirtEvent = false;
        //     EventManager.Inst.DistributeEvent(EventName.OnPlayerAttackCrit);
        // }
        dmgList.Add(target);

    }
    public void SetSplashDamage(float value)
    {
        SplashDamage += value;
    }
    //看向目标
    public virtual void LookAtTarget()
    {
        if (roleController.EnemyTarget != null)
        {
            var dir = roleController.EnemyTarget.transform.position - roleController.transform.position;
            dir.y = 0;
            roleController.Animator.transform.forward = dir.normalized;
        }
    }
    public override void PlayAttackAudio()
    {
        // if(currentComboIndex==998)
        // {
        //     if(ChargeAttackAudio!=null)
        //     {
        //         AudioManager.Inst.PlaySource(ChargeAttackAudio);
        //     }
        //     return;
        // }
        // if(currentComboIndex==999)
        // {
        //     if(DashAttackAudio!=null)
        //     {
        //         AudioManager.Inst.PlaySource(DashAttackAudio);
        //     }
        // }
        // else
        {
            base.PlayAttackAudio();
        }
    }
    public override void DoSomeThingBeforeDamage(AttackInfo ai)
    {
        if (ai.isUseAutoRotate && roleController.EnemyTarget != null)
        {
            var dir = roleController.EnemyTarget.transform.position - roleController.Animator.transform.position;
            float ang = Vector3.Angle(roleController.Animator.transform.forward, dir);
            if (ai.AutoRotateAng >= ang)
            {
                //产生伤害时会再次偷偷瞄准
                LookAtTarget();
            }
        }
    }
    protected override void DamageCalculation(AttackInfo info)
    {
        base.DamageCalculation(info);

        StartCoroutine(DelayOnHitAliveEnemy());

    }
    IEnumerator DelayOnHitAliveEnemy()
    {
        yield return new WaitForEndOfFrame();

        //击中目标是不可击退的就不位移
        if (roleController.EnemyTarget != null && dmgList.Count > 0)
        {
            if (!roleController.EnemyTarget.IsCanHitFly && !roleController.EnemyTarget.IsDie)
                roleController.StopFastMove();
        }

        /*
        //打中人就不位移
        int aliveEnemyCount = 0;
        for (int i = 0; i < dmgList.Count; i++)
        {
            if (!dmgList[i].IsDie)
                aliveEnemyCount++;
        }
        if (aliveEnemyCount > 0)
        {
            roleController.StopFastMove();
        }
        */
    }

    public override void BackMoveAnim()
    {
        
        if (((PlayerController)roleController).CanMoveSkillUsingCount <= 0 && ((PlayerController)roleController).CantMoveSkillUsingCount <= 0 )
        {
            StopAttack();
            roleController.Animator.Play("Move");
        }
    }
}
