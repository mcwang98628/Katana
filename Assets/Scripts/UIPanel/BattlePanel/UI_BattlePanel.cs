using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattlePanel : PanelBase
{
    [SerializeField]
    private VerticalLayoutGroup _layoutGroupL;
    [SerializeField]
    private VerticalLayoutGroup _layoutGroupR;
    
    [SerializeField]
    private Text goldText;

    [SerializeField]
    private CanvasGroup comboTextGanvas;
    [SerializeField]
    private Text comboText;
    [SerializeField]
    private Text hitText;

    [SerializeField]
    private UI_Joy joy;
    [SerializeField]
    private CanvasGroup panelCanvasGroup;
    [SerializeField]
    private UI_BattlePanel_Dialog dialog;
    [SerializeField]
    private Button _dialogBtn;

    [SerializeField]
    private CanvasGroup _assetsCanvasGroup;
    
    private void Awake()
    {
        EventManager.Inst.AddEvent(EventName.OnRoleInjured, OnPlayerHitEnemy);
        EventManager.Inst.AddEvent(EventName.GuideDialog,OnDialog);
        EventManager.Inst.AddEvent(EventName.OnMoveCamera,OnMoveCamera);
#if UNITY_ANDROID || UNITY_IOS
        EventManager.Inst.AddEvent(EventName.HideJoy,OnHideJoy);
#endif
        EventManager.Inst.AddEvent(EventName.EnterFightRoom,OnEnterFightRoom);
        EventManager.Inst.AddEvent(EventName.ClearAllEnemy,OnClearAllEnemy);
    }
    private void OnDestroy()
    {
#if UNITY_ANDROID || UNITY_IOS
        EventManager.Inst.RemoveEvent(EventName.HideJoy,OnHideJoy);
#endif
        EventManager.Inst.RemoveEvent(EventName.GuideDialog,OnDialog);
        EventManager.Inst.RemoveEvent(EventName.OnMoveCamera,OnMoveCamera);
        EventManager.Inst.RemoveEvent(EventName.OnRoleInjured, OnPlayerHitEnemy);
        EventManager.Inst.RemoveEvent(EventName.EnterFightRoom,OnEnterFightRoom);
        EventManager.Inst.RemoveEvent(EventName.ClearAllEnemy,OnClearAllEnemy);
    }

    private void OnMoveCamera(string arg1, object arg2)
    {
        bool isMoving = (bool) arg2;
        
        if (isMoving)
        {
            panelCanvasGroup.DOKill();
            panelCanvasGroup.interactable = false;
            panelCanvasGroup.DOFade(0, 0.3f);
        }
        else
        {
            panelCanvasGroup.DOKill();
            panelCanvasGroup.interactable = true;
            panelCanvasGroup.DOFade(1, 0.3f);
        }
    }

    private void OnEnterFightRoom(string arg1, object arg2)
    {
        _assetsCanvasGroup.DOKill(false);
        _assetsCanvasGroup.DOFade(0, 0.3f);
    }

    private void OnClearAllEnemy(string arg1, object arg2)
    {
        _assetsCanvasGroup.DOKill(false);
        _assetsCanvasGroup.DOFade(1, 0.3f);
    }

    private void Start()
    {
        _layoutGroupL.padding.top += (int)Screen.safeArea.min.y;
        _layoutGroupR.padding.top += (int)Screen.safeArea.min.y;
    }

    private void OnHideJoy(string arg1, object arg2)
    {
        bool isHide = (bool) arg2;
        joy.gameObject.SetActive(!isHide);
    }

    private void OnDialog(string arg1, object arg2)
    {
        BattleGuideSequenceData data = (BattleGuideSequenceData) arg2;
        if (string.IsNullOrEmpty(data.Text))
        {
            return;
        }
        if (showTweener!=null)
        {
            showTweener.Kill();
            showTweener = null;
        }
        _dialogBtn.gameObject.SetActive(data.Force);
        
        if (data.Force)
        {
            BattleManager.Inst.CurrentPlayer.StopFastMove();
            panelCanvasGroup.interactable = false;
            panelCanvasGroup.DOFade(0, 0.3f);
        }
        else
        {
            panelCanvasGroup.interactable = true;
            panelCanvasGroup.DOFade(1, 0.3f);
        }
        // Debug.LogError(data.Text);
        dialog.ShowDialog(data.Text,data.ShowTime,data.TextColor,data.Force);
        // StartCoroutine(waitPanelCanvasGroup(data.ShowTime));
    }

    private Tweener showTweener;
    // IEnumerator waitPanelCanvasGroup(float time)
    // {
    //     yield return new WaitForSecondsRealtime(time);
    //     ShowPanelCanvas();
    // }

    public void ShowPanelCanvas()
    {
        
        panelCanvasGroup.interactable = true;
        showTweener = panelCanvasGroup.DOFade(1, 0.3f);
        showTweener.SetDelay(0.5f).Delay();
    }

    private Tweener comboTextScaleTweener;
    private Tweener comboTextColorTweener;
    private void OnPlayerHitEnemy(string arg1, object arg2)
    {
        var data = (RoleInjuredInfo) arg2;
        if (data.Dmg.AttackerRole != BattleManager.Inst.CurrentPlayer ||
            data.Dmg.HitRoleId == BattleManager.Inst.CurrentPlayer.TemporaryId ||
            data.Dmg.DmgType != DmgType.Physical || 
            data.Dmg.DmgValue < BattleManager.Inst.CurrentPlayer.AttackPower * 0.3f)
        {
            return;
        }

        int number = ((PlayerAttack) BattleManager.Inst.CurrentPlayer.roleAttack).ComboNumber;
        if (number < 3)
            return;

        StartCoroutine(waitShow());
    }

    IEnumerator waitShow()
    {
        yield return null;
        if (comboTextScaleTweener!=null)
        {
            comboTextScaleTweener.Kill();
            comboTextColorTweener.Kill();
        }

        comboTextGanvas.alpha = 1;
        int number = ((PlayerAttack) BattleManager.Inst.CurrentPlayer.roleAttack).ComboNumber;
        if (number < 10)
        {
            comboText.color =new Color(0.2f, 1f, 0.0f, 1f);
        }
        if (number < 20)
        {
            comboText.color =new Color(1f, 0.8f, 0.0f, 1f);
        }
        else if (number < 40)
        {
            comboText.color = new Color(0.6f, 0f, 0.2f, 1f);
        }
        else if (number < 60)
        {
            comboText.color = new Color(0.0f, 0.8f, 1f, 1f);
        }
        else if (number < 80)
        {
            comboText.color = new Color(1.0f, 0.4f, 0.0f, 1f);
        }
        else
        {            
            comboText.color = new Color(0.6f, 0f, 0, 1f);
        }
        comboText.text = "x"+number;
        // if (number > 3)
        // {
        //     hitText.text = "HITS";
        // }
        // else
        // {
        //     hitText.text = "HIT";
        // }
        comboTextGanvas.transform.localScale = Vector3.one * 2f;
        comboTextScaleTweener = comboTextGanvas.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        comboTextColorTweener = comboTextGanvas.DOFade(0, 0.3f).SetDelay(1f);
        comboTextColorTweener.Delay();
        // comboTextColorTweener = comboText.DOColor(new Color(1, 1, 1, 0), 0.3f).SetDelay(2f);
    }

    protected override void OnPause(){}
    protected override void OnUnPause(){}

    public void OnPauseBtnClick()
    {
        UIManager.Inst.Open("PausePanel",true);
    }

    

    private bool BgmIsPlaying = false;
    

    
    private void FixedUpdate()
    {
        int goldCount = BattleManager.Inst.CurrentGold;
        if (goldCount < 10)
            goldText.text = "00" + goldCount;
        else if (goldCount < 100)
            goldText.text = "0" + goldCount;
        else
            goldText.text = goldCount.ToString();
    }

    public void OnDialogBtnClick()
    {
        dialog.HideDialog(0);
        ShowPanelCanvas();
        BattleGuide.Inst.NextGuide();
    }
    
    // public void TestBtnFunc(int value)
    // {
    //     Application.targetFrameRate = value;
    // }
}
