using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShieldPower : RolePower
{
    // [SerializeField]
    // private float FlawAttackPower=1.5f;
    // protected override void OnPlayerAttack(string arg1, object arg2)
    // {
    //     float value = attackPowerRecover;
    //     if (BattleManager.Inst.EnemyTeam.ContainsKey(arg2.ToString()))
    //     {
    //         int count = BattleManager.Inst.EnemyTeam[arg2.ToString()].roleBuffController
    //             .GetBuffCount(((PlayerShieldHealth) RoleController.roleHealth).testBuff.ID);
    //         if (count>0)
    //         {
    //             value *= FlawAttackPower;
    //         }
    //     }
    //     AddSkillPower(value);
    // }
}
