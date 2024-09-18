using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public partial class SROptions
{
    private int _itemId;


    [Category("Item相关")]
    public void AddItemById()
    {
        if (BattleManager.Inst.CurrentPlayer == null)
        {
            Debug.LogError("玩家角色不存在");
            return;
        }
        BattleManager.Inst.CurrentPlayer.roleItemController.AddItem(DataManager.Inst.ParsingItemObj(DataManager.Inst.GetItemScrObj(ItemId)),null);
        
    }
    
    [Category("Item相关")]
    public int ItemId
    {
        get
        {
            return _itemId;
        }
        set
        {
            _itemId = value;
        }
    }
}
