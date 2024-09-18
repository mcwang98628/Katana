using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFly_Surround : Surround_Obj
{
    [SerializeField]
    private GameObject fireFly;
    [SerializeField]
    private int minFireFlyCount;
    [SerializeField]
    private int maxFireFlyCount;

    protected override void Trigger(string targetRoleId)
    {
        base.Trigger(targetRoleId);
        HitTarget(targetRoleId);
    }

    void HitTarget(string roleId)
    {
        int fireFlyCount = UnityEngine.Random.Range(minFireFlyCount,maxFireFlyCount+1);

        for (int i = 0; i < fireFlyCount; i++)
        {
            GameObject go= Instantiate(fireFly, transform.position, transform.rotation);

            //TODO：法球找不到拥有者，现在先写死player
            go.GetComponent<DmgBuffOnTouch>().Init(BattleManager.Inst.CurrentPlayer);
        }
    }
}