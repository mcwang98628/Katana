using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAbleObj_GoldAdd : PickAbleObj
{
    public int AddValue;

    protected override void Start()
    {
        base.Start();
        //经验有随机浮动值
    }
    protected override void TakeEffect()
    {
        BattleManager.Inst.AddGold(AddValue);

        base.TakeEffect();
    }

}
