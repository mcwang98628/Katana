using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "FeedBack/Common")]
public class CommonFeedBackDataObject : ScriptableObject
{
    [LabelText("死亡反馈")]
    public FeedBackObject DeathFeedback;
    [LabelText("死亡特效")]
    public ParticleSystem deadParticle;
}