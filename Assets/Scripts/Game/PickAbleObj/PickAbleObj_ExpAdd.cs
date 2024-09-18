using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAbleObj_ExpAdd : PickAbleObj
{
    public float AddValue;

    protected override void Start()
    {
        base.Start();
        //经验有随机浮动值
        AddValue += (Random.value - 1) * 0.4f;
    }
    protected override void TakeEffect()
    {
        // if (BattleManager.Inst.CurrentPlayer.playerExperience.enabled)
        //     BattleManager.Inst.CurrentPlayer.playerExperience.AddExp(AddValue);

        base.TakeEffect();
    }

}
