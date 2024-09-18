using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Buff_Surround : Surround_Obj
{
    [SerializeField]
    [LabelText("Buff 持续")]
    public BuffLifeCycle LifeCycle;

    [SerializeField]
    [LabelText("Buff 效果")]
    private BuffScriptableObject buffObj;



    protected override void Trigger(string targetRoleId)
    {
        base.Trigger(targetRoleId);
    
        ColdTarget(targetRoleId);
    }

    void ColdTarget(string targetId)
    {
        if (!Enemys.ContainsKey(targetId))
        {
            return;
        }
        // buffObj.LifeCycle = LifeCycle;
        var buff = DataManager.Inst.ParsingBuff(buffObj,LifeCycle);

        Enemys[targetId].roleBuffController.AddBuff(buff,BattleManager.Inst.CurrentPlayer);
    }
}
