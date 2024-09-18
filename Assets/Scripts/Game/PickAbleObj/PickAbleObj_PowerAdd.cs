using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAbleObj_PowerAdd : PickAbleObj
{
    public float AddValue;


    protected override void TakeEffect()
    {
        BattleManager.Inst.CurrentPlayer.AddPower(AddValue);

        base.TakeEffect();
    }
}
