using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BehaviorDesigner.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using DG.Tweening;


public class RoleController : MonoBehaviour
{
    #region Runtime

    [ShowInInspector] [ReadOnly] [LabelText("唯一ID")] [BoxGroup("RunTime—Id信息")]
    public int UniqueID { get; protected set; }
    [ShowInInspector] [ReadOnly] [LabelText("临时ID")] [BoxGroup("RunTime—Id信息")]
    public string TemporaryId { get; private set; }

    #endregion

    #region 基础组件

    [LabelText("刚体")] [BoxGroup("Role基础组件")]
    public Rigidbody Rigidbody;
    [LabelText("动画模型")] [BoxGroup("Role基础组件")]
    public Animator Animator;
    // [LabelText("动画模型")] [BoxGroup("Role基础组件")]
    [HideInInspector]
    public Animator Animator2;
    [LabelText("Item管理")] [BoxGroup("Role基础组件")]
    public RoleItemController roleItemController;
    [LabelText("Buff管理")] [BoxGroup("Role基础组件")]
    public RoleBuffController roleBuffController;
    [LabelText("环绕物 管理")] [BoxGroup("Role基础组件")]
    public RoleSurroundController roleSurroundController;
    [BoxGroup("Role基础组件")]
    public RoleMove roleMove;
    [BoxGroup("Role基础组件")]
    public RoleRoll roleRoll;
    [BoxGroup("Role基础组件")]
    public RoleAttack roleAttack;
    [BoxGroup("Role基础组件")]
    public RoleHealth roleHealth;
    [BoxGroup("Role基础组件")]
    public RolePower rolePower;
    [BoxGroup("Role基础组件")]
    [LabelText("节点")]
    public RoleNode roleNode;
    [BoxGroup("Role基础组件")]
    public FindTarget FindEnemyTarget;

    public RoleController EnemyTarget
    {
        get
        {
            if (FindEnemyTarget == null)
            {
                return null;
            }
            return FindEnemyTarget.EnemyTarget;
        }
    }

    [Button("Get组件")]
    public virtual void GetAllRoleComponent()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        roleItemController = GetComponent<RoleItemController>();
        roleBuffController = GetComponent<RoleBuffController>();
        roleSurroundController = GetComponent<RoleSurroundController>();
        roleMove = GetComponent<RoleMove>();
        roleRoll = GetComponent<RoleRoll>();
        roleAttack = GetComponent<RoleAttack>();
        roleHealth = GetComponent<RoleHealth>();
        rolePower = GetComponent<RolePower>();
        roleNode = GetComponent<RoleNode>();
        FindEnemyTarget = GetComponent<FindTarget>();
    }

    #endregion

    #region 特殊状态

    [ReadOnly] [LabelText("IsAuto?")] [BoxGroup("特殊状态")]
    public bool IsAuto { get; set; } = true;
    
    [ReadOnly] [BoxGroup("特殊状态")]
    public RoleTeamType roleTeamType = RoleTeamType.Player;

    public virtual bool IsAcceptInput => isAcceptInput>0;
    private int isAcceptInput = 1;
    public bool IsAcceptAttackInput => roleAttack.IsAcceptInput;
    public bool IsAcceptInterruption => roleHealth.IsAcceptInterruption;
    public bool IsCanHitFly => roleHealth.IsCanHitFly;
    public bool IsPlayer => BattleManager.Inst.CurrentPlayer == this;
    #endregion
    
    #region 状态

    public bool IsDie => roleHealth.IsDeath;
    public virtual bool IsCanMove => !IsDie && !IsDizziness && !IsRolling && !IsFastMoving && !IsDmging;
    public virtual bool IsCanRoll => !isRolling && !IsDie && !IsDizziness && rolePower != null && rolePower.IsCanRoll;
    public virtual bool IsCanAttack => !IsDie && !IsDizziness && !IsFreeze;
    [ReadOnly] [LabelText("移动中")][BoxGroup("状态")]
    public bool IsMoving { get; protected set; }
    [ReadOnly] [LabelText("受击中")][BoxGroup("状态")]
    public bool isHiting { get;protected set; }

    [ReadOnly]
    [LabelText("冰冻中")]
    [BoxGroup("状态")]
    public bool IsFreeze => isFreeze>0;
    private int isFreeze;
    [ShowInInspector] [LabelText("眩晕")][BoxGroup("状态")]
    public bool IsDizziness => isDizziness>0;
    private int isDizziness;
    [ShowInInspector] [LabelText("翻滚中")][BoxGroup("状态")]
    public bool IsRolling => isRolling;
    private bool isRolling;

    [ShowInInspector] [LabelText("正在FastMove")][BoxGroup("状态")]
    public bool IsFastMoving => fastMoveCount > 0;
    private int fastMoveCount = 0;
    [ShowInInspector] [LabelText("攻击中")][BoxGroup("状态")]
    public bool IsAttacking => isAttacking;
    private bool isAttacking;

    public bool IsBigSkill => isBigSkill;
    private bool isBigSkill;
    
    public bool IsDmging => roleAttack != null && roleAttack.IsDmging;
    

    

    #endregion

    #region 属性

    public float MinMoveSpeed => roleMove.MinMoveSpeed;
    public float MaxMoveSpeed => roleMove.MaxMoveSpeed;
    public float CurrentMoveSpeed => roleMove.CurrentMoveSpeed;
    public float InjuryMultiplier => roleHealth.InjuryMultiplier;
    public float TreatMultiplier => roleHealth.HpTreatMultiplier;

    public float MaxHp => roleHealth.MaxHp;

    public float OriginalMaxHp => roleHealth.OriginalMaxHp;
    public float CurrentHp => roleHealth.CurrentHp;
    public int AttackPower => roleAttack.AttackPower;
    public float MagicPower => roleAttack.MagicPower;
    public float AttackSpeed => roleAttack.AttackSpeed;
    public float OriginalAttackSpeed
    {
        get
        {
            if (roleAttack == null)
            {
                return 0.1f;
            }
            return roleAttack.OriginalAttackSpeed;
        }
    }

    public int OriginalAttackPower => roleAttack.OriginalAttackPower;


    public float MaxPower => rolePower.MaxPower;
    public float CurrentPower => rolePower.CurrentPower;
    public float RollNeedPower => rolePower.RollNeedPower;
    public float MaxSkillPower => rolePower.MaxSkillPower;
    public float CurrentSkillPower => rolePower.CurrentSkillPower;
    public float SkillCoolingScale => skillCoolingScale;
    private float skillCoolingScale = 1;
    

    #endregion

    #region 属性加成
    //属性加成
    private Dictionary<string, AttributeBonus> attributeBonus = new Dictionary<string, AttributeBonus>();
    private Dictionary<AttributeType, List<AttributeBonus>> attributeBonusFind = new Dictionary<AttributeType, List<AttributeBonus>>();

    public void AddAttributeBonus(AttributeBonus bonus)
    {
        if (attributeBonus.ContainsKey(bonus.GUID))
        {
            return;
            attributeBonus[bonus.GUID] = bonus;
        }
        else
        {
            attributeBonus.Add(bonus.GUID, bonus);
        }

        if (attributeBonusFind.ContainsKey(bonus.Type))
        {
            attributeBonusFind[bonus.Type].Add(bonus);
        }
        else
        {
            attributeBonusFind.Add(bonus.Type,new List<AttributeBonus>());
            attributeBonusFind[bonus.Type].Add(bonus);
        }

        if(bonus.Type==AttributeType.MaxHp)
        {
            roleHealth.UpdateCurrentHp();
        }
    }

    public void RemoveAttributeBonus(AttributeBonus bonus)
    {
        attributeBonus.Remove(bonus.GUID);
        if (attributeBonusFind.ContainsKey(bonus.Type) && attributeBonusFind[bonus.Type].Contains(bonus))
        {
            attributeBonusFind[bonus.Type].Remove(bonus);
        }
        if(bonus.Type==AttributeType.MaxHp)
        {
            roleHealth.UpdateCurrentHp();
        }
    }

    public float GetAttributeBonusValue(AttributeType attributetype)
    {
        if (!attributeBonusFind.ContainsKey(attributetype))
        {
            return 0;
        }
        
        float value = 0;
        for (int i = 0; i < attributeBonusFind[attributetype].Count; i++)
        {
            value += attributeBonusFind[attributetype][i].Value;
        }
        return value;
    }
    //获取外围属性加成
    public float GetPeripheryAttributeBonusValue(AttributeType attributetype)
    {
        if (!attributeBonusFind.ContainsKey(attributetype))
        {
            return 0;
        }
        
        float value = 0;
        for (int i = 0; i < attributeBonusFind[attributetype].Count; i++)
        {
            if (attributeBonusFind[attributetype][i].SourceType != SourceType.Periphery)
                continue;
            
            value += attributeBonusFind[attributetype][i].Value;
        }
        return value;
    }
    
    //属性加成 END
    #endregion

    #region 属性修改

    public void MultiplySkillCoolingScale(float value)
    {
        skillCoolingScale *= value;
        if (skillCoolingScale < 0f)
        {
            Debug.LogError("技能冷却缩放为负数！！");
        }
    }

    public void ExceptSkillCoolingScale(float value)
    {
        skillCoolingScale /= value;
        if (skillCoolingScale < 0f)
        {
            Debug.LogError("技能冷却缩放为负数！！");
        }
    }

    //无敌状态
    public void SetGod(bool god)
    {
        roleHealth.SetGod(god);
        
        EventManager.Inst.DistributeEvent(EventName.GodStateChange,TemporaryId);
    }
    //闪避状态
    public void SetDodge(bool isdodge)
    {
        roleHealth.SetDodge(isdodge);
    }
    //受伤比例
    public void SetInjuryMultiplier(float value)
    {
        roleHealth.SetInjuryMultiplier(value);
    }
    public void SetUnInjuryMultiplier(float value)
    {
        roleHealth.SetUnInjuryMultiplier(value);
    }

    //移动加速度
    public void SetAcceleration(bool isUseAccel, float startAccel, float stopAccel)
    {
        roleMove.SetAcceleration(isUseAccel, startAccel, stopAccel);
    }
    public void SetMultiplyMoveSpeed(float value)
    {
        roleMove.SetMultiplyMoveSpeed(value);
    }
    public void SetExceptMoveSpeed(float value)
    {
        roleMove.SetExceptMoveSpeed(value);
    }
    //溅射伤害
    public void SetSplashDamage(float value)
    {
        (roleAttack as PlayerAttack).SetSplashDamage(value);
    }

    private float lastFreezeTime;
    private float freezeInterval = 8;
    public void SetFreeze(bool value)
    {
        
        if (value && Time.time - lastFreezeTime < freezeInterval)
            return;
        
        if (value)
            lastFreezeTime = Time.time;
        
        if (value)
            isFreeze++;
        else
            isFreeze--;

        if (isFreeze < 0)
            isFreeze = 0;

        
        if (IsFreeze)
        {
            Animator.speed = 0;
            InputMove(new Vector2(0, 0));
            Rigidbody.velocity = Vector3.zero;
            
            if (IsFastMoving)
                StopFastMove();

            if (IsRolling)
                SetIsRoll(false);

            if (isAttacking)
                SetIsAttacking(false);
        }
        else
        {
            Animator.speed = 1;
        }
        
        
        if (!IsAcceptInput)
            SetAcceptInput(true);
    }
    
    public void SetIsMoving(bool moving)
    {
        IsMoving = moving;
    }

    public void SetIsRoll(bool roll)
    {
        isRolling = roll;
    }

    public void SetIsAttacking(bool isAttack)
    {
        isAttacking = isAttack;
    }

    public void SetIsBigSkill(bool b)
    {
        isBigSkill = b;
    }
    
    public void SetDizziness(bool isDizziness)
    {
        if (roleHealth.IsGod && !IsDizziness)
            return;
        
        if (isDizziness)
        {
            this.isDizziness++;
        }
        else
        {
            this.isDizziness--;
        }
        
        if (IsDizziness)
        {
            if (IsFastMoving)
            {
                StopFastMove();
            }

            if (IsRolling)
            {
                SetIsRoll(false);
            }

            if (isAttacking)
            {
                SetIsAttacking(false);
            }

            roleAttack.ReAttack();
        }
        
        if (!IsAcceptInput)
        {
            SetAcceptInput(true);
        }
        
    }
    
    
    public void AddPower(float value)
    {
        rolePower.AddPower(value);
    }

    public void AddSkillPower(float value)
    {
        rolePower.AddSkillPower(value);
    }
    
    public void FreezeSkillPower()
    {
        rolePower.FreezeSkillPower();
    }
    public void UnFreezeSkillPower()
    {
        rolePower.UnFreezeSkillPower();
    }

    public void SetAcceptInput(bool isOk)
    {
        return;
        // if (isOk)
        // {
        //     isAcceptInput++;
        // }
        // else
        // {
        //     isAcceptInput--;
        // }
        // Debug.LogError(isAcceptInput);
    }
    
    #endregion
    
    #region 初始化相关

    private void Start()
    {
        Init();
    }

    protected bool isInit = false;
    public virtual void Init()
    {
        if (isInit)
        {
            return;
        }
        isInit = true;
        SetTemporaryId(GuidTools.GetGUID());
        roleItemController = GetComponent<RoleItemController>();
        roleBuffController = GetComponent<RoleBuffController>();
        if (roleItemController == null)
        {
            roleItemController = gameObject.AddComponent<RoleItemController>();
        }
        if (roleBuffController == null)
        {
            roleBuffController = gameObject.AddComponent<RoleBuffController>();
        }
        
    }
    protected void SetTemporaryId(string id)
    {
        TemporaryId = id;
    }

    public void SetUniqueID(int id)
    {
        UniqueID = id;
    }
    public virtual void InitAttacker(int attackPower,float magic)
    {
        if (roleAttack == null)
        {
            Debug.LogError("#Role# RoleAttacker为空");
            return;
        }
        roleAttack.Init(attackPower,magic);
    }
    public virtual void InitMaxHp(int maxHp)
    {
        if (roleHealth == null)
        {
            Debug.LogError("#Role# RoleHealth为空");
            return;
        }
        roleHealth.Init(maxHp);
    }

    #endregion

    #region Input相关
    public void InputMove(Vector2 value)
    {
        if (roleMove == null)
        {
            return;
        }

        if (value!=Vector2.zero)
        {
            isWaitRoll = false;
        }

        if (!IsAcceptInput || !IsCanMove)
        {
            value = Vector2.zero;
        }
        roleMove.InputMoveDir(value);
    }
    //等待翻滚 （当能量不足时触发了翻滚，其他动作会打断
    private bool isWaitRoll = false;
    //等待翻滚方向
    private Vector2 waitRollDir;
    [LabelText("无法翻滚提示音")]
    public AudioClip cantRollSound;
    public virtual void InputRoll(Vector2 dir)
    {
        // if (rolePower!=null && !rolePower.IsCanRoll)
        // {
        //     isWaitRoll = true;
        //     waitRollDir = dir;
        //     if (CurrentPower < RollNeedPower)
        //     {
        //         EventManager.Inst.DistributeEvent(EventName.NeedPower);
        //     }
        //     if(cantRollSound)
        //     {
        //         AudioManager.Inst.PlaySource(cantRollSound);
        //     }
        //     return;
        // }
        if (!IsCanRoll)
        {
            EventManager.Inst.DistributeEvent(EventName.RollCool);
            return;
        }
        if (roleRoll == null)
        {
            return;
        }

        roleRoll.InputRoll(dir.normalized);
        
    }

    // public Action<bool> OnInputTouch = delegate { };
    // public void InputTouch(bool isDown)
    // {
    //     OnInputTouch.Invoke(isDown);
    // }

    public virtual void InputAttack(int attackState = 0)
    {
        if (!IsAcceptInput)
        {
            return;
        }
        
        if (roleAttack == null)
        {
            return;
        }

        if (InteractManager.Inst.IsCanInteract())
        {
            return;
        }
        isWaitRoll = false;
        roleAttack.AttackFunc(attackState);
    }

    public void StopAttack()
    {
        if (roleAttack == null)
        {
            return;
        }
        roleAttack.StopAttack();
    }
    public void BackMoveAnim()
    {
        if (roleAttack == null)
        {
            return;
        }
        roleAttack.BackMoveAnim();
    }
    
    
    #endregion

    #region 加血扣血

    public void HpTreatment(TreatmentData value)
    {
        if (roleHealth == null)
        {
            return;
        }
        roleHealth.Treatment(value);
    }

    public void HpInjured(DamageInfo value)
    {
        roleHealth.Injured(value);
    }

    //根据最大血量百分比 设置当前血量
    public void SetCurrentHpOfMaxHpPercentage(float value)
    {
        roleHealth.SetCurrentHpOfMaxHpPercentage(value);
    } 
    //直接设置血量
    public void SetCurrentHp(float value)
    {
        roleHealth.SetCurrentHp(value);
    } 
    #endregion

    #region 死亡

    //TODO 应该放在Enemy中
    //死亡之后加个位移试试看
    public virtual void OnDeadEvent()
    {

        //StopFastMove();

        //Vector3 PlayerDir = BattleManager.Inst.CurrentPlayer.transform.position - transform.position;
        //FastMove(0.1f,30f,-PlayerDir.normalized,null);
        StartCoroutine(onDead());
    }

    protected virtual IEnumerator onDead()
    {
        // EventManager.Inst.DistributeEvent(EventName.OnRoleDead, TemporaryId);
        BehaviorTree bt = GetComponent<BehaviorTree>();
        if (bt != null)
        {
            bt.enabled = false;
        }
        //打开来
        //Collider col = GetComponent<Collider>();
        //if (col != null)
        //{
        //    col.enabled = false;
        //}
        Rigidbody.velocity = Vector3.zero;
        yield return null;
        //BattleManager.Inst.RoleUnRegistered(this);
        yield return new WaitForEndOfFrame();

        yield return new WaitForSeconds(4);
        //改变了位置
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
        //transform.DOMoveY(-0.5f, 4);
        //Destroy(gameObject,5);
    }
    
    #endregion

    #region 生命周期

    protected virtual void Awake()
    {
        EventManager.Inst.AddAnimatorEvent(AnimEvent);
        EventManager.Inst.AddEvent(EventName.EnterNextRoom,OnNextRoom);
    }
    protected virtual void Update()
    {
        if (isWaitRoll && IsCanRoll)
        {
            isWaitRoll = false;
            InputRoll(waitRollDir);
        }

        if (IsFreeze && !IsDie)
        {
            Animator.speed = 0;
        }

        if (IsDie)
        {
            Animator.speed = 1;
        }
    }
    protected virtual void OnDestroy()
    {
        EventManager.Inst.RemoveAnimatorEvent(AnimEvent);
        EventManager.Inst.RemoveEvent(EventName.EnterNextRoom,OnNextRoom);
        BattleManager.Inst.RoleUnRegistered(this);
    }
    
    private void OnNextRoom(string arg1, object arg2)
    {
        if (IsDie)
        {
            Destroy(gameObject);
        }
    }

    private void AnimEvent(GameObject go, string eventName)
    {
        if (go != Animator.gameObject)
        {
            return;
        }

        OnSelfAnimEvent(eventName);
    }

    protected virtual void OnSelfAnimEvent(string eventName)
    {
        // if (eventName.Contains(AnimatorEventName.EndInput))
        // {
        //     SetAcceptInput(false);
        // }
        // if (eventName.Contains(AnimatorEventName.StartInput))
        // {
        //     SetAcceptInput(true);
        // }
        
        if (eventName.Contains(AnimatorEventName.AcceptInterruptionStart))
        {
            roleHealth.SetIsAcceptInterruption(true);
        }
        if (eventName.Contains(AnimatorEventName.AcceptInterruptionEnd))
        {
            roleHealth.SetIsAcceptInterruption(false);
        }
    }
    

    #endregion
    
    #region fastMove

    private Action fastMoveCallBack;
    private Coroutine fastMoveCor;
    private bool stopFastMove = false;
    public void StopFastMove()
    {
        if (fastMoveCor == null)
        {
            return;
        }
        if (!IsFastMoving)
        {
            return;
        }

        if (fastMoveCallBack!=null)
        {
            fastMoveCallBack.Invoke();
            fastMoveCallBack = null;
        }
        StopCoroutine(fastMoveCor);
        fastMoveCount=0;
        stopFastMove = true;
        fastMoveCor = null;
        Rigidbody.velocity = Vector3.zero;
    }
    public void FastMove(float moveTime, float moveSpeed, Vector3 moveDir, Action callBack, AnimationCurve ac = null)
    {
        StopFastMove();
        stopFastMove = false;
        fastMoveCor = StartCoroutine(fastMoveTo(moveTime, moveSpeed, moveDir, callBack, ac));
    }
    public void DelayFastMove(float DelayTime,float moveTime, float moveSpeed, Vector3 moveDir, Action callBack, AnimationCurve ac = null)
    {
        StartCoroutine(DelayFastMoveIE(DelayTime,moveTime,moveSpeed,moveDir,callBack,ac));
    }
    public IEnumerator DelayFastMoveIE(float DelayTime, float moveTime, float moveSpeed, Vector3 moveDir, Action callBack, AnimationCurve ac = null)
    {
        yield return new WaitForSeconds(DelayTime);
        StopFastMove();
        FastMove(moveTime,moveSpeed,moveDir,callBack,ac);
    }
    private IEnumerator fastMoveTo(float moveTime, float MoveSpeed, Vector3 moveDir, Action callBack, AnimationCurve ac = null)
    {
        //yield return new WaitForEndOfFrame();
        fastMoveCount++;
        fastMoveCallBack = callBack;
        float timer = 0;
        Vector3 targetV3 = Vector3.zero;
        while (timer < moveTime && !stopFastMove)
        {
            if (ac != null)
            {
                float value = timer / moveTime;
                float acSpeed = ac.Evaluate(value) * MoveSpeed;
                targetV3 = moveDir.normalized * acSpeed;
            }
            else
            {
                targetV3 = moveDir.normalized * MoveSpeed;
            }
            if (!IsFreeze)
            {
                Rigidbody.velocity = targetV3;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        Rigidbody.velocity = Vector3.zero;

        fastMoveCount--;

        if (callBack != null)
        {
            callBack();
        }
        fastMoveCallBack = null;
        fastMoveCor = null;
    }

    #endregion

    #region 特殊逻辑

    public Action<Collision> OnCollisionEnterEvent;

    public void OnCollisionEnter(Collision other)
    {
        if (OnCollisionEnterEvent == null)
        {
            return;
        }
        OnCollisionEnterEvent.Invoke(other);
    }

    #endregion

    public Dictionary<string,int> Tags = new Dictionary<string,int>();

    public void AddTag(string tag)
    {
        if (!Tags.ContainsKey(tag))
            Tags.Add(tag,0);
        Tags[tag]++;
        
        EventManager.Inst.DistributeEvent(EventName.OnRoleTagChange);
    }

    public void RemoveTag(string tag)
    {
        Tags.Remove(tag);
        EventManager.Inst.DistributeEvent(EventName.OnRoleTagChange);
    }

    public int GetTagCount(string tag)
    {
        if (!Tags.ContainsKey(tag))
            return 0;

        return Tags[tag];
    }

    protected List<GameObject> _floorList = new List<GameObject>();
    protected bool _isExitFloor => _floorList.Count == 0;
    protected float _exitFloorTimer = 0;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
            _floorList.Add(other.gameObject);
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
            _floorList.Remove(other.gameObject);
    }
    
    
}



#region 属性加成
public enum AttributeType
{

    AttackPower = 0,        //攻击力
    AttackSpeed = 1,
    CriticalProbability = 2,     //暴击概率
    TargetFullHpCriticalProbability=3,//目标满血暴击概率
    CriticalMultiplier=4, //暴击倍率
    
    
    AttackDistance=5,
    MaxPower=6,//体力
    AttackPowerRecovery=7,//体力
    PowerRecovery=8,//体力
    
    MaxHp=9,

    MoveSpeed=10,
    TurningAngSpeed=11,//旋转速度
    HpTreatMultiplier=12,
    InjuryMultiplier=13,//受伤倍率

    AccumulateTime=14,//蓄力时间
    AddGoldMagnification=15,//金钱获取比例
    AntiDefenseLevel=16,//破防等级
    DefenseLevel=17,//防御等级
    MagicPower=18,//法术强度
    DodgeProbability=19,//闪避概率
}

public enum SourceType
{
    Periphery = 1,//外围系统，周边
    Battle = 2,//局内战斗
}
//属性加成类
public class AttributeBonus
{
    public AttributeBonus()
    {
        GUID = GuidTools.GetGUID();
    }
    public string GUID { get; private set; }
    public AttributeType Type;
    public SourceType SourceType = SourceType.Battle;
    public float Value;
}

#endregion


