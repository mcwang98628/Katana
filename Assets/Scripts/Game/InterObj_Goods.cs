// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using DG.Tweening;
//
// public class InterObj_Goods : InteractObj
// {
//     [SerializeField]
//     private Transform sphere;
//     [SerializeField]
//     private TextMesh itemName;
//     [SerializeField]
//     private TextMesh price;
//     [SerializeField]
//     private SpriteRenderer icon;
//     [SerializeField]
//     private SpriteRenderer priceIcon;
//
//     [SerializeField]
//     private Sprite hp;
//     [SerializeField]
//     private Sprite gold;
//
//     private ShopItem goods;
//
//     private Action BuyCallBack;
//     public void Init(ShopItem shopGods,Action buyCallBack)
//     {
//         goods = shopGods;
//         BuyCallBack = buyCallBack;
//         gameObject.SetActive(shopGods.Number>0);
//         itemName.text = shopGods.Item.Name;
//         price.text = shopGods.TruePrice + "";
//         icon.sprite = shopGods.Item.Icon;
//         switch (shopGods.PriceType)
//         {
//             case PriceType.Gold:
//                 priceIcon.sprite = gold;
//                 break;
//             case PriceType.Item:
//                 priceIcon.sprite = shopGods.PriceItem.Icon;
//                 break;
//             case PriceType.HP:
//                 priceIcon.sprite = hp;
//                 break;
//             
//         }
//         gameObject.SetActive(true);
//         sphere.DOScale(Vector3.one, 0.2f);
//     }
//     public override void InteractStart()
//     {
//         // base.InteractStart();
//         OnBuy();
//     }
//     
//     
//     
//     void OnBuy()
//     {
//         if (goods != null && goods.Item != null)
//         {
//             if (CheckBuy(goods))
//             {
//                 Item item = DataManager.Inst.ParsingItemObj(goods.Item);
//                 BattleManager.Inst.CurrentPlayer.roleItemController.AddItem(item);
//                 // gameObject.SetActive(false); //itemData.Number>0);
//                 //Destroy(gameObject);
//                 DisableSphere();
//                 if (BuyCallBack!=null)
//                 {
//                     BuyCallBack.Invoke();
//                 }
//             }
//         }
//     }
//     public void DisableSphere()
//     {
//         sphere.DOScale(Vector3.zero, 0.4f);
//         SetCanInteract(false);
//     }
//     
//
//     bool CheckBuy(ShopGodsBase shopGods)
//     {
//         if (shopGods.Number <= 0)
//         {
//             UIManager.Inst.Tips.ShowText("没有库存。");
//             return false;
//         }
//         if (shopGods != null)
//         {
//             switch (shopGods.PriceType)
//             {
//                 case PriceType.Gold:
//                     if (BattleManager.Inst.CurrentGold >= shopGods.TruePrice)
//                     {
//                         BattleManager.Inst.AddGold(-shopGods.TruePrice);
//                         return true;
//                     }
//                     else
//                     {
//                         UIManager.Inst.Tips.ShowText("金币不够。");
//                         // Debug.LogError("金币不够。");
//                         return false;
//                     }
//                 case PriceType.Item:
//                     int guid = DataManager.Inst.GetItemId(shopGods.PriceItem);
//                     if (BattleManager.Inst.CurrentPlayer.roleItemController.GetItemCount(guid) >= shopGods.Price)
//                     {
//                         BattleManager.Inst.CurrentPlayer.roleItemController.ReMoveItemByID(guid, shopGods.Price);
//                         return true;
//                     }
//                     else
//                     {
//                         UIManager.Inst.Tips.ShowText("item不够。");
//                         // Debug.LogError("item不够。");
//                         return false;
//                     }
//                 case PriceType.HP:
//                     if (BattleManager.Inst.CurrentPlayer.CurrentHp > shopGods.Price)
//                     {
//                         DamageInfo dmg = new DamageInfo(BattleManager.Inst.CurrentPlayer.TemporaryId,shopGods.Price,
//                             BattleManager.Inst.CurrentPlayer,
//                             BattleManager.Inst.CurrentPlayer.transform.position);
//
//                         BattleManager.Inst.CurrentPlayer.HpInjured(dmg);
//                         return true;
//                     }
//                     else
//                     {
//                         UIManager.Inst.Tips.ShowText("血量不够。");
//                         // Debug.LogError("血量不够。");
//                         return false;
//                     }
//                 default:
//                     break;
//             }
//         }
//         return false;
//     }
//
// }
