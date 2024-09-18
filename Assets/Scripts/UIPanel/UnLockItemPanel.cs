using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnLockItemPanel : PanelBase
{

    [SerializeField]
    private UnLockItemPanel_ItemPrefab itemPrefab;

    [SerializeField]
    private Transform itemGroup;

    private List<int> itemIds = new List<int>();
    private Action callBack;
    public void OnOpen(List<int> itemIds,Action callBack)
    {
        this.callBack = callBack;
        this.itemIds.AddRange(itemIds);
        for (int i = 0; i < itemIds.Count; i++)
        {
            GameObject.Instantiate(itemPrefab,itemGroup).Init(DataManager.Inst.GetItemScrObj(itemIds[i]));
        }

    }

    
    public void ClosePanel()
    {
        UIManager.Inst.Close("UnLockItemPanel");
        callBack?.Invoke();
    }
}
