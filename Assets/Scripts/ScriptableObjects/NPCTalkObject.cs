using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "NPC/Talk")]
public class NPCTalkObject : ScriptableObject
{
    [FormerlySerializedAs("NpcId")] public int NpcTalkId;
    public string NPCTalkName;
    public List<TalkData> TalkList = new List<TalkData>();
    public List<String> RepeatTalk = new List<String>();
    private int lastRepeatTalkID=-1;
    public String GetRepeatTalk()
    {
        if (RepeatTalk.Count <= 0)
            return null;
        int index=lastRepeatTalkID+1<RepeatTalk.Count?lastRepeatTalkID+1:0;
        lastRepeatTalkID=index;
        return RepeatTalk[index];
    }
}

[Serializable]
public class TalkData
{
    [LabelText("对话类型")]
    public NpcTalkType TalkType;
    [LabelText("对话文本string")]
    [ShowIf("TalkType", NpcTalkType.Text)]
    public string TalkText;
    [LabelText("False对话文本string")]
    [ShowIf("TalkType", NpcTalkType.Text)]
    public string FalseTalkText;
    [LabelText("物品奖励池")]
    [ShowIf("TalkType", NpcTalkType.Item)]
    public ItemPoolScriptableObject DropItemPool;
    [LabelText("奖励个数")]
    [ShowIf("TalkType", NpcTalkType.Item)]
    public int DropItemCount;

    
    [LabelText("对话文本string")]
    [ShowIf("TalkType", NpcTalkType.ItemShop)]
    public string ShopTalkText;
    
    
    [Space]
    [LabelText("商店物品数量")]
    [ShowIf("TalkType", NpcTalkType.ItemShop)]
    public int ShopGoodsCount;
    [LabelText("商店类型")]
    [ShowIf("TalkType", NpcTalkType.ItemShop)]
    public ShopType ShopType;

    [LabelText("对话文本")]
    [ShowIf("TalkType", NpcTalkType.YesOrNo)]
    public string YesOrNoText;


    [LabelText("合成对话文本")]
    [ShowIf("TalkType", NpcTalkType.Synthetic)]
    public string SyntheticText;
    [LabelText("合成...")]
    [ShowIf("TalkType", NpcTalkType.Synthetic)]
    public ItemReplaceFormulas ItemReplaceFormulas;
    
    [LabelText("白给 对话文本")]
    [ShowIf("TalkType", NpcTalkType.GiveItem)]
    public string GiveText;
    [LabelText("白给道具...")]
    [ShowIf("TalkType", NpcTalkType.GiveItem)]
    public ItemPoolScriptableObject GiveItemPool;
    
    
    [LabelText("扣钱...")]
    [ShowIf("TalkType", NpcTalkType.DeductGold)]
    public int GoldValue;
    [LabelText("对话文本")]
    [ShowIf("TalkType", NpcTalkType.DeductGold)]
    public string DeductGoldText;
}

public enum NpcTalkType
{
    [LabelText("对话 文本")]
    Text,
    [LabelText("奖励 道具")]
    Item,
    [LabelText("商店")]
    ItemShop,
    YesOrNo,
    [LabelText("合成")]
    Synthetic,
    [LabelText("白给道具")]
    GiveItem,
    [LabelText("扣钱")]
    DeductGold,
    [LabelText("战斗存档")]
    BattleArchive,
    
}
