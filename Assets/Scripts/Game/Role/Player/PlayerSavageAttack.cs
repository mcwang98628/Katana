using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerSavageAttack : PlayerAttack
{
    public bool IsAccepting { get; private set; }
    public bool IsCounterattacking { get; private set; }
    public ParticleSystem HealPartilces;
    protected override void AnimEvent(GameObject go, string eventName)
    {
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
        base.AnimEvent(go,eventName);
        if (eventName.Contains("StartAccept"))
        {
            IsAccepting = true;
        }
        else if (eventName.Contains("EndAccept"))
        {
            IsAccepting = false;
        }
        else if (eventName.Contains("StartCounterattack"))//开始反击
        {
            IsCounterattacking = true;
            accumulationAttackPower.Value = (1-attackPowerPercentage) * -roleController.OriginalAttackPower;
        }
        else if (eventName.Contains("EndCounterattack"))
        {
            IsCounterattacking = false;
            accumulationAttackPower.Value = 0;
            clearValue();
        }
    }
    protected override void Awake()
    {
        base.Awake();
        EventManager.Inst.AddEvent(EventName.OnRoleInjured,OnPlayerInjured);
        // roleController.OnInputTouch += InputTouch;
        accumulationAttackPower = new AttributeBonus();
        accumulationAttackPower.Type = AttributeType.AttackPower;
        accumulationAttackPower.Value = 0;
        roleController.AddAttributeBonus(accumulationAttackPower);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.Inst.RemoveEvent(EventName.OnRoleInjured,OnPlayerInjured);
        roleController.RemoveAttributeBonus(accumulationAttackPower);
        // roleController.OnInputTouch -= InputTouch;
    }

    private bool isDown;
    private float downTime;//按下时间
    [SerializeField]//最大蓄力时间
    private float accumulationTime = 2;
    //蓄力攻击力增幅百分比
    private float attackPowerPercentage = 1;
    private AttributeBonus accumulationAttackPower;
    private void InputTouch(bool obj)
    {
        if (!isDown && obj)
        {
            downTime = Time.time;
        }
        else if(isDown && !obj)
        {
            attackPowerPercentage = (Time.time - downTime) / accumulationTime;
            if (attackPowerPercentage>1)
            {
                attackPowerPercentage = 1;
            }
        }
        isDown = obj;
    }


    protected override void Update()
    {
        base.Update();
        Arrest();
        
        
        roleController.Animator.SetBool("Hold",isDown);
    }

    [SerializeField][BoxGroup("野蛮人")]
    private Transform ArrestPoint;
    public RoleController ArrestEnemy { get; private set; }
    void Arrest()
    {
        if (ArrestEnemy != null && ArrestEnemy.IsDie)
        {
            UnArrestEnemy();
        }
        
        if (!roleController.IsFastMoving && roleController.CurrentMoveSpeed>=6 && ArrestEnemy==null)
        {
            var role = BattleTool.FindNearestEnemy(transform);
            if (role!=null)
            {
                if (role.roleTeamType == RoleTeamType.Enemy)
                {
                    var dir = role.transform.position - roleController.transform.position;
                    if (dir.magnitude < 1.8f)
                    {
                        float ang = Vector3.Angle(roleController.Animator.transform.forward, dir);
                        if (ang <= 20)
                        {
                            ArrestEnemy = role;
                            ArrestEnemy.OnCollisionEnterEvent += OnEnemyCollisionEnterEvent;
                        }   
                    }
                }
            }
        }

        if (ArrestEnemy != null)
        {
            if (roleController.CurrentMoveSpeed <6)
            {
                UnArrestEnemy();
            }
            else
            {
                ArrestEnemy.SetDizziness(true);
                ArrestEnemy.StopAttack();
                ArrestEnemy.transform.position = ArrestPoint.position;
            }
        }
    }

    void UnArrestEnemy()
    {
        ArrestEnemy.SetDizziness(false);
        ArrestEnemy.StopAttack();
        // ReSharper disable once DelegateSubtraction
        ArrestEnemy.OnCollisionEnterEvent -= OnEnemyCollisionEnterEvent;
        ArrestEnemy = null;
        // hitObjList.Clear();
        AttackFunc();
    }
    
    // List<GameObject> hitObjList = new List<GameObject>();
    private void OnEnemyCollisionEnterEvent(Collision obj)
    {
        if (ArrestEnemy == null)
        {
            return;
        }

        // if (hitObjList.Contains(obj.gameObject))
        // {
        //     return;
        // }
        
        if (obj.gameObject.layer == LayerMask.NameToLayer("Player")||
            obj.gameObject.layer == LayerMask.NameToLayer("PlayerRoll"))
        {
            return;
        }

        if (obj.gameObject.layer == LayerMask.NameToLayer("Enemy")||
            obj.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            foreach (BreakableObj breakableObj in BreakableObjManager.Inst.BreakableObjList)
            {
                if ((breakableObj.transform.position - obj.transform.position).magnitude < 1f)
                {
                    breakableObj.BreakObj();
                }
            }
            
            // hitObjList.Add(obj.gameObject);
            
            DamageInfo dmg = new DamageInfo(ArrestEnemy.TemporaryId,roleController.AttackPower*0.5f,roleController,roleController.transform.position,DmgType.Physical);
            EventManager.Inst.DistributeEvent(EventName.OnPlayerAttackHitEnemy,dmg);
            ArrestEnemy.HpInjured(dmg);

            if (obj.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                var role = obj.gameObject.GetComponent<RoleController>();
                dmg = new DamageInfo(role.TemporaryId,roleController.AttackPower*0.5f,roleController,roleController.transform.position,DmgType.Physical,true,true,0.1f,15);
                role.HpInjured(dmg);
            }
        }
    }


    private int playerInjuredTimes;
    private float dmgValue;
    private int playerTreatmentTimes;
    // private float treatmentValue;
    private void OnPlayerInjured(string arg1, object arg2)
    {
        var data = (RoleInjuredInfo) arg2;
        if (data.RoleId != BattleManager.Inst.CurrentPlayer.TemporaryId)
        {
            if (IsCounterattacking && 
                playerTreatmentTimes < playerInjuredTimes)
            {
                float value = dmgValue / playerInjuredTimes;
                TreatmentData tdata = new TreatmentData(value,roleController.TemporaryId);
                playerTreatmentTimes++;
                HealPartilces.Play();
                roleController.HpTreatment(tdata);
            }
        }
        else
        {
            if (IsAccepting)
            {
                dmgValue += data.Dmg.DmgValue;
                playerInjuredTimes++;
                roleController.Animator.SetTrigger("AccepTrigger");
            }
        }

    }

    void clearValue()
    {
        playerInjuredTimes = 0;
        dmgValue = 0;
        playerTreatmentTimes = 0;
        // treatmentValue = 0;
    }
    
    
    // public override void Attack_B_Func()
    // {
    //     // if (roleController.MaxSkillPower != roleController.CurrentSkillPower)
    //     // {
    //     //     EventManager.Inst.DistributeEvent(EventName.NeedPower);
    //     //     return;
    //     // }
    //     // roleController.AddSkillPower(-roleController.MaxSkillPower);
    //     //不需要消耗能量
    //     
    //     
    //     roleController.Animator.SetTrigger(Attack_B);
    // }
}
