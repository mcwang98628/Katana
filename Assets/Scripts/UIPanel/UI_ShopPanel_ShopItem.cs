using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopPanel_ShopItem : MonoBehaviour
{
    public UI_ShopPanel rootPanel;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private UIText itemName;
    [SerializeField]
    private Text Price;
    
    [SerializeField]
    private UIText Desc;

    [SerializeField]
    private AnimationCurve _curve;
    private ShopItem itemData;
    private InteractObj_NPC _npc;
    public void Init(InteractObj_NPC npc,ShopItem item)
    {
        gameObject.SetActive(true);
        _npc = npc;
        itemData = item;
        icon.sprite = itemData.Item.Icon;
        itemName.text = itemData.Item.Name;
        itemName.Te.color =  Item.GetColor(itemData.Item.itemColorType);
        if(Desc!=null)
            Desc.text = item.Item.Describe;
        
        gameObject.SetActive(item.Number>0);
        Price.text = item.Price + " G";
    }
    
    private InteractObj_NPC gameNpc;
    public void SetNPC(InteractObj_NPC npc)
    {
        gameNpc = npc;
    }

    public void OnBtnClick()
    {
        if (itemData != null && itemData.Item != null)
        {
            if (CheckBuy(itemData))
            {
                Item item = DataManager.Inst.ParsingItemObj(itemData.Item);
                BattleManager.Inst.CurrentPlayer.roleItemController.AddItem(item, isOk =>
                {
                    if (isOk)
                    {
                        Buy(itemData);
                        gameNpc.DeleteGoods(itemData);
                        // rootPanel.ShowTips(LocalizationManger.Inst.GetText("ShopPanel_1"),new Color(0,0,0,0));
                        UIManager.Inst.Tips.ShowText(LocalizationManger.Inst.GetText("ShopPanel_1"));
                        icon.transform.DOScale(Vector3.one*1.4f, 0.3f).SetEase(_curve).OnComplete(() =>
                        {
                            gameObject.SetActive(itemData.Number>0);
                        });
                    }
                });
                EventManager.Inst.DistributeEvent(TGANames.BattleShopBuyItem,item.ID);
            }
            else
            {
                return;
            }
        }
    }

    public bool CheckBuy(ShopItem shopGods)
    {
        if (shopGods.Number <= 0)
        {
            // rootPanel.ShowTips(LocalizationManger.Inst.GetText("ShopPanel_2"),Color.red);
            UIManager.Inst.Tips.ShowText(LocalizationManger.Inst.GetText("ShopPanel_2"));
            // Debug.LogError("没有库存。");
            return false;
        }
        if (shopGods != null)
        {
            if (BattleManager.Inst.CurrentGold >= shopGods.Price)
            {
                return true;
            }
            else
            {
                // rootPanel.ShowTips(LocalizationManger.Inst.GetText("ShopPanel_3"),Color.red);
                UIManager.Inst.Tips.ShowText(LocalizationManger.Inst.GetText("ShopPanel_3"));
                return false;
            }
        }
        return false;
    }
    
    public bool Buy(ShopItem shopGods)
    {
        if (shopGods.Number <= 0)
        {
            // rootPanel.ShowTips(LocalizationManger.Inst.GetText("ShopPanel_2"),Color.red);
            UIManager.Inst.Tips.ShowText(LocalizationManger.Inst.GetText("ShopPanel_2"));
            return false;
        }
        if (shopGods != null)
        {
            if (BattleManager.Inst.CurrentGold >= shopGods.Price)
            {
                BattleManager.Inst.AddGold(-shopGods.Price);
                return true;
            }
            else
            {
                // rootPanel.ShowTips(LocalizationManger.Inst.GetText("ShopPanel_3"),Color.red);
                UIManager.Inst.Tips.ShowText(LocalizationManger.Inst.GetText("ShopPanel_3"));
                return false;
            }
        }
        return false;
    }
    
}
