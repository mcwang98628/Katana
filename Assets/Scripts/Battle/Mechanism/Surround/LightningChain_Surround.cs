using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LightningChain_Surround : Surround_Obj
{
    [LabelText("弹射次数")]
    public int CatapultTimes;
    [LabelText("每次弹射距离")]
    public int CatapultDistance;
//    [LabelText("弹射伤害")]
//    public int CatapultAttackPower;

    private LightningChainEffect lightningChainEffect;

    public override void Start()
    {
        base.Start();
        
        lightningChainEffect = new LightningChainEffect("",CatapultTimes,CatapultDistance,dmgValue,0,0,Color.black,null,true);
        lightningChainEffect.Awake(BattleManager.Inst.CurrentPlayer.roleItemController);
    }

    public override void Update()
    {
        base.Update();
        if (lightningChainEffect != null && BattleManager.Inst.CurrentPlayer != null)
        {
            lightningChainEffect.Update(BattleManager.Inst.CurrentPlayer.roleItemController);
        }
    }

    private void OnDestroy()
    {
        if (BattleManager.Inst.CurrentPlayer == null)
        {
            return;
        }
        lightningChainEffect.Destroy(BattleManager.Inst.CurrentPlayer.roleItemController);
    }

    protected override void Trigger(string targetRoleId)
    {
        base.Trigger(targetRoleId);
        lightningChainEffect.TriggerEffect(new ItemEffectTriggerValue(){TargetId = targetRoleId});
    }
    
    
}
