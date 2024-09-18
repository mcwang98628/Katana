using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AOE_Surround : Surround_Obj
{
    [SerializeField]
    [LabelText("AOE半径")]
    private float aoeDistance;
    [SerializeField]
    [LabelText("使用击退")]
    private bool isMove;
    
    [SerializeField]
    [LabelText("击退速度")]
    [ShowIf("isMove")]
    private float moveSpeed;
    [SerializeField]
    [LabelText("击退时间")]
    [ShowIf("isMove")]
    private float moveTime;
    
    protected override void Trigger(string targetRoleId)
    {
        base.Trigger(targetRoleId);
        AOEAttack(targetRoleId);
    }

    void AOEAttack(string roleId)
    {
        if (!Enemys.ContainsKey(roleId))
        {
            return;
        }
        foreach (KeyValuePair<string,RoleController> enemy in Enemys)
        {
            float dis = (enemy.Value.transform.position - Enemys[roleId].transform.position).magnitude;
            if (dis <= aoeDistance)
            {
                /*
                DamageInfo dmg = new DamageInfo();
                dmg.DmgValue = dmgValue;
                dmg.Attacker = BattleManager.Inst.CurrentPlayer;
                dmg.AttackPoint = transform.position;
                dmg.IsUseMove = isMove;
                dmg.MoveSpeed = moveSpeed;
                dmg.MoveTime = moveTime;
                dmg.Interruption = false;
                */
                DamageInfo dmg = new DamageInfo(enemy.Value.TemporaryId,dmgValue, BattleManager.Inst.CurrentPlayer, transform.position, DmgType.Other,
                    false, isMove,moveTime,moveSpeed);
                enemy.Value.HpInjured(dmg);
            }
        }
    }
}
