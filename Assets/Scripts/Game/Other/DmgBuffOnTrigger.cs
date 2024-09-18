using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class DmgBuffOnTrigger : MonoBehaviour
{
    public DmgTarget DamageTarget;

    [Header("Buff")]
    public BuffScriptableObject buffObj;
    public BuffLifeCycle LifeCycle;
    [Header("Damage")]
    public bool SelfDamage = false;
    public DamageInfo Damage;
    public float Interval = -1;
    protected RoleController owner;
    List<RoleController> damgeRoleList;
    float timer;
    //key：被添加过buff的roleId，value 添加时间

    protected bool isInit = false;

    enum DamageTypeInit
    {
        Ratio,
        Value,
    }

    private DamageTypeInit _damageTypeInit;
    protected float damageRatio;
    public void Init(RoleController role, int AttackPower = -1)
    {
        isInit = true;
        _damageTypeInit = DamageTypeInit.Value;
        owner = role;
        Damage.DmgValue = AttackPower > 0 ? AttackPower : owner.AttackPower;
        Damage.AttackerID = role.UniqueID;
        gameObject.SetActive(true);
    }

    public void Init(RoleController role, float ratio)
    {
        isInit = true;
        owner = role;
        damageRatio = ratio;
        _damageTypeInit = DamageTypeInit.Ratio;
        Damage.AttackerID = role.UniqueID;
        gameObject.SetActive(true);
    }
    public void ActiveTrigger()
    {
        GetComponent<Collider>().enabled = true;
        damgeRoleList = new List<RoleController>();
        timer = Time.time;

    }
    public void CloseTrigger()
    {
        GetComponent<Collider>().enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!isInit || damgeRoleList == null)
            return;            
        RoleController target = other.GetComponent<RoleController>();
        if (target == null)
            return;
        if (target.roleTeamType != RoleTeamType.Player && DamageTarget == DmgTarget.Player)
            return;
        if (target.roleTeamType != RoleTeamType.Enemy && DamageTarget == DmgTarget.Enemy)
            return;
        if (target.roleTeamType != RoleTeamType.EliteEnemy && DamageTarget == DmgTarget.Enemy)
            return;
        if (target.roleTeamType != RoleTeamType.Enemy_Boss && DamageTarget == DmgTarget.Enemy)
            return;
        if (!SelfDamage && target == owner)
            return;

        doDamageAndBuff(target);
    }
    private void OnTriggerStay(Collider other)
    {
        if (!isInit || damgeRoleList == null)
            return;
        RoleController target = other.GetComponent<RoleController>();
        if (target == null)
            return;
        if (target.roleTeamType != RoleTeamType.Player && DamageTarget == DmgTarget.Player)
            return;
        if (target.roleTeamType != RoleTeamType.Enemy && DamageTarget == DmgTarget.Enemy)
            return;
        if (target.roleTeamType != RoleTeamType.EliteEnemy && DamageTarget == DmgTarget.Enemy)
            return;
        if (target.roleTeamType != RoleTeamType.Enemy_Boss && DamageTarget == DmgTarget.Enemy)
            return;
        if (!SelfDamage && target == owner)
            return;

        if (Interval > 0 && timer < Time.time)
        {
            if (damgeRoleList.Contains(target))
                damgeRoleList.Remove(target);
            doDamageAndBuff(target);
        }

    }


    protected void doDamageAndBuff(RoleController target)
    {
        if (damgeRoleList == null)
            return;
        if (damgeRoleList.Contains(target))
            return;
        damgeRoleList.Add(target);
        
        if (Damage != null)
        {
            if (_damageTypeInit==DamageTypeInit.Ratio)
            {
                Damage.DmgValue =target.MaxHp*damageRatio;
            }
            DamageInfo damage = new DamageInfo(
                target.TemporaryId,
                (DamageTarget == DmgTarget.All && target.roleTeamType != RoleTeamType.Player) ? Damage.DmgValue/2f : Damage.DmgValue,
                owner,
                transform.position,
                Damage.DmgType,
                Damage.Interruption,
                Damage.IsUseMove,
                Damage.MoveTime,
                Damage.MoveSpeed,
                Damage.IsRemotely,
                Damage.RemotelyObject,
                buffObj,
                LifeCycle);

            target.HpInjured(damage);
        }

        if (Interval > 0)
        {
            timer = Time.time + Interval;
        }
    }

}
