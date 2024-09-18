using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceItemPanel : PanelBase
{
    [SerializeField]//当前选择的item
    private ReplaceItemPanel_ItemPrefab currentItemPrefab;
    [SerializeField]//当前拥有的item
    private ReplaceItemPanel_ItemPrefab _itemPrefab;
    [SerializeField]
    private Transform itemGroup;
    
    private Item currentItem;
    private Action<bool> callBack;
    public void OnOpen(Item item,Action<bool> callBack)
    {
        currentItem = item;
        this.callBack = callBack;
        currentItemPrefab.Init(item,false);
        
        var haveItemList = BattleManager.Inst.CurrentPlayer.roleItemController.GetItemsForItemType(item.ItemType);
        for (int i = 0; i < haveItemList.Count; i++)
        {
            GameObject.Instantiate(_itemPrefab,itemGroup).Init(haveItemList[i],true);
        }
    }

    public void AddItem()
    {
        BattleManager.Inst.CurrentPlayer.roleItemController.AddItem(currentItem,isOk=>{});
        UIManager.Inst.Close();
        callBack?.Invoke(true);
    }

    public void ClosePanel()
    {
        UIManager.Inst.Close();
        EventManager.Inst.DistributeEvent(EventName.OnReplaceItem);  
        callBack?.Invoke(false);
    }

    
    
    
}
