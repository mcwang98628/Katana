using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class ItemEffectEffectData
{
    // [ShowIf("EffectType", EffectType.NewKatanaSkill)]
    // [LabelText("斩杀线")]
    // public float KatanaSkillKillLine;//斩杀线
    [ShowIf("EffectType", EffectType.NewKatanaSkill)]
    [LabelText("击杀恢复能量")]
    public int KatanaSkillKillRecoveryPower;//击杀恢复能量数
    [ShowIf("EffectType", EffectType.NewKatanaSkill)]
    [LabelText("攻击距离")]
    public float KatanaSkillAttackDis;//攻击距离
    [ShowIf("EffectType", EffectType.NewKatanaSkill)]
    [LabelText("攻击角度")]
    public float KatanaSkillAttackAng;//攻击角度
    [ShowIf("EffectType", EffectType.NewKatanaSkill)]
    [LabelText("攻击力百分比")]
    public float KatanaSkillAttackPowerPercentage;//攻击力百分比
    [ShowIf("EffectType", EffectType.NewKatanaSkill)]
    [LabelText("攻击力效果延迟时间")]
    public float KatanaSkillAttackWaitTime;
    
    [ShowIf("EffectType", EffectType.NewKatanaSkill)]
    [LabelText("特效")]
    public GameObject KatanaSkillFx;
    [ShowIf("EffectType", EffectType.NewKatanaSkill)]
    [LabelText("音效")]
    public AudioClip KatanaSkillAudio;
}
//戳戳戳戳戳戳戳戳戳
public class NewKatanaSkillEffect:ItemEffect
{
    // private float killHpPercentage;//斩杀线
    private int killRecoveryPower;//击杀恢复能量数
    private float attackDis;//攻击距离
    private float attackAng;//攻击角度
    private float attackPowerPercentage;//攻击力百分比
    private float waitTime;

    private GameObject skillKillFx;
    private AudioClip skillKillAudio;
    
    public NewKatanaSkillEffect(int killPower,float attackDis,float attackAng,float attackPowerPercentage
        ,float waitTime,GameObject killFx,AudioClip killAduio)
    {
        // killHpPercentage = killLine;
        killRecoveryPower = killPower;
        this.attackDis = attackDis;
        this.attackAng = attackAng;
        this.attackPowerPercentage = attackPowerPercentage;
        this.waitTime = waitTime;
        this.skillKillFx = killFx;
        this.skillKillAudio = killAduio;
    }


    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        // if (roleController is PlayerKatanaController playerKatanaController && playerKatanaController.IsFevering)
        // {
        //     roleController.Animator.SetTrigger("Skill_Spike2");
        //     return;
        // }
        roleController.Animator.SetTrigger("Skill_Spike");
        if (roleController.Animator2 != null)
        {
            roleController.Animator2.SetTrigger("Skill_Spike");
        }
        roleController.StartCoroutine(WaitDoEffect(waitTime));
    }

    IEnumerator WaitDoEffect(float time)
    {
        if (roleController.IsDie)
            yield break;
        
        var enemyTargets = BattleTool.GetEnemysInDistance(roleController,attackDis);
        float minHp = 9999;
        RoleController target = null; 
        foreach (RoleController enemyTarget in enemyTargets)
        {
            float hpValue = enemyTarget.CurrentHp / enemyTarget.MaxHp;
            if (hpValue < minHp)
            {
                minHp = hpValue;
                target = enemyTarget;
            }
        }
        if (target != null)
        {
            var dir = target.transform.position - roleController.transform.position;
            roleController.Animator.transform.forward = dir;
        }

        float timer = 0;
        while (timer < time)
        {
            if (roleController.IsDie)
                yield break;
            
            yield return null;
            timer += Time.deltaTime;
            
            if (target != null)
            {
                var dir = target.transform.position - roleController.transform.position;
                roleController.Animator.transform.forward = dir;
            }
        }
        
        if (roleController.IsDie)
            yield break;
        

        var targets = GetTargets();
        foreach (var targetEnemy in targets)
        {
            DamageInfo dmg = new DamageInfo(
                target.TemporaryId,
                attackPowerPercentage * roleController.AttackPower,
                roleController, 
                roleController.transform.position, 
                DmgType.Physical,
                true,
                false,
                0,
                0,
                false,
                null, null, null, false);
            
            targetEnemy.HpInjured(dmg);
        }

        foreach (var targetEnemy in targets)
        {
            if (targetEnemy.IsDie || 
                targetEnemy.CurrentHp <= targetEnemy.MaxHp/(float)targetEnemy.roleHealth.HpBarCount)
            {
                DamageInfo dmg = new DamageInfo(
                    target.TemporaryId,
                    targetEnemy.CurrentHp+1,
                    roleController, 
                    roleController.transform.position, 
                    DmgType.Physical,
                    true,
                    false, 0, 0, false, null, null, null, false);
                targetEnemy.HpInjured(dmg);
                if (skillKillFx != null)
                {
                    var fx = GameObject.Instantiate(skillKillFx, target.transform);
                    fx.transform.localPosition = Vector3.zero;
                }

                if (skillKillAudio != null)
                {
                    skillKillAudio.Play();
                }
                
                if (roleController is PlayerKatanaController playerKatanaController)
                    playerKatanaController.AddFeverPower(killRecoveryPower);

                roleController.StartCoroutine(WaitRefreshCoolDown());
            }
        }
    }

    IEnumerator WaitRefreshCoolDown()
    {
        //yield return new WaitForSeconds(0.5f);
        yield return null;
        Root.item.RefreshCool();
    }
    
    List<RoleController> GetTargets()
    {
        List<RoleController> targets = new List<RoleController>();
        foreach (var enemy in BattleManager.Inst.EnemyTeam)
        {
            if (enemy.Value.IsDie)
                continue;
            Vector3 v3 = enemy.Value.transform.position - roleController.transform.position;
            if (v3.magnitude > attackDis)
                continue;
            float angle = Vector3.Angle(roleController.Animator.transform.forward, v3);
            if (angle > attackAng * 0.5f)
                continue;
            targets.Add(enemy.Value);
        }
        return targets;
    }
}