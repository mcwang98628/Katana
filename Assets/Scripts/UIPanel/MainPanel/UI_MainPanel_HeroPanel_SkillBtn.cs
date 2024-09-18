using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Events;

public class UI_MainPanel_HeroPanel_SkillBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public CanvasGroup SkillInfo;
    public UnityEvent CallBack;
    Tweener tweener;
    private void Start()
    {
        SkillInfo.alpha = 0;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (tweener != null)
            tweener.Kill();
        tweener = SkillInfo.DOFade(1, 0.2f);
        EventManager.Inst.DistributeEvent(TGANames.MainPanelPeekHeroSkillInfo);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (tweener != null)
            tweener.Kill();
        tweener = SkillInfo.DOFade(0, 0.2f);
        
        CallBack?.Invoke();
    }

}
