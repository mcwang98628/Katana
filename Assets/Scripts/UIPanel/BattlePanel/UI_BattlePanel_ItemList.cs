using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BattlePanel_ItemList : MonoBehaviour
{
    [SerializeField]
    private UI_BattlePanel_ItemPrefab itemPrefab;
    
    private List<UI_BattlePanel_ItemPrefab> _itemPrefabs = new List<UI_BattlePanel_ItemPrefab>();
    
    private void Awake()
    {
        EventManager.Inst.AddEvent(EventName.OnRoleAddItem,OnPlayerAddItem);
        EventManager.Inst.AddEvent(EventName.OnRoleReMoveItem,OnPlayerReMoveItem);
        EventManager.Inst.AddEvent(EventName.ShowBattlePanelItem,OnShowBattlePanelItem);
        
        if (BattleManager.Inst.CurrentPlayer != null)
        {
            foreach (Item item in BattleManager.Inst.CurrentPlayer.roleItemController.Items)
            {
                OnPlayerAddItem("",new RoleItemEventData(){Item = item,RoleId = BattleManager.Inst.CurrentPlayer.TemporaryId});
                EventManager.Inst.DistributeEvent(EventName.ShowBattlePanelItem,item);
            }
        }

    }
    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.ShowBattlePanelItem,OnShowBattlePanelItem);
        EventManager.Inst.RemoveEvent(EventName.OnRoleAddItem,OnPlayerAddItem);
        EventManager.Inst.RemoveEvent(EventName.OnRoleReMoveItem,OnPlayerReMoveItem);
    }

    private void OnShowBattlePanelItem(string arg1, object arg2)
    {
        Item item = (Item) arg2;
        var go = GetItemPrefab(item.TemporaryId);
        if (go!=null)
        {
            go.gameObject.SetActive(true);
        } 
    }


    UI_BattlePanel_ItemPrefab GetItemPrefab(int id)
    {
        for (int i = 0; i < _itemPrefabs.Count; i++)
        {
            if (_itemPrefabs[i].ItemData == null)
            {
                continue;
            }
            
            if (_itemPrefabs[i].ItemData.ID == id)
            {
                return _itemPrefabs[i];
            }
        }
        return null;
    }
    UI_BattlePanel_ItemPrefab GetItemPrefab(string tId)
    {
        for (int i = 0; i < _itemPrefabs.Count; i++)
        {
            if (_itemPrefabs[i].ItemData == null)
            {
                continue;
            }
            
            if (_itemPrefabs[i].ItemData.TemporaryId == tId)
            {
                return _itemPrefabs[i];
            }
        }
        return null;
    }

    UI_BattlePanel_ItemPrefab CreateItemPrefab()
    {
        var item = GameObject.Instantiate(itemPrefab, transform);
        _itemPrefabs.Add(item);
        return item;
    }

    private void OnPlayerAddItem(string eventName, object itemEventData)
    {
        RoleItemEventData ried = (RoleItemEventData) itemEventData;
        if (ried.Item.ID == -1)
            return;
        var item = DataManager.Inst.GetItemScrObj(ried.Item.ID);
        if (item == null || (item.ItemType != ItemType.Artifact && item.ItemType != ItemType.SoulStone))
        {
            return;
        }
        if (ried.RoleId != BattleManager.Inst.CurrentPlayer.TemporaryId)
        {
            return;
        }


        UI_BattlePanel_ItemPrefab itemObj=null;
        if (item.ItemType == ItemType.Artifact)
        {
            itemObj = CreateItemPrefab();
            itemObj.Init(ried.Item);
            
            if (itemObj!=null)//TODO 
            {
                itemObj.gameObject.SetActive(false);
            }
        }
        else if (item.ItemType == ItemType.SoulStone)
        {
            itemObj = GetItemPrefab(ried.Item.ID);
            if (itemObj != null)
            {
                itemObj.UpdateNumber();
            }
            else
            {
                itemObj = CreateItemPrefab();
                itemObj.Init(ried.Item);
                itemObj.transform.SetSiblingIndex(0);
                if (itemObj!=null)//TODO 
                {
                    itemObj.gameObject.SetActive(false);
                }
            }
        }

    }
    private void OnPlayerReMoveItem(string eventName, object itemEventData)
    {
        RoleItemEventData ried = (RoleItemEventData) itemEventData;
        if (ried.RoleId != BattleManager.Inst.CurrentPlayer.TemporaryId || 
            ried.Item.ItemType != ItemType.Artifact)
        {
            return;
        }

        var itemPrefab = GetItemPrefab(ried.Item.TemporaryId);
        _itemPrefabs.Remove(itemPrefab);
        GameObject.Destroy(itemPrefab.gameObject); 
    }
}
