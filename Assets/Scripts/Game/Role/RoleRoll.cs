using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class RoleRoll : MonoBehaviour
{
    public ParticleSystem Effect;
    protected RoleController roleController;
    public PhantomReleaser phantom;
    bool IsRollLastFrame;
    public FeedBackObject RollFeedbacks;

    // public float RollSpeed;
    // public float RollTime;
    // [SerializeField]
    // protected AnimationCurve rollCurve;

    private static readonly int Roll = Animator.StringToHash("Roll"); 
    private static readonly int Attack = Animator.StringToHash("Attack");



    protected  virtual void Awake()
    {
        roleController = GetComponent<RoleController>();

        EventManager.Inst.AddAnimatorEvent(AnimEvent);
        EventManager.Inst.AddEvent(EventName.OnRoleInjured,OnPlayerInjured);
    }

    private void OnPlayerInjured(string arg1, object arg2)
    {
        var data = (RoleInjuredInfo) arg2;
        if (data.RoleId==roleController.TemporaryId && roleController.IsRolling)
        {
            RollBack();
        }
    }


    public virtual void InputRoll(Vector2 v2)
    {
        if (!roleController.IsCanRoll)
        {
            return;
        }
         
        roleController.Animator.ResetTrigger(Attack);
        if (roleController.Animator2 != null)
        {
            roleController.Animator2.ResetTrigger(Attack);
        }
        roleController.StopAttack();
        
        if (v2 == Vector2.zero)
        {
            var forward = roleController.Animator.transform.forward;
            v2 = new Vector2(forward.x,forward.z);
        }
        
        roleController.Animator.transform.forward = new Vector3(v2.x,0,v2.y);
        
        
        //在这里设置无敌，因为这样不容易出问题
        OnStartRoll();
        roleController.Animator.SetTrigger(Roll);
        if (roleController.Animator2 != null)
        {
            roleController.Animator2.SetTrigger(Roll);
        }
        
        // roleController.FastMove(RollTime,RollSpeed,new Vector3(v2.x,0,v2.y), RollBack,rollCurve);
        PlayEffect(new Vector3(v2.x, 0, v2.y));
        EventManager.Inst.DistributeEvent(EventName.OnRoleRoll);
    }

    protected void PlayEffect(Vector3 dir)
    {
        if (Effect)
        {
            Effect.transform.forward = dir;
            Effect.transform.position = transform.position - dir.normalized*.5f + Vector3.up;
            Effect.Play();
        }
    }
    public virtual void OnStartRoll()
    {
        roleController.SetIsRoll(true);
        if(RollFeedbacks!=null)
        FeedbackManager.Inst.UseFeedBack(roleController,RollFeedbacks);
    }
    public void OnEndRoll()
    {
        roleController.SetIsRoll(false);
    }
    protected virtual void RollBack()
    {
        
    }
    protected virtual void AnimEvent(GameObject go, string eventName)
    {
        if (go !=roleController.Animator.gameObject)
        {
            return;
        }

        if (eventName.Contains(AnimatorEventName.RollEnd))
        {
            RollBack();
        }
        else if (eventName.Contains(AnimatorEventName.RollStart))
        {
            roleController.SetIsRoll(true);
        }
    }
    protected virtual void OnDestroy()
    {
        EventManager.Inst.RemoveAnimatorEvent(AnimEvent);
        EventManager.Inst.RemoveEvent(EventName.OnRoleInjured,OnPlayerInjured);
    }


}
