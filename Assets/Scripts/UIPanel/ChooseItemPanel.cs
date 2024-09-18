using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class ChooseItemPanel : PanelBase
{
    [SerializeField]
    private ChooseItemPanel_ShowItem itemPanel;
    [SerializeField]
    private Text diceText;
    //[SerializeField]
    //private Text text;
    //[SlizeField]
    //private Text PanelName;


    private List<UI_ShopPanel_ShopItem> shopItemList = new List<UI_ShopPanel_ShopItem>();
    private ItemPool _itemPool;
    private ItemPoolType _itemPoolType;
    private int _itemCount;
    private Action<Item> _closeCallBack;
    public void OnOpen(ItemPool pool,ItemPoolType itemPoolType,int itemCount,Action<Item> closeCallBack)
    {
        _itemPool = pool;
        _itemPoolType = itemPoolType;
        _itemCount = itemCount;
        _closeCallBack = closeCallBack;
        List<ItemPoolData> itemPoolDatas = null;
        switch (itemPoolType)
        {
            case ItemPoolType.Lv1:
                itemPoolDatas = new List<ItemPoolData>(pool.Lv1);
                break;
            case ItemPoolType.Lv2:
                itemPoolDatas = new List<ItemPoolData>(pool.Lv2);
                break;
            case ItemPoolType.Lv3:
                itemPoolDatas = new List<ItemPoolData>(pool.Lv3);
                break;
            case ItemPoolType.Shop:
                itemPoolDatas = new List<ItemPoolData>(pool.Shop);
                break;
        }
        itemPanel.ShowItem(itemPoolDatas,itemCount, ClosePanel);
    }
    public void OnOpen()
    {
        // DataManager.Inst.GetCpData(BattleManager.Inst.RuntimeData.CurrentChapterId).LoadChapterData(
        //     delegate(ChapterData data)
        //     {
        //         itemPanel.ShowItem(
        //             data.ItemData.ArtifactPool,
        //             ClosePanel
        //             );
        //     });
    }

    private void Update()
    {
        if (diceText == null)
            return;
        diceText.text = LocalizationManger.Inst.GetText("Dice") + ":" + BattleManager.Inst.RuntimeData.CurrentDice;
    }

    public void OnRefreshItem()
    {
        if (BattleManager.Inst.RuntimeData.CurrentDice < 1)
        {
            UIManager.Inst.Tips.ShowText(LocalizationManger.Inst.GetText("NotEnoughDice"));
            return;
        }
        BattleManager.Inst.RuntimeData.AddDice(-1);
        OnOpen(_itemPool,_itemPoolType,_itemCount,_closeCallBack);
    }
    public void ClosePanel(Item item)
    {
        UIManager.Inst.Close("ChooseItemPanel");
        _closeCallBack?.Invoke(item);
    }
    
}
