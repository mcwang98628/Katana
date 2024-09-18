using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UI_PromotePanel : PanelBase
{
    [SerializeField]
    private CanvasGroup closeBtn;
    [SerializeField]
    private UIText skillDesc;
    
    public void OnOpen(int heroId)
    {
        closeBtn.interactable = false;
        closeBtn.DOFade(1, 0.3f).OnComplete(() =>
        {
            closeBtn.interactable = true;
        }).SetDelay(2f).Delay();

        HeroUpgradeData heroUpgradeData = ArchiveManager.Inst.GetHeroUpgradeData(heroId).GetHeroUpgradeData();
        skillDesc.text = heroUpgradeData.SkillDesc;
    }

    protected override void OnPause()
    {
        
    }

    protected override void OnUnPause()
    {
        
    }

    public void OnCloseBtnClickPanel()
    {
        UIManager.Inst.Close();
    }
}

