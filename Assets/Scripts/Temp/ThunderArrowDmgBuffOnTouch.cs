using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class ThunderArrowDmgBuffOnTouch :ArrowDmgBuffOnTouch
{
    [LabelText("弹射次数")]
    public int CatapultTimes;
    [LabelText("每次弹射距离")]
    public int CatapultDistance;
    [LabelText("弹射伤害")]
    public int CatapultAttackPower;
    private LightningChainEffect lightningChainEffect;
    private void Awake()
    {
        lightningChainEffect = new LightningChainEffect("",CatapultTimes, CatapultDistance, CatapultAttackPower,0,0,Color.black);
        lightningChainEffect.Awake(BattleManager.Inst.CurrentPlayer.roleItemController);
    }
    protected override void Update()
    {
        base.Update();

        lightningChainEffect.Update(BattleManager.Inst.CurrentPlayer.roleItemController);
    }
    private void OnDestroy()
    {
        lightningChainEffect.Destroy(BattleManager.Inst.CurrentPlayer.roleItemController);
    }
}
