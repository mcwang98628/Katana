using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEAttackEffect : ItemEffect
{
    public SpwanPosType CenterPosType;
    private float Distance;
    private int DmgValue;
    private float AOEAttackPowerPercentage;
    private bool isMove;
    private float moveSpeed;
    private float moveTime;
    private ParticleSystem particleSystem;

    private BuffLifeCycle LifeCycle;
    private BuffScriptableObject Buff;

    private Vector3 FxOffset;
    public AOEAttackEffect(SpwanPosType centerPosType,float distance, int dmgvalue,float AOEAttackPowerPercentage, bool ismove, float movespeed, float movetime, ParticleSystem particleSystem, Vector3 fxOffset, BuffScriptableObject buff = null, BuffLifeCycle lifeCycle = null)
    {
        CenterPosType = centerPosType;
        FxOffset = fxOffset;
        Distance = distance;
        DmgValue = dmgvalue;
        this.AOEAttackPowerPercentage = AOEAttackPowerPercentage;
        isMove = ismove;
        moveSpeed = movespeed;
        moveTime = movetime;
        Buff = buff;
        LifeCycle = lifeCycle;
        this.particleSystem = particleSystem;
    }

    private ParticleSystem currentParticle;
    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        DmgValue += (int) (roleController.OriginalAttackPower * AOEAttackPowerPercentage);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        if (particleSystem != null)
            GameObject.Destroy(currentParticle);
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        if (value.HasValue)
        {
            Trigger(value.Value.TargetId);
        }
        else
        {
            Trigger("");
        }
    }

    void Trigger(string targetId)
    {
        Vector3 CenterPos = BattleManager.Inst.CurrentPlayer.transform.position;
        switch (CenterPosType)
        {
            case SpwanPosType.RoleCenter:
                break;
            case SpwanPosType.EnemyTarget:
                if (BattleManager.Inst.EnemyTeam.ContainsKey(targetId))
                {
                    var enemy = BattleManager.Inst.EnemyTeam[targetId];
                    if (enemy != null)
                    {
                        CenterPos = enemy.transform.position;
                    }
                }
                break;
            default:
                break;
        }
        
        foreach (KeyValuePair<string, RoleController> enemy in BattleManager.Inst.EnemyTeam)
        {
            if (enemy.Value == null || enemy.Value.IsDie)
            {
                continue;
            }
            float dis = (enemy.Value.transform.position - CenterPos).magnitude;
            if (dis <= Distance)
            {
                /*
                DamageInfo dmg = new DamageInfo();
                dmg.DmgValue = DmgValue;
                dmg.Attacker = roleController;
                dmg.AttackPoint = roleController.transform.position;
                dmg.IsUseMove = isMove;
                dmg.MoveSpeed = moveSpeed;
                dmg.MoveTime = moveTime;
                dmg.Interruption = false;
                */
                DamageInfo dmg = new DamageInfo(enemy.Value.TemporaryId,DmgValue, roleController, CenterPos, DmgType.Other,
                    false, isMove, moveTime, moveSpeed);
                enemy.Value.HpInjured(dmg);

                if (Buff != null)
                {
                    // Buff.LifeCycle = LifeCycle;
                    RoleBuff newbuff = DataManager.Inst.ParsingBuff(Buff,LifeCycle);
                    enemy.Value.roleBuffController.AddBuff(newbuff, roleController);
                }
            }
        }

        if (currentParticle != null)
        {
            currentParticle.transform.position = CenterPos + FxOffset;
            currentParticle.Play();
        }
        else
        {
            if (particleSystem != null)
            {
                currentParticle = GameObject.Instantiate(particleSystem);
                currentParticle.transform.position = CenterPos + FxOffset;
                currentParticle.Play();
            }
        }
    }
}
