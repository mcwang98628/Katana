using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;


public class RoleHealth : MonoBehaviour
{
    [SerializeField] private List<AudioClip> deadAudio;
    [SerializeField] private int isGodCount;

    public bool IsGod
    {
        get { return isGodCount > 0; }
    }

    //是否可被打断
    public bool IsAcceptInterruption => (isAcceptInterruption > 0) && isCanHitInterrupt;
    private int isAcceptInterruption = 1;

    public int HpBarCount => hpBarCount;
    [SerializeField] [LabelText("血条数量")] protected int hpBarCount = 2;

    //防御等级
    public virtual int DefenseLevel => defenseLevel + (int) roleController.GetAttributeBonusValue(AttributeType.DefenseLevel);
    [SerializeField] [LabelText("防御等级")] protected int defenseLevel;

    [SerializeField] private bool isCanHitFly;
    public bool IsCanHitFly => isCanHitFly;

    [LabelText("是否可以被打断")] [SerializeField]
    private bool isCanHitInterrupt = true;

    public bool IsCanHitInterrupt => isCanHitInterrupt;

    [ShowInInspector] public bool IsDeath => CurrentHp <= 0;
    [ShowInInspector] public float CurrentHp => currentHp;

    public float IniHp = 20;

    //[ShowInInspector]
    public float MaxHp
    {
        get
        {
            float value = maxHp + roleController.GetAttributeBonusValue(AttributeType.MaxHp);
            if (value < 1)
            {
                value = 1;
            }

            return value;
        }
    }

    public float OriginalMaxHp => maxHp + roleController.GetPeripheryAttributeBonusValue(AttributeType.MaxHp);
    protected float currentHp;
    protected float maxHp;

    //闪避概率0-100
    private int dodgeProbability = 0;

    public int DodgeProbability
    {
        get
        {
            int value = dodgeProbability +
                        (int) roleController.GetAttributeBonusValue(AttributeType.DodgeProbability);
            if (value >= 50)
                value = 50;

            return value;
        }
    }

    //受伤系数
    protected float injuryMultiplier = 1;
    public float InjuryMultiplier => injuryMultiplier; // + roleController.GetAttributeBonusValue(AttributeType.InjuryMultiplier);
    [SerializeField] public UnityEvent OnDead = new UnityEvent();

    protected RoleController roleController;
    private static readonly int Die = Animator.StringToHash("Die");
    protected static readonly int Hit = Animator.StringToHash("Hit");
    EnemyShield shield;

    public float HpTreatMultiplier => hpTreatMultiplier + roleController.GetAttributeBonusValue(AttributeType.HpTreatMultiplier);
    private float hpTreatMultiplier = 1;


    protected virtual void Awake()
    {
        roleController = GetComponent<RoleController>();
        // if (roleController.roleTeamType != RoleTeamType.Player)
        // {
        //     Init(IniHp);
        // }

        shield = GetComponent<EnemyShield>();
    }

    public void SetIsAcceptInterruption(bool isok)
    {
        if (isok)
        {
            isAcceptInterruption++;
        }
        else
        {
            isAcceptInterruption--;
        }
    }

    //出房间会清除所有怪物
    private void OnEnable()
    {
        EventManager.Inst.AddEvent(EventName.EnterNextRoom, DestroyAllMons);
    }

    protected virtual void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.EnterNextRoom, DestroyAllMons);
    }


    //特殊处理，直接清除怪物
    public void DestroyAllMons(string arg1, object arg2)
    {
        if (roleController.roleTeamType != RoleTeamType.Player)
        {
            if (!roleController.IsDie)
            {
                Destroy(gameObject);
            }
        }
    }


    public virtual void Init(float _maxHp)
    {
        maxHp = _maxHp;
        currentHp = maxHp;
    }


    public void UpdateCurrentHp()
    {
        if (currentHp > MaxHp)
            currentHp = MaxHp;
    }

    public void SetGod(bool god)
    {
        if (god)
        {
            //进入无敌状态
            if (isGodCount == 0)
            {
                EventManager.Inst.DistributeEvent(EventName.GodStart);
            }

            isGodCount++;
        }
        else
        {
            isGodCount--;
            //退出无敌状态
            if (isGodCount == 0)
            {
                EventManager.Inst.DistributeEvent(EventName.GodStop);
            }
            // if (isGodCount<0)
            // {
            //     isGodCount = 0;
            // }
        }
    }

    private int isForcedDodgeCount;
    public bool IsForcedDodge => isForcedDodgeCount > 0;

    public void SetDodge(bool isOk)
    {
        if (isOk)
        {
            isForcedDodgeCount++;
        }
        else
        {
            isForcedDodgeCount--;
            if (isForcedDodgeCount < 0)
            {
                isForcedDodgeCount = 0;
            }
        }
    }


    public void SetInjuryMultiplier(float value)
    {
        injuryMultiplier = injuryMultiplier * (1 - value);
    }

    public void SetUnInjuryMultiplier(float value)
    {
        injuryMultiplier = injuryMultiplier / (1 - value);
    }


    public virtual void Treatment(TreatmentData value)
    {
        if (IsDeath && !value.IsResurrection)
        {
            return;
        }

        value.TreatmentValue *= HpTreatMultiplier > 0 ? HpTreatMultiplier : 0;

        currentHp += value.TreatmentValue;
        if (currentHp > MaxHp)
        {
            currentHp = MaxHp;
        }

        if (currentHp < 1)
        {
            currentHp = 1;
        }

        roleController.Animator.SetBool(Die, IsDeath);
        if (roleController.Animator2 != null)
        {
            roleController.Animator2.SetBool(Die, IsDeath);
        }

        EventManager.Inst.DistributeEvent(EventName.OnRoleTreatment, value);
    }

    public void SetCurrentHp(float value)
    {
        currentHp = value;
    }

    //根据最大血量百分比 设置当前血量
    public void SetCurrentHpOfMaxHpPercentage(float value)
    {
        currentHp = maxHp * value;
        if (currentHp > MaxHp)
        {
            currentHp = MaxHp;
        }
    }

    void ActiveInterruption(DamageInfo dmg)
    {
        if (roleController.roleTeamType == RoleTeamType.Player)
        {
            Vector3 dmgDir = dmg.AttackPoint - transform.position;
            dmgDir.y = 0;
            roleController.Animator.transform.forward = dmgDir;
        }

        roleController.Animator.SetTrigger(Hit);
        if (roleController.Animator2 != null)
        {
            roleController.Animator2.SetTrigger(Hit);
        }

        roleController.roleMove.StopMove();
        roleController.SetAcceptInput(true);
        roleController.SetIsAttacking(false);
    }

    public virtual void Injured(DamageInfo dmg)
    {
        if (IsDeath)
        {
            return;
        }

        //player的id是-1，如果是enemy打enemy就减半伤害。
        if (!dmg.IsPlayer && roleController.roleTeamType != RoleTeamType.Player)
        {
            dmg.DmgValue *= 0.5f;
        }

        //触发翻滚无敌闪避
        if (dmg.DmgType != DmgType.Unavoidable && dmg.DmgType != DmgType.Vampire && dmg.DmgValue > 1)
        {
            int dodgeValue = Random.Range(0, 100);
            if (IsForcedDodge || dodgeValue < DodgeProbability)
            {
                RoleInjuredInfo roleInjuredInfoGod = new RoleInjuredInfo();
                roleInjuredInfoGod.RoleId = roleController.TemporaryId;
                roleInjuredInfoGod.Dmg = dmg;
                EventManager.Inst.DistributeEvent(EventName.OnRoleDodgeInjured, roleInjuredInfoGod);
                return;
            }
        }


        if (dmg.BuffScriptableObject != null)
        {
            roleController.roleBuffController.AddBuff(DataManager.Inst.ParsingBuff(dmg.BuffScriptableObject, dmg.BuffLifeCycle), dmg.AttackerRole);
        }

        if (IsGod && dmg.DmgType != DmgType.Unavoidable)
        {
            RoleInjuredInfo roleInjuredInfoGod = new RoleInjuredInfo();
            roleInjuredInfoGod.RoleId = roleController.TemporaryId;
            roleInjuredInfoGod.Dmg = dmg;
            EventManager.Inst.DistributeEvent(EventName.OnRoleGodInjured, roleInjuredInfoGod);
            return;
        }

        if (shield)
        {
            if (shield.SheildTakeDamage(roleController, dmg))
                return;
        }

        if (IsAcceptInterruption
            && (dmg.DmgType == DmgType.Physical || dmg.DmgType == DmgType.Explosion)
            && !roleController.IsRolling)
        {
            if (dmg.AttackerRole != null)
            {
                if (dmg.AttackerRole.roleAttack != null)
                {
                    if (dmg.AttackerRole.roleAttack.AntiDefenseLevel >= this.DefenseLevel || dmg.Interruption)
                    {
                        ActiveInterruption(dmg);
                    }
                }
                else
                {
                    ActiveInterruption(dmg);
                }
            }
            else
            {
                ActiveInterruption(dmg);
            }
        }


        if (dmg.IsUseMove && isCanHitFly && !roleController.IsRolling)
        {
            Vector3 v3;
            if (dmg.RepelDir == Vector2.zero)
            {
                v3 = transform.position - dmg.AttackPoint;
                v3 = new Vector3(v3.x, 0, v3.z).normalized;
            }
            else
            {
                float ang = Vector2.Angle(Vector2.up, dmg.RepelDir);

                v3 = Quaternion.Euler(0, ang, 0) * dmg.AttackerRole.Animator.transform.forward;
            }

            roleController.FastMove(dmg.MoveTime, dmg.MoveSpeed, v3, null);
            roleController.Animator.transform.forward = v3 * -1;
        }

        /*
        if (frozenFrame && dmg.DmgType == DmgType.Physical)
        {
            if (frozenFrameCoroutine != null)
            {
                StopCoroutine(frozenFrameCoroutine);
                frozenFrameCoroutine = null;
            }
            frozenFrameCoroutine = StartCoroutine(FrozenFrameAnim(frozenFrameTime));
        }
        */

        if (dmg.DmgType == DmgType.Physical) // || dmg.DmgType == DmgType.PhysicalCrit)
        {
            if (frozenFrameCoroutine != null)
            {
                StopCoroutine(frozenFrameCoroutine);
                frozenFrameCoroutine = null;
            }

            //关掉冻帧
            frozenFrameCoroutine = StartCoroutine(FrozenFrameAnim(0f));
        }

        dmg.DmgValue *= InjuryMultiplier;
        currentHp -= dmg.DmgValue;
        if (currentHp < 0)
        {
            currentHp = 0;
        }

        if (IsDeath)
        {
            roleController.Animator.SetBool(Die, IsDeath);
            roleController.Animator.speed = 1;
            if (roleController.Animator2 != null)
            {
                roleController.Animator2.SetBool(Die, IsDeath);
                roleController.Animator2.speed = 1;
            }

            OnDead.Invoke();
            OnDeath(dmg.AttackerID, dmg.AttackerRole);

            //判断击飞


            if (dmg.AttackerRole != roleController &&
                (dmg.DmgType == DmgType.ArrowHit || dmg.DmgType == DmgType.Physical || dmg.DmgType == DmgType.Explosion))
            {
                Vector3 PlayerDir = BattleManager.Inst.CurrentPlayer.transform.position - transform.position;
                roleController.FastMove(0.1f, 30f, -PlayerDir.normalized, null);
            }

            if (deadAudio != null && deadAudio.Count > 0)
            {
                deadAudio[Random.Range(0, deadAudio.Count - 1)].Play();
            }
        }

        RoleInjuredInfo roleInjuredInfo = new RoleInjuredInfo();
        roleInjuredInfo.RoleId = roleController.TemporaryId;
        roleInjuredInfo.Dmg = dmg;
        EventManager.Inst.DistributeEvent(EventName.OnRoleInjured, roleInjuredInfo);
    }

    private Coroutine frozenFrameCoroutine;

    IEnumerator FrozenFrameAnim(float time)
    {
        roleController.Animator.speed = 0;
        yield return new WaitForSecondsRealtime(time);
        roleController.Animator.speed = 1;
    }

    public virtual void OnDeath(int AttackerID, RoleController AttackerRole)
    {
        RoleDeadEventData data = new RoleDeadEventData() {AttackerRole = AttackerRole, DeadRole = roleController, RolePos = transform.position};
        EventManager.Inst.DistributeEvent(EventName.OnRoleDead, data);
        // BattleManager.Inst.RoleUnRegistered();
    }
}

public enum DmgType
{
    Physical,
    Other,
    Fire,
    Explosion,
    Thunder,

    // PhysicalCrit,
    ArrowHit,
    ThunderWeak,
    Unavoidable,
    Vampire, //吸血
    Spike

    // DmgBuffOnTouchCrit,//AOE暴击
    //PhysicalShield
}

public enum DamageAttackType
{
    None, //无类型
    NormalAttack, //普通攻击
    RollAttack, //翻滚
    Skill, //技能
}

[System.Serializable]
public class DamageInfo
{
    public DamageInfo(
        string hitRoleId,
        float dmgValue,
        RoleController attacker,
        Vector3 attackPoint,
        DmgType dmgType = DmgType.Other,
        bool interruption = false,
        bool isUseMove = false,
        float moveTime = 0,
        float moveSpeed = 0,
        bool isRemotely = false,
        MonoBehaviour remotelyObject = null,
        BuffScriptableObject buffScriptableObject = null,
        BuffLifeCycle buffLifeCycle = null,
        bool isCrit = false,
        DamageAttackType attackType = DamageAttackType.None)
    {
        DmgValue = dmgValue;
        AttackerID = attacker.UniqueID;
        AttackerRole = attacker;
        IsPlayer = attacker.IsPlayer;
        DmgType = dmgType;
        Interruption = interruption;
        IsUseMove = isUseMove;
        MoveTime = moveTime;
        MoveSpeed = moveSpeed;
        AttackPoint = attackPoint;
        IsRemotely = isRemotely;
        RemotelyObject = remotelyObject;
        BuffScriptableObject = buffScriptableObject;
        BuffLifeCycle = buffLifeCycle;
        HitRoleId = hitRoleId;
        IsCrit = isCrit;
        AttackType = attackType;
    }

    public void SetFeedBack(FeedBackObject feedBackObject)
    {
        InjuredFeedBackObject = feedBackObject;
    }

    public DamageAttackType AttackType; //翻滚攻击
    public float DmgValue; //伤害
    public bool Interruption; //打断
    public bool IsUseMove;
    public float MoveTime;
    public float MoveSpeed;
    public int AttackerID;
    public RoleController AttackerRole;
    public bool IsPlayer;
    public Vector3 AttackPoint; //攻击的点
    public DmgType DmgType = DmgType.Other;
    public Vector2 RepelDir;

    public string HitRoleId; //被击中Role的临时ID

    public bool IsCrit;

    //远程
    public bool IsRemotely;
    public MonoBehaviour RemotelyObject;

    //Buff
    public BuffScriptableObject BuffScriptableObject;
    public BuffLifeCycle BuffLifeCycle;

    public FeedBackObject InjuredFeedBackObject = null;
}

public class TreatmentData
{
    public TreatmentData(float treatmentValue, string roleId, bool isResurrection = false)
    {
        TreatmentValue = treatmentValue;
        RoleId = roleId;
        IsResurrection = isResurrection;
    }

    public float TreatmentValue;
    public string RoleId; //被 治疗的RoleID
    public bool IsResurrection;
}

public enum DmgTarget
{
    Player,
    Enemy,
    All,
}

public struct RoleInjuredInfo
{
    public string RoleId;
    public DamageInfo Dmg;
}

public class RoleDeadEventData
{
    public RoleController AttackerRole; //攻击者
    public RoleController DeadRole; //被击杀的角色
    public Vector3 RolePos; //角色坐标
}