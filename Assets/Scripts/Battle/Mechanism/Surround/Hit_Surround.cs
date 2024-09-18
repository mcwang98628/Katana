using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_Surround : Surround_Obj
{

    protected override void Trigger(string targetRoleId)
    {
        base.Trigger(targetRoleId);
        HitTarget(targetRoleId);
    }

    void HitTarget(string roleId)
    {
        /*     
        DamageInfo dmg = new DamageInfo();
        dmg.DmgValue = attackPower;
        dmg.Attacker = BattleManager.Inst.CurrentPlayer;
        dmg.AttackPoint = BattleManager.Inst.CurrentPlayer.transform.position;
        dmg.IsUseMove = false;
        dmg.Interruption = false;
        */

        DamageInfo dmg = new DamageInfo(Enemys[roleId].TemporaryId,dmgValue, BattleManager.Inst.CurrentPlayer, BattleManager.Inst.CurrentPlayer.transform.position, DmgType.Physical,
            false, false,0,0);
        Enemys[roleId].HpInjured(dmg);
    }
}