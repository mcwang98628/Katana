using BehaviorDesigner.Runtime.Tasks.Unity.Timeline;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRecover_Surround : Hit_Surround
{
    [SerializeField]
    private int healPower;

    [SerializeField]
    private GameObject healParticle;

    protected override void Trigger(string targetRoleId)
    {
        base.Trigger(targetRoleId);
        PlayerRecover();
    }

    void PlayerRecover()
    {
        RoleController player = BattleManager.Inst.CurrentPlayer;
        float healValue = healPower;

        player.HpTreatment(new TreatmentData(healValue,player.TemporaryId));
        Instantiate(healParticle, player.transform.position, Quaternion.identity);
    }
}