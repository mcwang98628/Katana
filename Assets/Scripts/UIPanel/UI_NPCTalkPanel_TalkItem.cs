// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Random = UnityEngine.Random;
//
// public class UI_NPCTalkPanel_TalkItem : MonoBehaviour
// {
//     [SerializeField]
//     private UI_NPCTalkPanel_TalkItem_ItemInfo itemInfo;
//     [SerializeField] 
//     private Transform group;
//
//     private Action<bool> HideTalkCallBack;
//     public void ShowItem(ItemPoolScriptableObject pool,int itemCount,Action<bool> callBack)
//     {
//         gameObject.SetActive(true);
//         for (int i = 0; i < group.childCount; i++)
//         {
//             Destroy(group.GetChild(0).gameObject);
//         }
//         HideTalkCallBack = callBack;
//         RandomItem(pool,itemCount);
//     }
//
//     //升级用的接口
//     public void ShowItem(LevelUpItemPool levelUpItemPool, int lv, Action<bool> callBack)
//     {
//         gameObject.SetActive(true);
//         for (int i = 0; i < group.childCount; i++)
//         {
//             Destroy(group.GetChild(0).gameObject);
//         }
//         HideTalkCallBack = callBack;
//
//         List<ItemScriptableObject> ItemList = levelUpItemPool.GetItemsAt(lv);
//         int itemCount = ItemList.Count;
//         
//         for (int i = 0; i < itemCount; i++)
//         {
//             Item item = DataManager.Inst.ParsingItemObj(ItemList[i]);
//
//             //创建item info
//             UI_NPCTalkPanel_TalkItem_ItemInfo info = Instantiate(itemInfo, group);
//             info.Init(item, OnHide);
//         }
//     }
//
//     void RandomItem(ItemPoolScriptableObject pool,int itemCount)
//     {
//         List<int> indexs = new List<int>(pool.Items.Count);
//         for (int i = 0; i < pool.Items.Count; i++)
//         {
//             indexs.Add(i);
//         }
//         
//         for (int i = 0; i < itemCount; i++)
//         {
//             if (indexs.Count == 0)
//             {
//                 break;
//             }
//             int index = indexs[Random.Range(0, indexs.Count)];
//             indexs.Remove(index);
//             bool isHave = BattleManager.Inst.CurrentPlayer.roleItemController.IsHaveItem(DataManager.Inst.GetItemId(pool.Items[index]));
//             if (isHave && pool.Items[index].isUnique)
//             {
//                 // Debug.LogError("有重复的Item");
//                 i--;
//                 continue;
//             }
//             Item item = DataManager.Inst.ParsingItemObj(pool.Items[index]);
//
//             //创建item info
//             UI_NPCTalkPanel_TalkItem_ItemInfo info = Instantiate(itemInfo, group);
//             info.Init(item,OnHide);
//         }
//     }
//
//     public void OnClose()
//     {
//         if (HideTalkCallBack != null)
//         {
//             HideTalkCallBack(false);
//         }
//         gameObject.SetActive(false);
//     }
//     void OnHide()
//     {
//         if (HideTalkCallBack != null)
//         {
//             HideTalkCallBack(true);
//         }
//         gameObject.SetActive(false);
//     }
//     
// }
