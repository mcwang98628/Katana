using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleFootFeedback : RoleFootAudio
{
    public FeedBackObject feedbacks;
    public override void OnFoot(GameObject go, string eventname)
    {
        base.OnFoot(go, eventname);
        if (roleAnim != go)
        {
            return;
        }
        //Debug.Log(go);
        if (AnimatorEventName.Foot == eventname)
        {
            if(feedbacks!=null)
            {
                FeedbackManager.Inst.UseFeedBack(GetComponent<RoleController>(),feedbacks);
            }
        }
    }
}
