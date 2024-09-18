using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBuffEffect : ItemEffect
{
    private BuffScriptableObject roleBuff;
    private BuffLifeCycle _lifeCycle;
    public ItemBuffEffect(BuffScriptableObject roleBuff,BuffLifeCycle lifeCycle)
    {
        this.roleBuff = roleBuff;
        _lifeCycle = lifeCycle;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        if (value.HasValue)
        {
            if (!string.IsNullOrEmpty(value.Value.TargetId))
            {
                AddBuff(value.Value.TargetId);
            }else if (value.Value.DamageInfo!=null)
            {
                AddBuff(value.Value.DamageInfo.HitRoleId);
            }
        }
        else
        {
            AddBuff(roleController.TemporaryId);
        }
    }

    void AddBuff(string targetId)
    {
        RoleBuff newbuff = DataManager.Inst.ParsingBuff(roleBuff,_lifeCycle);

        if (BattleManager.Inst.EnemyTeam.ContainsKey(targetId))
        {
            var enemy = BattleManager.Inst.EnemyTeam[targetId];
            if (enemy!=null && !enemy.IsDie)
            {
                enemy.roleBuffController.AddBuff(newbuff,roleController);
            }
        }else if (roleController.TemporaryId == targetId)
        {
            roleController.roleBuffController.AddBuff(newbuff,roleController);
        }
    }
}
