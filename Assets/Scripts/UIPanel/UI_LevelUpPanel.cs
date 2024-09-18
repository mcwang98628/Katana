using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UI_LevelUpPanel : PanelBase
{
    [SerializeField]
    private CanvasGroup closeBtn;
    
    public void OnOpen()
    {
        closeBtn.interactable = false;
        closeBtn.DOFade(1, 0.3f).OnComplete(() =>
        {
            closeBtn.interactable = true;
        }).SetDelay(2f).Delay();
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
