using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTestHealth : EnemyHealth
{

    public TextMesh TextMesh;

    public float AverageTimeS = 3;
    private struct dmgInfo
    {
        public float time;
        public float value;
    }
    
    List<dmgInfo> dmgList = new List<dmgInfo>();
    public override void Injured(DamageInfo dmg)
    {
        roleController.Animator.SetTrigger(Hit);
        dmgList.Add(new dmgInfo(){time = Time.time,value = dmg.DmgValue});
        dmg.DmgValue = 1;
        base.Injured(dmg);
        currentHp = maxHp;
    }

    private void Update()
    {
        checkDPS();
    }

    void checkDPS()
    {
        float dmgValues=0;
        var delectInfos = new List<dmgInfo>();
        foreach (dmgInfo info in dmgList)
        {
            if (Time.time - info.time > AverageTimeS)
            {
                delectInfos.Add(info);
            }
            else
            {
                dmgValues += info.value;
            }
        }

        dmgValues /= AverageTimeS;

        foreach (var dmgInfo in delectInfos)
        {
            dmgList.Remove(dmgInfo);
        }

        TextMesh.text = dmgValues.ToString();
    }
    
}
