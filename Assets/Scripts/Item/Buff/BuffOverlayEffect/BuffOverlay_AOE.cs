using System.Collections.Generic;
using UnityEngine;

public class BuffOverlay_AOE : BuffOverlayEffect
{

    private float Distance;
    private int DmgValue;
    private bool isMove;
    private float moveSpeed;
    private float moveTime;
    private ParticleSystem particleSystem;
    public BuffLifeCycle lifeCycle;
    private BuffScriptableObject Buff;
    // private float buffTime;
    private Vector3 particleOffset;
    public BuffOverlay_AOE(List<int> probability,float distance,int dmgvalue,bool ismove,float movespeed,float movetime,ParticleSystem particleSystem,Vector3 particleOffset, BuffScriptableObject buff,BuffLifeCycle lifeCycle) : base(probability)
    {
        Distance = distance;
        DmgValue = dmgvalue;
        isMove = ismove;
        moveSpeed = movespeed;
        moveTime = movetime;
        Buff = buff;
        this.particleSystem = particleSystem;
        this.lifeCycle = lifeCycle;
        this.particleOffset = particleOffset;
    }

    private ParticleSystem currentParticle;
    public override void Destroy()
    {
        if (particleSystem!=null)
            GameObject.Destroy(currentParticle);
    }

    RoleBuff getBuff()
    {
        RoleBuff aoebuff =null;
        if (Buff)
        {
            // Buff.LifeCycle = lifeCycle;
            aoebuff = DataManager.Inst.ParsingBuff(Buff,lifeCycle);
        }

        return aoebuff;
    }

    public override void Trigger(int overlayNumber)
    {
        base.Trigger(overlayNumber);
        var PlayerPos = _roleBuff.roleController.transform.position;
        foreach (KeyValuePair<string,RoleController> enemy in BattleManager.Inst.EnemyTeam)
        {
            if (enemy.Value == null||enemy.Value.IsDie)
            {
                continue;
            }
            float dis = (enemy.Value.transform.position - PlayerPos).magnitude;
            if (dis <= Distance)
            {
                DamageInfo dmg = new DamageInfo(enemy.Value.TemporaryId,DmgValue,_roleBuff.Adder, _roleBuff.roleController.transform.position,
                DmgType.Other, false,isMove,moveSpeed,moveTime);
                /*
                dmg.DmgValue = DmgValue;
                dmg.Attacker = _roleBuff.Adder;
                dmg.AttackPoint = _roleBuff.roleController.transform.position;
                dmg.IsUseMove = isMove;
                dmg.MoveSpeed = moveSpeed;
                dmg.MoveTime = moveTime;
                dmg.Interruption = false;
                */
                enemy.Value.HpInjured(dmg);

                if ( Buff!=null)// && enemy.Value.CurrentId != _roleBuff.roleController.CurrentId)
                {
                    enemy.Value.roleBuffController.AddBuff(getBuff(), _roleBuff.Adder);
                }
                
            }
            
        }

        if (currentParticle != null)
        {
            currentParticle.transform.position = _roleBuff.roleController.transform.position + particleOffset;
            currentParticle.Play();
        }
        else
        {
            if (particleSystem!=null)
            {
                currentParticle = GameObject.Instantiate(particleSystem);
                currentParticle.transform.position = _roleBuff.roleController.transform.position + particleOffset;
                currentParticle.Play();
            }
        }
    }
    
    
    
    
}
