using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionType
{
    None,
    GoldGreater,//大于多少金币
    HpPercentageGreater,//大于多少Hp百分比 0-1f
    HpPercentageLess,//小于多少Hp百分比 0-1f
    LevelIndexGreater,//大于多少关
    HaveItemList,//拥有这些item
}

public struct ConditionInfo
{
    public int Id;
    public string RoomName;
    public RoomType RoomType;
    public string ContentFileName;
    public ConditionType ConditionType;
    public string ConditionValue;
    public bool IsOnly;
}

