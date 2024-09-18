using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

public class DmgBuffOnTouch : MonoBehaviour
{

    public bool SetAttackPowerAsDamage = true;
    public DmgTarget DamageTarget;
    public bool SelfDamage = false;
    [Header("Base")]
    public float interval;//伤害和buff间隔时间
    public float duration;//持续时间
    public float radius;//半径

    [Header("DamageInfo")]
    public DamageInfo Damage;

    public bool useRatio;
    [ShowIf("useRatio")]
    public float damageValueRatio;
    
    public float DamageDelay = 0;

    [Header("Buff")]
    public BuffLifeCycle LifeCycle;
    public BuffScriptableObject buffObj;

    [Header("FeedBack")]
    public FeedBackObject HitFeedBack;
    
    protected RoleController owner;

    //key：被添加过buff的roleId，value 添加时间
    protected Dictionary<string, float> Targets = new Dictionary<string, float>();

    protected bool isInit = false;
    public virtual void Init(RoleController role, float AttackPower = -1)
    {
        //Debug.Log("Init:"+role);
        owner = role;
        timer = Time.time;
        if (SetAttackPowerAsDamage)
        {
            Damage.DmgValue = AttackPower >= 0 ? AttackPower : !useRatio? owner.AttackPower:owner.AttackPower*damageValueRatio;
        }
        else
        {
            Damage.DmgValue = AttackPower >= 0 ? AttackPower : Damage.DmgValue;
        }
        gameObject.SetActive(true);
        _damageTypeInit = DamageTypeInit.Value;
        StartCoroutine(DelayInitDamage(DamageDelay));
    }

    enum DamageTypeInit
    {
        Ratio,
        Value,
    }

    private DamageTypeInit _damageTypeInit;
    private float damageRatio;
    public virtual void InitRatio(RoleController role, float Ratio)
    {
        //Debug.Log("Init:"+role);
        isInit = true;
        owner = role;
        timer = Time.time;
        damageRatio = Ratio;
        gameObject.SetActive(true);
        _damageTypeInit = DamageTypeInit.Ratio;
        SelfDamage = true;
    }
    
    public void WZYInit()
    {
        isInit = true;
        gameObject.SetActive(false);
    }
    IEnumerator DelayInitDamage(float time)
    {
        yield return new WaitForSeconds(time);
        isInit = true;
    }

    protected float timer = 0;
    protected virtual void Update()
    {
        if (!isInit)
        {
            return;
        }

        TakeEffect();
        if (duration>=999)
        {
            return;
        }
        if (Time.time - timer > duration && duration > 0)
        {
            DestroyObj();
        }
    }
    public virtual void DestroyObj()
    {
        ParticleSystem particle = GetComponent<ParticleSystem>();
        if (particle == null)
            particle = GetComponentInChildren<ParticleSystem>();
        if (particle != null)
            particle.Stop();
        isInit = false;
        Destroy(gameObject, 1f);
    }
    public virtual void TakeEffect()
    {
        initTargets();
        var targets = calculationTouch();
        doDamageAndBuff(targets);
    }

    protected Dictionary<string, RoleController> targets = null;

    //初始化 目标集合
    protected void initTargets()
    {
        targets = null;
        if (targets == null)
        {
            switch (DamageTarget)
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
        }
    }
    //计算触碰
    private List<RoleController> calculationTouch()
    {
        //Debug.Log("CalculateTouch");
        List<RoleController> roles = new List<RoleController>();
        //Debug.Log(targets.Count);
        foreach (KeyValuePair<string, RoleController> roleController in targets)
        {
            if (roleController.Value == null)
            {
                continue;
            }
            if (roleController.Value == owner && (!SelfDamage))
            {
                continue;
            }
            Vector3 v3 = roleController.Value.transform.position - transform.position;
            v3.y = 0;
            //Debug.Log(roleController.ToString()+ distance);
            if (v3.magnitude > radius)
            {
                continue;
            }

            roles.Add(roleController.Value);
        }

        return roles;
    }
    public virtual void OtherEffect(RoleController Target)
    {

    }
    protected virtual void doDamageAndBuff(List<RoleController> targets)
    {
        if (!isInit)
            return;
        for (int i = 0; i < targets.Count; i++)
        {
            //记录和判断 目标触发时间是否在限制间隔内
            if (Targets.ContainsKey(targets[i].TemporaryId))
            {
                if (Time.time - Targets[targets[i].TemporaryId] >= interval)
                {
                    Targets[targets[i].TemporaryId] = Time.time;
                }
                else
                {
                    continue;
                }
            }
            else
            {
                Targets.Add(targets[i].TemporaryId, Time.time);
            }

            if (buffObj != null)
            {
                var buff = DataManager.Inst.ParsingBuff(buffObj, LifeCycle);
                targets[i].roleBuffController.AddBuff(buff, owner);
            }
            if (Damage != null)
            {
                if (Damage.DmgValue > 0)
                {
                    float value = Damage.DmgValue;
                    bool isCrit = false;
                    if (owner is PlayerController playerController &&
                        playerController.roleAttack is PlayerAttack playerAttack &&
                        playerController.AOECrit)
                    {
                        //目标满血暴击率
                        float fullHpProba = targets[i].CurrentHp == targets[i].MaxHp
                                ? playerAttack.TargetFullHpCriticalProbability
                                : 0;
                        if (Random.Range(0, 1.0f) < playerAttack.CriticalProbability + fullHpProba)
                        {
                            value *= playerAttack.CriticalMultiplier;
                            isCrit = true;
                        }

                        value *= playerAttack.MagicPower;
                    }

                    if (_damageTypeInit== DamageTypeInit.Ratio)
                    {
                        value = targets[i].MaxHp * damageRatio;
                    }
                    DamageInfo dmg = new DamageInfo(targets[i].TemporaryId, value, owner, new Vector3(transform.position.x, 0, transform.position.z), Damage.DmgType, Damage.Interruption, Damage.IsUseMove, Damage.MoveTime, Damage.MoveSpeed, Damage.IsRemotely, Damage.RemotelyObject, null, null, isCrit);
                    dmg.SetFeedBack(HitFeedBack);
                    if(this is Projectile)
                    {
                        dmg.AttackPoint= new Vector3(owner.transform.position.x, 0, owner.transform.position.z);
                    }
                    OtherEffect(targets[i]);
                    targets[i].HpInjured(dmg);
                    if (owner.IsPlayer && dmg.DmgValue > 0)
                    {
                        EventManager.Inst.DistributeEvent(EventName.OnDmgBuffOnTouchHitRole, dmg);
                    }
                }
            }

        }
    }
}
