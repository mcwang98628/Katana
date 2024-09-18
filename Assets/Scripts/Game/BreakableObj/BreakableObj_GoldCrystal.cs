using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
public class BreakableObj_GoldCrystal : BreakableObj
{

    [Header("GoldCrystal")]
    public int MaxHp = 10;
    int currentHp = 0;
    public ItemScriptableObject GoldItem;
    public AnimationCurve ScaleAnimCurve;
    public FeedBackObject OnHitFeedBack;
    public FeedBackObject OnBreakFeedBack;
    protected override void Start()
    {
        base.Start();
        currentHp = MaxHp;
    }
    public override void BreakObj()
    {

        if (currentHp <= 0)
        {
            IsBroken = true;
            SwitchModel();
            PlayPartical();
            FeedbackManager.Inst.UseFeedBack(BattleManager.Inst.CurrentPlayer, OnBreakFeedBack);
        }
        else
        {
            currentHp--;
            SpwanGold();
        }
    }
    Tweener tweener;
    void SpwanGold()
    {
        BattleManager.Inst.CurrentPlayer.roleItemController.AddItem(DataManager.Inst.ParsingItemObj(GoldItem), isOk => { });
        FeedbackManager.Inst.UseFeedBack(BattleManager.Inst.CurrentPlayer, OnHitFeedBack);
        PlayPartical();
        if (tweener != null)
        {
            tweener.Kill();
            Model.transform.localScale = Vector3.one;
        }
        tweener = Model.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.15f).SetEase(ScaleAnimCurve);
    }
    protected override void PlayPartical()
    {
        BreakParticles.GetComponent<ParticleSystem>().Play();
    }
}
