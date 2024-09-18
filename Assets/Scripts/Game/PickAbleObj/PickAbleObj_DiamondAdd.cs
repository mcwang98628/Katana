using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAbleObj_DiamondAdd : PickAbleObj
{
    public int AddValue;

    protected override void Start()
    {
        base.Start();
        //经验有随机浮动值
    }
    protected override void TakeEffect()
    {
        BattleManager.Inst.AddDiamond(AddValue);

        base.TakeEffect();
    }

}
