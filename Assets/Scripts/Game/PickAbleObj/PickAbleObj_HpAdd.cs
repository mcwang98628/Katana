using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAbleObj_HpAdd : PickAbleObj
{
    public float AddValue;


    protected override void TakeEffect()
    {
        TreatmentData treatment=new TreatmentData(AddValue,BattleManager.Inst.CurrentPlayer.TemporaryId);
        BattleManager.Inst.CurrentPlayer.HpTreatment(treatment);

        base.TakeEffect();
    }
}
