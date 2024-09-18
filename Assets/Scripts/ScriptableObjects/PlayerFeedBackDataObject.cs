using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "FeedBack/Player")]
public class PlayerFeedBackDataObject : ScriptableObject
{
    [LabelText("清空房间反馈")]
    public FeedBackObject ClearRoomFeedback;
    [LabelText("闪避成功反馈")]
    public FeedBackObject EvadeInjureFeedBack;
    [LabelText("无敌受击反馈")]
    public FeedBackObject IsGodOnHitFeedback;
    [LabelText("无敌受击特效")]
    public GameObject IsGodOnHitParticle;
    [LabelText("普通受伤反馈")]
    public FeedBackObject NormalOnHitFeedback;
    [LabelText("普通受伤特效")]
    public GameObject NormalOnHitParticle;
    [LabelText("小受伤反馈")]
    public FeedBackObject SmallOnHitFeedback;
    [LabelText("小受伤特效")]
    public GameObject SmallOnHitParticle;
}
