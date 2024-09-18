using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AOETouch : MonoBehaviour,IBattleObject
{
    public Guid TemporaryId { get; protected set; }
    public Transform ObjectTransform { get; protected set; }
    public BattleObjectType BattleObjectType => _battleObjectType;
    
    [BoxGroup("BattleObject",true,true)]
    [SerializeField] 
    protected BattleObjectType _battleObjectType = BattleObjectType.None;


    [BoxGroup("Base",true,true)]
    [SerializeField] [LabelText("初始延迟时间")]
    private float initDelay;
    [BoxGroup("Base",true,true)]
    [SerializeField] [LabelText("触发间隔")]
    private float interval;
    [BoxGroup("Base",true,true)]
    [SerializeField] [LabelText("持续时间")]
    private float duration;
    [BoxGroup("Base",true,true)]
    [SerializeField] [LabelText("半径")]
    protected float radius;
    
    [BoxGroup("Damage",true,true)]
    [SerializeField] [LabelText("使用Dmg")]
    private bool useDmg;
    [InfoBox("-1 为Role自身攻击力，0为不造成伤害")]
    [BoxGroup("Damage",true,true)]
    [SerializeField] [LabelText("攻击力")] [ShowIf("useDmg")]
    private float attackPower;
    [BoxGroup("Damage",true,true)]
    [SerializeField] [LabelText("伤害类型")] [ShowIf("useDmg")]
    private AttackDamageType damageType;
    [BoxGroup("Damage",true,true)]
    [SerializeField] [LabelText("元素类型")] [ShowIf("useDmg")]
    private DamageElementType damageElementType; 
    [BoxGroup("Damage",true,true)]
    [SerializeField] [LabelText("伤害目标")] [ShowIf("useDmg")]
    private DmgTarget damageTarget;
    [BoxGroup("Damage",true,true)]
    [SerializeField] [LabelText("打断")]  [ShowIf("useDmg")]
    private bool interruption;
    [BoxGroup("Damage",true,true)]
    [SerializeField] [LabelText("击退")] [ShowIf("useDmg")]
    private bool isUseMove;
    [BoxGroup("Damage",true,true)]
    [SerializeField] [LabelText("击退时长")] [ShowIf("isUseMove")] [ShowIf("useDmg")]
    private float moveTime;
    [BoxGroup("Damage",true,true)]
    [SerializeField] [LabelText("击退速度")] [ShowIf("isUseMove")] [ShowIf("useDmg")]
    private float moveSpeed;
    [BoxGroup("Damage",true,true)]
    [SerializeField] [LabelText("是远程单位")] [ShowIf("useDmg")]
    private bool isRemotely;

    [BoxGroup("Buff",true,true)] 
    [SerializeField] [LabelText("使用Buff")]
    private bool useBuff;
    [BoxGroup("Buff",true,true)]
    [SerializeField] [LabelText("Buff声明周期")] [ShowIf("useBuff")]
    private BuffLifeCycle lifeCycle;
    [BoxGroup("Buff",true,true)]
    [SerializeField] [LabelText("Buff")] [ShowIf("useBuff")]
    private BuffScriptableObject buffObj;
    
    //RuntimeData....................
    protected bool IsInit = false;
    //key：被添加过buff的roleId，value 添加时间
    protected Dictionary<string, float> Targets = new Dictionary<string, float>();
    protected RoleController Owner;
    protected float InitTime;

    protected virtual void Start()
    {
        BattleManager.Inst.BattleObjectRegistered(this);
    }

    protected virtual void OnDestroy()
    {
        BattleManager.Inst.BattleObjectUnRegistered(this);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="role">拥有者</param>
    /// <param name="attackPower">-1 为Role自身攻击力，0为不造成伤害</param>
    public void Init(RoleController role)
    {
        gameObject.SetActive(true);
        TemporaryId = Guid.NewGuid();
        ObjectTransform = transform;
        
        Owner = role;
        InitTime = Time.time;
        StartCoroutine(DelayInitDamage(initDelay));
        
        
    }

    IEnumerator DelayInitDamage(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        IsInit = true;
    }


    protected Dictionary<string, RoleController> GetDmgTargets()
    {
        Dictionary<string, RoleController> targets = null;
        switch (damageTarget)
        {
            case DmgTarget.Player:
                targets = BattleManager.Inst.PlayerTeam;
                break;
            case DmgTarget.Enemy:
                targets = BattleManager.Inst.EnemyTeam;
                break;
            case DmgTarget.All:
                targets = new Dictionary<string, RoleController>();
                foreach (var roleController in BattleManager.Inst.PlayerTeam)
                {
                    targets.Add(roleController.Key, roleController.Value);
                }
                foreach (var roleController in BattleManager.Inst.EnemyTeam)
                {
                    targets.Add(roleController.Key, roleController.Value);
                }
                break;
        }
        return targets;
    }
    
    //计算触碰
    protected List<RoleController> GetTouchTarget()
    {
        List<RoleController> roles = new List<RoleController>();
        //Debug.Log(targets.Count);
        foreach (KeyValuePair<string, RoleController> roleController in GetDmgTargets())
        {
            if (roleController.Value == null)
            {
                continue;
            }
            
            if (roleController.Value.IsDie == true)
            {
                continue;
            }
            
            
            Vector3 v3 = roleController.Value.transform.position - transform.position;
            v3.y=0;
            //Debug.Log(roleController.ToString()+ distance);
            if (v3.magnitude > radius)
            {
                continue;
            }

            roles.Add(roleController.Value);
        }

        return roles;
    }

    //计算目标时间间隔
    protected bool CalculationTargetInterval(RoleController target)
    {
        if (Targets.ContainsKey(target.TemporaryId))
            if (Time.time - Targets[target.TemporaryId] >= interval)
                Targets[target.TemporaryId] = Time.time;
            else
                return false;
        else
            Targets.Add(target.TemporaryId, Time.time);

        return true;
    }

    protected virtual void CalculationDamage(RoleController target)
    {
        float value = 0;
        if (attackPower != 0)
        {
            value = attackPower > 0 ? attackPower : Owner.AttackPower;
        }

        if (value <= 0)
        {
            return;
        }

        DmgType dmgType = DmgType.Physical;
        bool isCrit = false;
        if (Owner is PlayerController playerController && 
            playerController.roleAttack is PlayerAttack playerAttack && 
            playerController.AOECrit)
        {
            //目标满血暴击率
            float fullHpProba = target.CurrentHp == target.MaxHp
                ? playerAttack.TargetFullHpCriticalProbability
                : 0;
            if (Random.Range(0, 1.0f) < playerAttack.CriticalProbability+fullHpProba)
            {
                value *= playerAttack.CriticalMultiplier;
                isCrit = true;
            }
        }
        DamageInfo dmg = new DamageInfo(
            target.TemporaryId,
            value,
            Owner,
            new Vector3(transform.position.x,0,transform.position.z),
            dmgType,
            interruption,
            isUseMove,
            moveTime,
            moveSpeed,
            isRemotely,
            this,
            null,
            null,
            isCrit
            );
        
        target.HpInjured(dmg);
        if (Owner.IsPlayer && dmg.DmgValue>0)
        {
            EventManager.Inst.DistributeEvent(EventName.OnDmgBuffOnTouchHitRole,dmg);
        }
    }
    
    protected virtual void DoDmgAndBuff(List<RoleController> targets)
    {
        if (!IsInit)
            return;
        
        for (int i = 0; i < targets.Count; i++)
        {
            //记录和判断 目标触发时间是否在限制间隔内
            if (!CalculationTargetInterval(targets[i]))
                continue;
            //--------------------------------------
            
            //上buff
            if (useBuff && buffObj != null)
            {
                var buff = DataManager.Inst.ParsingBuff(buffObj,lifeCycle);
                targets[i].roleBuffController.AddBuff(buff, Owner);
            }
            
            if (useDmg)
            {
                CalculationDamage(targets[i]);
            }

        }
    }

    protected virtual void Update()
    {
        if (Time.time - InitTime > duration)
        {
            Destroy(gameObject);
        }
        else
        {
            DoDmgAndBuff(GetTouchTarget());
        }
    }


#if UNITY_EDITOR
    [BoxGroup("Editor",true,true)]
    [SerializeField]
    private bool drawWire;
    
    private void OnDrawGizmos()
    {
        Color oldColor = Gizmos.color;
        Matrix4x4 oldMat = Gizmos.matrix;
        
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        if (drawWire)
        {
            Gizmos.DrawWireSphere(Vector3.zero, radius);
        }
        else
        {
            Gizmos.DrawSphere(Vector3.zero, radius);
        }
        Gizmos.matrix = oldMat;
        Gizmos.color = oldColor;
    }
#endif
}
