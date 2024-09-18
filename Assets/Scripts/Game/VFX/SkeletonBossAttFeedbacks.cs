using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBossAttFeedbacks : MonoBehaviour
{
    public GameObject roleAnim;
    public FeedBackObject Attack1Feedback;
    public FeedBackObject Attack2Feedback;
    public FeedBackObject GroundFeedback;
    public GameObject Attack1Particles;
    void Awake()
    {
        EventManager.Inst.AddAnimatorEvent(OnAttack);
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveAnimatorEvent(OnAttack);
    }

    public void OnAttack(GameObject go, string eventname)
    {
        if(go!=roleAnim)
        {
            return;
        }
        else
        {
                //Debug.Log(go+eventname);
            if(eventname=="Attack1"&&Attack1Feedback!=null)
            {
                FeedbackManager.Inst.UseFeedBack(GetComponent<RoleController>(),Attack1Feedback);
                if(Attack1Particles!=null)
                {
                    Instantiate(Attack1Particles,Attack1Particles.transform.position,Attack1Particles.transform.rotation).SetActive(true);
                }
            }
            if(eventname == "Attack2"&& Attack2Feedback != null)
            {
                FeedbackManager.Inst.UseFeedBack(GetComponent<RoleController>(), Attack2Feedback);
            }
            if (eventname == "Ground" && GroundFeedback != null)
            {
                FeedbackManager.Inst.UseFeedBack(GetComponent<RoleController>(), GroundFeedback);
            }

        }
    }
}
