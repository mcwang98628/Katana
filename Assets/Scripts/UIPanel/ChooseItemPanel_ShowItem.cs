using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ChooseItemPanel_ShowItem : MonoBehaviour
{
    [SerializeField]
    private UI_NPCTalkPanel_TalkItem_ItemInfo itemInfo;
    [SerializeField] 
    private Transform group;
    [SerializeField]
    private Text buttonText;
    [SerializeField]
    private int skipItemId;
    
    private List<UI_NPCTalkPanel_TalkItem_ItemInfo> itemPrefabList = new List<UI_NPCTalkPanel_TalkItem_ItemInfo>();
    private ItemPoolScriptableObject itemPool;
    private Action<Item> HideTalkCallBack;

    private List<ItemScriptableObject> itemList;
    
    //
    // public void ShowItem(ItemPoolScriptableObject itemPool, Action callBack)
    // {
    //     gameObject.SetActive(true);
    //     for (int i = 0; i < group.childCount; i++)
    //     {
    //         Destroy(group.GetChild(0).gameObject);
    //     }
    //     HideTalkCallBack = callBack;
    //
    //     itemList = RandomItemTool.RandomItems(itemPool.Items,3);
    //     
    //     itemPrefabList.Clear();
    //     StartCoroutine(DelayInitItem(0.15f));
    //
    //     int goldValue = DataManager.Inst.GetItemScrObj(skipItemId).effects[0].effectData.GoldValue;
    //     //int goldValue = 20;
    //     buttonText.text = LocalizationManger.Inst.GetText("Skip")  +"  +"+goldValue+"G";
    //     //GameManager.Inst.StartCoroutine(anim(levelUpItemPool,lv));
    // }

    private List<ItemPoolData> _showItemList = new List<ItemPoolData>();
    public void ShowItem(List<ItemPoolData> itemPool,int count,Action<Item> callBack)
    {
        HideTalkCallBack = callBack;
        _showItemList.Clear();


        List<ItemPoolData> removeList = new List<ItemPoolData>();
        foreach (var poolData in itemPool)
        {
            if (poolData.Item.isUnique)
            {
                int itemId = DataManager.Inst.GetItemId(poolData.Item);
                if (BattleManager.Inst.CurrentPlayer.roleItemController.IsHaveItem(itemId))
                    removeList.Add(poolData);
            }
        }
        foreach (var poolData in removeList)
            itemPool.Remove(poolData);
        
        
        for (int i = 0; i < count; i++)
        {
            int randomMaxValue = 0;
            foreach (var poolData in itemPool)
            {
                if (poolData.Item.isUnique)
                {
                    int itemId = DataManager.Inst.GetItemId(poolData.Item);
                    if (BattleManager.Inst.CurrentPlayer.roleItemController.IsHaveItem(itemId))
                    {
                        continue;
                    }
                }
                randomMaxValue += poolData.ProbabilityWeight;
            }
            int randomValue = Random.Range(0, randomMaxValue+1);
            int weight = 0;
            foreach (var poolData in itemPool)
            {
                weight += poolData.ProbabilityWeight;
                if (randomValue <= weight)
                {
                    _showItemList.Add(poolData);
                    itemPool.Remove(poolData);
                    break;
                }
            }
        }

        for (int i = 0; i < itemPrefabList.Count; i++)
        {
            GameObject.Destroy(itemPrefabList[i].gameObject);
        }
        itemPrefabList.Clear();
        foreach (ItemPoolData poolData in _showItemList)
        {
            UI_NPCTalkPanel_TalkItem_ItemInfo info = Instantiate(itemInfo, group);
            Item item = DataManager.Inst.ParsingItemObj(poolData.Item);
            info.Init(item, HideTalkCallBack);
            itemPrefabList.Add(info);
        }
    }
    
    IEnumerator DelayInitItem(float interval)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            //创建item info
            UI_NPCTalkPanel_TalkItem_ItemInfo info = Instantiate(itemInfo, group);
            Item item = DataManager.Inst.ParsingItemObj(itemList[i]);
            info.Init(item, HideTalkCallBack);
            itemPrefabList.Add(info);
            yield return new WaitForSecondsRealtime(interval);
        }
    }
    
    public void OnSkipBtnClick()
    {
        EventManager.Inst.DistributeEvent(TGANames.BattleTreasureChestSkip);
        BattleManager.Inst.CurrentPlayer.roleItemController.AddItem( DataManager.Inst.ParsingItemObj(DataManager.Inst.GetItemScrObj(skipItemId)),
            isOk =>
            {
                if (isOk)
                {
                    if (HideTalkCallBack != null)
                    {
                        HideTalkCallBack(null);
                    }
                    EventManager.Inst.DistributeEvent(EventName.OnNpcTalkYesOrNoEvent, new NpcTalkYesOrNoEventData(-1,true));
                }
            } );
    }

    /*
    IEnumerator anim(LevelUpItemPool levelUpItemPool, int lv)
    {
        var canvasgroup = transform.parent.GetComponent<CanvasGroup>();
      

        foreach (var itemInfo in itemPrefabList)
        {
            itemInfo.Init(currentLevelUpItemPool.LevelUpItemList[Random.Range(0, currentLevelUpItemPool.LevelUpItemList.Count)]);
            canvasgroup.interactable = false;
            itemInfo.transform.localScale = Vector3.zero;
            itemInfo.transform.DOScale(Vector3.one, 0.2f);
            //yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.1f);
        List<ItemScriptableObject> ItemList = levelUpItemPool.GetItemsAt(lv);
        for (int i = 0; i < ItemList.Count; i++)
        {
            Item item = DataManager.Inst.ParsingItemObj(ItemList[i]);
            itemPrefabList[i].Init(item,HideTalkCallBack);
        }
        canvasgroup.interactable = true;
    }
    */
    
}
