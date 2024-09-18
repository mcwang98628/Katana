using Sirenix.OdinInspector;

public enum TriggerType
{
    [LabelText("概率")]
    Probability,//概率
    [LabelText("计数")]
    Times,//计数
//    [LabelText("计时")]
//    Time,//计时
    
}
public enum ItemType
{
    [LabelText("主动道具")]
    Prop = 0,
    [LabelText("神器")]
    Artifact = 1,
    [LabelText("主动技能")]
    ButtonSkill = 2,
    [LabelText("角色技能")]
    CharacterSkill = 3,
    [LabelText("其他")]
    Other = 4,
    [LabelText("魂石")]
    SoulStone = 5,
    [LabelText("ALL")]
    All = 99,
}

//Item主动使用操作类型，Btn or Joy
public enum ItemUseType
{
    [LabelText("点击")]
    Click,
    [LabelText("长按")]
    Hold,
    [LabelText("拖拽摇杆")]
    Drag,
}

public enum RatioOrValue
{
    [LabelText("百分比")]
    Probability,
    [LabelText("固定值")]
    Value,
}