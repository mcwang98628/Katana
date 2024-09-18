using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UI_ShopPanel : PanelBase
{
    // [SerializeField]
    // private UI_ShopPanel_ItemCurrency itemCurrencyPrefab;
    // [SerializeField]
    // private Transform itemCurrencyGroup;
    [SerializeField]
    private Transform group;
    [SerializeField]
    private UI_ShopPanel_ShopItem shopItem;
    [SerializeField]
    private Text text;
    [SerializeField]
    private UIText PanelName;

    // [SerializeField]
    // private Text tipsText;
    
    private List<UI_ShopPanel_ShopItem> shopItemList = new List<UI_ShopPanel_ShopItem>();

    [SerializeField]
    private Text diceText;
    // [SerializeField]
    // private Text _refreshPriceText;

    [SerializeField]
    private GameObject _refreshBtn;
    
    private ChapterData cpData;
    private InteractObj_NPC gameNpc;

    // private int RefreshPrice => cpData.ItemData.ShopRefreshPrice +
    //                             BattleManager.Inst.RuntimeData.ShopRefreshTimes * cpData.ItemData.ShopRefreshPriceStep;
    public void OnOpen(InteractObj_NPC gameNpc)
    {
        this.gameNpc = gameNpc;
        PanelName.text = gameNpc.DialogData.NPCTalkName;
        
        DataManager.Inst.GetCpData(BattleManager.Inst.RuntimeData.CurrentChapterId).LoadChapterData(
            delegate(ChapterData cpData)
            {
                this.cpData = cpData;
            });
        Init();
    }

    void Init()
    {
        // _refreshPriceText.text = $"{RefreshPrice} G";
        for (int i = 0; i < group.childCount; i++)
        {
            Destroy(group.GetChild(i).gameObject);
        }
        if (gameNpc.ItemGods != null)
        {
            StartCoroutine (DelayInitShopItem(0.15f));
        }
        else
        {
            Debug.LogError("错误！");
        }
    }

    
    private bool canRefresh=true;
    IEnumerator DelayInitShopItem(float interval)
    {
        canRefresh = false;
        foreach (var gods in gameNpc.ItemGods)
        {
            var go = Instantiate(shopItem, group);
            go.Init(gameNpc,gods);
            go.SetNPC(gameNpc);
            shopItemList.Add(go);
            yield return new WaitForSecondsRealtime(interval);
        }
        canRefresh = true;
    }

    public void Refresh()
    {
        if(!canRefresh)
            return;

        // if (gameNpc.ItemGods == null || gameNpc.ItemGods.Count == 0)
        //     return;
        
        if (BattleManager.Inst.RuntimeData.CurrentDice < 1)
        {
            // ShowTips(LocalizationManger.Inst.GetText("NotEnoughDice"), Color.red);
            // ShowTips(LocalizationManger.Inst.GetText("ShopPanel_3"),Color.red);
            // ShowTips(LocalizationManger.Inst.GetText("UI_Tips_NoDiamond"),Color.white);
            UIManager.Inst.Tips.ShowText(LocalizationManger.Inst.GetText("NotEnoughDice"));
            return;
        }
        // ShowTips(LocalizationManger.Inst.GetText("UI_Refresh"),Color.white);
        UIManager.Inst.Tips.ShowText(LocalizationManger.Inst.GetText("UI_Refresh"));
        
        BattleManager.Inst.RuntimeData.AddDice(-1);
        BattleManager.Inst.RuntimeData.ShopRefreshTimes++;
        gameNpc.RefreshFunc?.Invoke();
        Init();
    }
    
    

    public override void Show()
    {        if (gameObject==null)
        {
            return;
        }
        gameObject.SetActive(true);
        IsShow = true;
        if (_canvasGroup == null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        if (_canvasScaler == null)
        {
            _canvasScaler = GetComponent<CanvasScaler>();
        }

        if (_canvasScaler != null)
        {
            float value = (float) Screen.width / (float) Screen.height; 
            _canvasScaler.matchWidthOrHeight = value>0.57f ? 1 : 0;
        }

        _canvasGroup.interactable = false;
        _canvasGroup.alpha = 0;
        _canvasGroup.DOKill(true);
        _canvasGroup.alpha = 1;
        StartCoroutine(waitInteractable()); 
        OnPause();
    }

    private void Update()
    {
        diceText.text = LocalizationManger.Inst.GetText("Dice")+ ":" + BattleManager.Inst.RuntimeData.CurrentDice;
        text.text = BattleManager.Inst.CurrentGold.ToString();
        // _refreshBtn.SetActive(gameNpc.ItemGods != null && gameNpc.ItemGods.Count > 0);
    }

    public void CloseBtnClick()
    {
        UIManager.Inst.Close("ShopPanel");
    }


    // private Tweener colorTweener1;
    // private Tweener colorTweener2;
    // public void ShowTips(string str,Color color)
    // {
    //     tipsText.text = str;
    //     tipsText.color = color;
    //     if (colorTweener1!=null)
    //     {
    //         colorTweener1.Kill();
    //         DOTween.Kill(colorTweener1);
    //     }
    //     if (colorTweener2!=null)
    //     {
    //         colorTweener2.Kill();
    //         DOTween.Kill(colorTweener2);
    //     }
    //     colorTweener1 = tipsText.DOColor(Color.white, 0.3f).OnComplete(() =>
    //         {
    //             colorTweener2 = tipsText.DOColor(new Color(1,1,1,0), 0.3f);
    //         });
    // }
}
