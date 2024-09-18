using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UI_NPCTalkPanel : PanelBase
{
    [SerializeField]
    private UI_NPCTalkPanel_TalkText textPanel;
    // [SerializeField]
    // private UI_NPCTalkPanel_TalkItem itemPanel;
    // [SerializeField]
    // private UI_NPCTalkPanel_TalkYesOrNo yesOrNoPanel;

    [SerializeField]
    private UIText NpcNames;

    private NPCTalkObject npcTalkObject;

    private InteractObj_NPC gameNpc;
    public void OnOpen(NPCTalkObject TalkObject, InteractObj_NPC npc)
    {
        gameNpc = npc;
        Init(TalkObject);
        NpcNames.text = TalkObject.NPCTalkName;
        base.Show();
    }

    private void Awake()
    {
        EventManager.Inst.AddEvent(EventName.OnNpcTalkYesOrNoEvent,OnTalkYesOrNo);
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnNpcTalkYesOrNoEvent,OnTalkYesOrNo);
    }

    private void OnTalkYesOrNo(string arg1, object arg2)
    {
        
        var eventData = (NpcTalkYesOrNoEventData)arg2;
        talkBool = eventData.IsOk;
    }


    public override void Show()
    {        if (gameObject==null)
        {
            return;
        }
        gameObject.SetActive(true);
        IsShow = true;
        if (_canvasGroup == null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        if (_canvasScaler == null)
        {
            _canvasScaler = GetComponent<CanvasScaler>();
        }

        if (_canvasScaler != null)
        {
            float value = (float) Screen.width / (float) Screen.height; 
            _canvasScaler.matchWidthOrHeight = value>0.57f ? 1 : 0;
        }

        _canvasGroup.interactable = false;
        _canvasGroup.alpha = 0;
        _canvasGroup.DOKill(true);
        _canvasGroup.alpha = 1;
        UIManager.Inst.StartCoroutine(waitInteractable()); 
        OnPause();
    }

    protected override void OnUnPause() { }
    protected override void OnPause() { }
    

    public void OnReShow()
    {
        NextTalkPanel();
    }

    private void Init(NPCTalkObject talkObject)
    {
        npcTalkObject = talkObject;
        if (npcTalkObject.TalkList.Count == 0)
        {
            Debug.LogError("TalkDataError！！！！");
            return;
        }
        currentTalkIndex = 0;
        NextTalkPanel();
    }

    private bool talkBool = true;
    private int currentTalkIndex = 0;
    private bool repeatEnd = false;
    void NextTalkPanel()
    {
        //触发复读机对话的情况
        if (!gameNpc.CanRepeat && gameNpc.Talked)
        {
            if (repeatEnd)
            {
                repeatEnd = false;
                ClosePanel();
                return;
            }
            else
            {
                textPanel.ShowText(npcTalkObject.GetRepeatTalk(), NextTalkPanel);
                repeatEnd = true;
            }

            return;
        }

        //触发正常对话的情况
        //对话用完，结束！
        if (currentTalkIndex >= npcTalkObject.TalkList.Count)
        {
            ClosePanel();
            return;
        }
        TalkData data = npcTalkObject.TalkList[currentTalkIndex];
        currentTalkIndex++;
        switch (data.TalkType)
        {
            case NpcTalkType.Text:
                if (talkBool)
                {
                    textPanel.ShowText(data.TalkText, NextTalkPanel);
                }
                else
                {
                    textPanel.ShowText(data.FalseTalkText, NextTalkPanel);
                    currentTalkIndex = 999;
                }
                break;
            case NpcTalkType.Item:
                Debug.LogError("功能已被弃用，有疑问点击Log查看代码。");
                Debug.LogError("功能已被弃用，有疑问点击Log查看代码。");
                Debug.LogError("功能已被弃用，有疑问点击Log查看代码。");
                // itemPanel.ShowItem(data.DropItemPool, data.DropItemCount, isOk =>
                //  {
                //      talkBool = isOk;
                //      NextTalkPanel();
                //  });
                break;
            case NpcTalkType.ItemShop:
                gameNpc.RefreshFunc = () =>
                {
                    int count = 0;

                    gameNpc.IsRefresh = true;
                    count = data.ShopGoodsCount;
                            
                    gameNpc.SetStoreGoods(RandomItemTool.RandomShopGod(count));
                };
                if ((gameNpc.ItemGods == null || gameNpc.ItemGods.Count == 0) && !gameNpc.IsRefresh)
                    gameNpc.RefreshFunc?.Invoke();
                textPanel.ShowText(data.ShopTalkText, () =>
                {
                    UIManager.Inst.Open("ShopPanel",true, gameNpc);
                });
                break;
            case NpcTalkType.YesOrNo:
                UIManager.Inst.Open("YesOrNoPanel",true,npcTalkObject.NPCTalkName,data.YesOrNoText,new Action<bool>(isOk =>
                {
                    talkBool = isOk;
                    NpcTalkYesOrNoEventData talkEventData = new NpcTalkYesOrNoEventData(npcTalkObject.NpcTalkId, isOk);
                    EventManager.Inst.DistributeEvent(EventName.OnNpcTalkYesOrNoEvent, talkEventData);
                }));
                break;
            case NpcTalkType.Synthetic:
                Debug.LogError("功能已被弃用，有疑问点击Log查看代码。");
                Debug.LogError("功能已被弃用，有疑问点击Log查看代码。");
                Debug.LogError("功能已被弃用，有疑问点击Log查看代码。");
                // ItemReplaceFormulas.ReplaceFormula paneldata;
                // if (gameNpc.ReplaceFormula == null)
                // {
                //     paneldata = data.ItemReplaceFormulas.GetReplaceFormula();
                //     gameNpc.SetSynthetic(paneldata);
                // }
                // else
                // {
                //     paneldata = gameNpc.ReplaceFormula;
                // }
                //  
                // if (paneldata == null)
                // {
                //     NextTalkPanel();
                // }
                // else
                // {
                //     UIManager.Inst.Open("GiveItemPanel",true, paneldata, npcTalkObject.NpcTalkId, npcTalkObject.NPCTalkName, data.SyntheticText);
                // }
                break;
            case NpcTalkType.GiveItem:
                ItemScriptableObject item;
                if (gameNpc.GiveItem == null)
                {
                    item = data.GiveItemPool.Items[Random.Range(0, data.GiveItemPool.Items.Count)];
                    gameNpc.SetGiveItem(item);
                }
                else
                {
                    item = gameNpc.GiveItem;
                }
                if (item == null)
                {
                    NextTalkPanel();
                }
                else
                {
                    UIManager.Inst.Open("YesOrNoPanel", true, npcTalkObject.NPCTalkName, data.GiveText,
                        DataManager.Inst.GetItemId(item), new Action<bool>(isOk =>
                        {
                            talkBool = isOk;
                            NpcTalkYesOrNoEventData talkEventData = new NpcTalkYesOrNoEventData(npcTalkObject.NpcTalkId, isOk);
                            EventManager.Inst.DistributeEvent(EventName.OnNpcTalkYesOrNoEvent, talkEventData);
                            
                            if (isOk)
                            {
                                BattleManager.Inst.CurrentPlayer.roleItemController.AddItem(DataManager.Inst.ParsingItemObj(item), isOk2 =>
                                {
                                    
                                });
                            }
                            else
                            {
                                NpcTalkYesOrNoEventData eventData = new NpcTalkYesOrNoEventData(npcTalkObject.NpcTalkId, isOk);
                                EventManager.Inst.DistributeEvent(EventName.OnNpcTalkYesOrNoEvent, eventData);
                            }
                        }));
                }
                break;
            case NpcTalkType.DeductGold: 
                UIManager.Inst.Open("YesOrNoPanel",true,npcTalkObject.NPCTalkName,data.DeductGoldText,new Action<bool>(isOk =>
                {
                    talkBool = isOk;
                    NpcTalkYesOrNoEventData talkEventData = new NpcTalkYesOrNoEventData(npcTalkObject.NpcTalkId, talkBool);
                    EventManager.Inst.DistributeEvent(EventName.OnNpcTalkYesOrNoEvent, talkEventData);
                    if (isOk)
                    {
                        talkBool = BattleManager.Inst.CurrentGold >= data.GoldValue;
                        if (talkBool)
                        {
                            BattleManager.Inst.AddGold(-data.GoldValue);
                        }
                    }
                }));
                break;
            case NpcTalkType.BattleArchive:
                ArchiveManager.Inst.SaveBattleArchive();
                NextTalkPanel();
                break;
        }

    }


    void ClosePanel()
    {
        UIManager.Inst.Close("NPCTalkPanel");
        gameNpc.InteractEnd();
        EventManager.Inst.DistributeEvent(EventName.OnNpcTalkOver, npcTalkObject.NpcTalkId);
    }


}

public class NpcTalkYesOrNoEventData
{
    public int NPCTalkID;
    public bool IsOk;
    public NpcTalkYesOrNoEventData(int npcTalkID, bool isOk)
    {
        NPCTalkID = npcTalkID;
        IsOk = isOk;
    }
}