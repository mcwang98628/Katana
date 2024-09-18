using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "A_Data/AttackInfo")]
public class AttackInfo : ScriptableObject
{
    [InfoBox("注释：1就是原始攻击，0就无伤害，1.3就是1.3倍攻击力伤害")]
    [LabelText("Attack 倍率")]
    public float AttackMagnification = 1f;
    [LabelText("Attack 类型")]
    public AttackType AttackType = AttackType.Sector;
    [LabelText("Attack 距离/半径")]
    public float AttackRadius;
    [ShowIf("AttackType",AttackType.Sector)] [LabelText("Attack 角度")]
    public float AttackAngle;

    [InfoBox("注释：位移方向是基于玩家本地的方向")]
    [LabelText("使用 位移")]
    public bool isUseMove = false;
    [LabelText("方 向")] [ShowIf("isUseMove")]
    public Vector2 AttackMove;
    [LabelText("移 速")] [ShowIf("isUseMove")]
    public float MoveSpeed;
    [LabelText("时 间")] [ShowIf("isUseMove")]
    public float MoveTime;
    [LabelText("曲线")] [ShowIf("isUseMove")]
    public AnimationCurve MoveCurve = AnimationCurve.Linear(0,1,1,1);

    [InfoBox("注释：击退方向是基于玩家本地的方向")]
    [LabelText("使用 击退")] 
    public bool isUseRepel = false;
    [LabelText("击退 方 向")] [ShowIf("isUseRepel")]
    public Vector2 RepelMove;
    [LabelText("击退 移 速")] [ShowIf("isUseRepel")]
    public float RepelSpeed;
    [LabelText("击退 时 间")] [ShowIf("isUseRepel")]
    public float RepelTime;

    [LabelText("强力攻击")] 
    public bool isInterruption = false;
    
    [LabelText("使用 自动转向")] 
    public bool isUseAutoRotate = true;

    [LabelText("自动转向角度")] [ShowIf("isUseAutoRotate")]
    public float AutoRotateAng = 180f;
    
    [LabelText("是否播放声音")]
    public bool isPlayAudio;

    [LabelText("是否使用Buff")]
    public bool isUseBuff;
    [LabelText("Buff生命周期")][ShowIf("isUseBuff")]
    public BuffLifeCycle BuffLifeCycle;
    [LabelText("Buff")][ShowIf("isUseBuff")]
    public BuffScriptableObject BuffObj;

    [LabelText("目标受伤FeedBack")]
    public FeedBackObject HitFeedBack;
}

public enum AttackType
{
    [LabelText("圆 形")]
    Round = 1,//圆形
    [LabelText("扇 形")]
    Sector = 2,//扇形
}
