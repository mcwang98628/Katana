using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class UI_YesOrNoPanel : PanelBase
{
    public UIText Title;
    public UIText Desc;

    public Image Icon;
    public UI_YesOrNoPanel_ItemPrefab ItemPrefab;

    private Action<bool> callBack;
    
    public void OnOpen(string title,string desc,Sprite icon,int itemId,Action<bool> callBack)
    {
        Title.text = title;
        Desc.gameObject.SetActive(!string.IsNullOrEmpty(desc));
        Desc.text = desc;
        Icon.gameObject.SetActive(icon!=null);
        Icon.sprite = icon;
        ItemPrefab.gameObject.SetActive(itemId>0);
        if (itemId>0)
            ItemPrefab.Init(DataManager.Inst.GetItemScrObj(itemId));
        this.callBack = callBack;
    }
    public void OnOpen(string title,string desc,Action<bool> callBack)
    {
        this.OnOpen(title, desc, null, -1, callBack);
    }

    public void OnOpen(string title, string desc, Sprite icon, Action<bool> callBack)
    {
        this.OnOpen(title, desc, icon, -1, callBack);
    }

    public void OnOpen(string title, string desc, int itemId, Action<bool> callBack)
    {
        this.OnOpen(title, desc, null, itemId, callBack);
    }

    public void OnBtnClick(bool isOk)
    {
        this.callBack?.Invoke(isOk);
        UIManager.Inst.Close();
    }
}
