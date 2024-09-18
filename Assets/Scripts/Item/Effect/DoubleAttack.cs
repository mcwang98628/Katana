

using System.Collections;
using UnityEngine;

public class DoubleAttack : ItemEffect
{
    

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        if (value == null || value.Value.DamageInfo == null)
            return;
        if (value.Value.DamageInfo.AttackType != DamageAttackType.NormalAttack)
            return;
        roleController.StartCoroutine(DelayDoubleAttack(value.Value.DamageInfo));
    }

    IEnumerator DelayDoubleAttack(DamageInfo dmgInfo)
    {
        int count = roleController.roleItemController.GetCurrentFrameItemEffectTriggerTimes(GetType()) -1;
        yield return new WaitForSeconds(0.15f);
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(0.15f);
        }
        dmgInfo.IsUseMove = false;
        if (BattleManager.Inst.EnemyTeam.ContainsKey(dmgInfo.HitRoleId))
        {
            dmgInfo.AttackType = DamageAttackType.None;
            BattleManager.Inst.EnemyTeam[dmgInfo.HitRoleId].HpInjured(dmgInfo);
        }
    }
    
}