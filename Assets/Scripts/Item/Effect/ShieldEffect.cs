using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//盾反 
public class ShieldAgainst:ItemEffect
{
    private static readonly int Against = Animator.StringToHash("ShieldAgainst");
    //是否能盾反
    private bool isCanShieldAgainst => !roleController.IsRolling && !roleController.IsAttacking && !roleController.IsDie && roleController.IsAcceptInput;

    //弹反距离
    private float distance = 2.5f;
    private float defenseAng = 120f;

    private FeedBackObject PerfectShieldFeedback;
    public ShieldAgainst(FeedBackObject feedBackObject,float distance)
    {
        PerfectShieldFeedback = feedBackObject;
        this.distance = distance;
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.JoyStatus,OnJoyStatus);
        EventManager.Inst.AddEvent(EventName.OnShieldDefense,OnShieldDefense);
        EventManager.Inst.AddAnimatorEvent(OnAnimEvent);
    }
    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveEvent(EventName.JoyStatus,OnJoyStatus);
        EventManager.Inst.RemoveEvent(EventName.OnShieldDefense,OnShieldDefense);
        EventManager.Inst.RemoveAnimatorEvent(OnAnimEvent);
    }

    //盾反中
    private bool isShieldAgainsting;
    private void OnAnimEvent(GameObject arg1, string eventName)
    {
        if (roleController.Animator.gameObject != arg1)
        {
            return;
        }

        if (eventName == AnimatorEventName.ShieldAgainstStart)
        {
            isShieldAgainsting = true;
        }
        else if (eventName == AnimatorEventName.ShieldAgainstEnd)
        {
            isShieldAgainsting = false;
        }
    }

    public override void Update(RoleItemController rpe)
    {
        base.Update(rpe);
        if (isShieldAgainsting)
        {
            for (int i = 0; i < BattleManager.Inst.BattleThrow.Count; i++)
            {
                var throwObject = BattleManager.Inst.BattleThrow[i];
                var dir = throwObject.transform.position - roleController.transform.position;
                if (dir.magnitude > distance)
                {
                    return;
                }
                var ang = Vector3.Angle(roleController.Animator.transform.forward, dir);
                if (ang > defenseAng/2f)
                {
                    return;
                }

                if (throwObject is Projectile projectile)
                {
                    projectile.Back();
                    EventManager.Inst.DistributeEvent(EventName.OnShieldAgainst);
                }
                else if (throwObject is ThrowProjectile throwProjectile)
                {
                    throwProjectile.Back();
                    EventManager.Inst.DistributeEvent(EventName.OnShieldAgainst);
                }
                
            }
        }
    }

    //道具的弹反
    private void OnShieldDefense(string arg1, object arg2)
    {
        var dmg = (DamageInfo) arg2;
        if (dmg.IsRemotely)
        {
            if (dmg.RemotelyObject is Projectile projectile)
            {
                projectile.Back();
                EventManager.Inst.DistributeEvent(EventName.OnShieldAgainst);
            }
            else if (dmg.RemotelyObject is ThrowProjectile throwProjectile)
            {
                throwProjectile.Back();
                EventManager.Inst.DistributeEvent(EventName.OnShieldAgainst);
            }
            return;
        }
        if (isShieldAgainsting)// && ((PlayerShieldController)roleController).IsHoldingShield)
        {
            roleController.FastMove(0.05f, 10f, roleController.Animator.transform.forward, null);
            FeedbackManager.Inst.UseFeedBack(roleController, PerfectShieldFeedback);
            
            //弹伤敌人
            if (dmg.AttackerRole != null && dmg.AttackerRole != roleController)
            {
                DamageInfo damage = new DamageInfo(dmg.AttackerRole.TemporaryId,dmg.DmgValue, roleController, roleController.transform.position,dmg.DmgType,dmg.Interruption);
                dmg.AttackerRole.HpInjured(damage);
            }
            
            //震退周围的敌人
            List<RoleController> NearEnemies = BattleTool.GetEnemysInDistance(roleController, 2.5f);
            for (int i = 0; i < NearEnemies.Count; i++)
            {
                NearEnemies[i].DelayFastMove(Time.deltaTime, 0.15f, 8f, (NearEnemies[i].Animator.transform.position - roleController.Animator.transform.position).normalized, null);
            }
            
            EventManager.Inst.DistributeEvent(EventName.OnShieldAgainst);
            //完美弹反之后重制时间
            // joyDownTime = Time.time;
        }
    }


    private void OnJoyStatus(string arg1, object arg2)
    {
        if (!isCanShieldAgainst)
        {
            return;
        }
        var joy = (JoyStatusData) arg2;
        if (joy.JoyStatus == UIJoyStatus.OnSlide)
        {
            var v2 = InputManager.Inst.GetCameraDir(joy.SlideDir);
            roleController.Animator.transform.forward = new Vector3(v2.x,0,v2.y);
            roleController.Animator.SetTrigger(Against);
        }
    }
}


//小盾反 ， 格挡就会造成伤害
public class SmallShieldAgainst:ItemEffect
{
    private float value;
    public SmallShieldAgainst(float value)
    {
        this.value = value;
    }
    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.OnShieldDefense,OnShieldDefense);
    }
    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveEvent(EventName.OnShieldDefense,OnShieldDefense);
    }

    private void OnShieldDefense(string arg1, object arg2)
    {
        var dmg = (DamageInfo) arg2;
        //弹伤敌人
        if (dmg.AttackerRole != null && dmg.AttackerRole != roleController && dmg.DmgType == DmgType.Physical)
        {
            DamageInfo damage = new DamageInfo(dmg.AttackerRole.TemporaryId,dmg.DmgValue*value, roleController, roleController.transform.position);
            dmg.AttackerRole.HpInjured(damage);
        }
    }
}

//盾 - 反弹子弹
public class ShieldAgainstBullet : ItemEffect
{
    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        ((PlayerShieldHealth) roleController.roleHealth).SetIsAgainstBullet(true);
    }
}

//能量反击盾 - 放下盾造成AOE伤害
public class PowerShieldAgainst : ItemEffect
{
    private DmgBuffOnTouch AOE;
    private Vector3 Offset;
    private float dmgValue = 0;

    public PowerShieldAgainst(DmgBuffOnTouch aoe,Vector3 offset)
    {
        AOE = aoe;//创建
        Offset = offset;
    }
    
    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.OnShieldDefense,OnShieldDefense);
    }
    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveEvent(EventName.OnShieldDefense,OnShieldDefense);
    }

    private void OnShieldDefense(string arg1, object arg2)
    {
        var dmg = (DamageInfo) arg2;
        dmgValue += dmg.DmgValue;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        if (dmgValue>0)
        {
            //AOE
            var fx = GameObject.Instantiate(AOE, roleController.Animator.transform);
            fx.transform.localPosition = Offset;
            fx.Init(roleController,dmgValue);
            dmgValue = 0;
        }
    }
}

//盾 防御角度和移速
public class ShieldAttributes : ItemEffect
{
    private float turningAngSpeed;
    private float moveSpeed;
    private Mesh mesh1;
    private Mesh mesh2;
    public ShieldAttributes(float ang,float moveSpeed,Mesh mesh1,Mesh mesh2)
    {
        turningAngSpeed = ang;
        this.moveSpeed = moveSpeed;
        this.mesh1 = mesh1;
        this.mesh2 = mesh2;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        ((PlayerShieldHealth)roleController.roleHealth).SetDefenseAng(turningAngSpeed);
        ((PlayerShieldMove)roleController.roleMove).SetShieldMoveSpeed(moveSpeed);
        // roleController.roleNode.Weapon.GetComponent<MeshFilter>().mesh = mesh;
        roleController.roleNode.WeaponMesh1.mesh = mesh1;
        roleController.roleNode.WeaponMesh2.mesh = mesh2;
    }
}

//挥盾攻击
public class ShieldSkill_Against : ItemEffect
{
    private static readonly int SkillAgainstOneTimes = Animator.StringToHash("Skill_Against_OneTimes");

    private int skillLoopTimes;
    private float interval;
    public ShieldSkill_Against(int attackTimes,float skillInterval)
    {
        skillLoopTimes = attackTimes;
        interval = skillInterval;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        roleController.StartCoroutine(skillLoop());
    }

    IEnumerator skillLoop()
    {
        
        for (int i = 0; i < skillLoopTimes; i++)
        {
            roleController.Animator.SetTrigger(SkillAgainstOneTimes);
            yield return new WaitForSeconds(interval);
        }
    }
}

//盾冲最后 攻击
public class ShieldRollAttack : ItemEffect
{
    private static readonly int RollAttack = Animator.StringToHash("ShieldRollAttack");

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        roleController.Animator.SetBool(RollAttack,true);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        roleController.Animator.SetBool(RollAttack,false);
    }
}

public class ShieldDash_SkipPreAction : ItemEffect
{
    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        if (roleController.roleRoll is PlayerShieldDash shield)
        {
            shield.SetDash_SkipPreAction(true);
        }
    }
}
public class ShieldDash_AfterAttack : ItemEffect
{
    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        if (roleController.roleRoll is PlayerShieldDash shield)
        {
            shield.SetDash_AfterAttack(true);
        }
    }
}

public class ShieldDash_God : ItemEffect
{
    private bool setGod = false;
    public override void Update(RoleItemController rpe)
    {
        base.Update(rpe);
        if (roleController.IsRolling && !setGod)
        {
            setGod = true;
            roleController.SetGod(true);
        }
        else if (!roleController.IsRolling && setGod)
        {
            setGod = false;
            roleController.SetGod(false);
        }
        
    }
}