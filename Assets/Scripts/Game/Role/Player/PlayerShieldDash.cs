using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//盾冲，先决条件：
//1.不在盾冲状态

//具有以下功能：
//1.位移
//2.开始举盾动画
//2.开启举盾状态
//3.开启伤害。
public class PlayerShieldDash : PlayerRoll
{
    public int DashPowerNeeded;
    public AudioClip StartSFX;
    public AudioClip WhooshSFX;
    public ParticleSystem DashParticles;
    public FeedBackObject StartShieldDashFeedback;
    PlayerShieldHealth _health;

    private bool Dash_SkipPreAction;
    private bool Dash_AfterAttack;
    //是否正在前冲的过程中
    public bool IsShieldDashing;
    protected override void Awake()
    {
        base.Awake();
        Dash_SkipPreAction = false;
        Dash_AfterAttack = false;
        _health = GetComponent<PlayerShieldHealth>();
    }

    public void SetDash_AfterAttack(bool value)
    {
        Dash_AfterAttack = value;
    }

    public void SetDash_SkipPreAction(bool value)
    {
        Dash_SkipPreAction = value;
    }
    
    void Dash(Vector2 v2)
    {
        if (roleController.CurrentSkillPower >= DashPowerNeeded)
        {
            roleController.Animator.SetBool("Dash_SkipPreAction",Dash_SkipPreAction);
            roleController.Animator.SetBool("Dash_AfterAttack",Dash_AfterAttack);
            float moveSpeed = 10f;
            float moveTime = ((PlayerShieldAttack) roleController.roleAttack).CurrentSprintDistance / moveSpeed;
            roleController.Animator.transform.forward = new Vector3(v2.x,0,v2.y);
            roleController.Animator.ResetTrigger("StopDash");
            IsShieldDashing = true;
            roleController.FastMove(moveTime,moveSpeed,roleController.Animator.transform.forward,() =>
            {
                IsShieldDashing = false;
                roleController.Animator.SetTrigger("StopDash");
            });
        }
    }
    
    
    protected override void OnInput(JoyStatusData statusData)
    {
        if (statusData.JoyStatus == UIJoyStatus.OnSlide ||
            statusData.JoyStatus == UIJoyStatus.OnHoldSlide)
        {
            base.OnInput(statusData);
        }
        if (!((PlayerAttack)roleController.roleAttack).IsAccumulateing )
        {
            return;
        }
        
        if (statusData.JoyStatus == UIJoyStatus.OnHoldEnd 
                 || statusData.JoyStatus == UIJoyStatus.OnHoldDragEnd)
        {
            Vector3 forward = roleController.Animator.transform.forward;
            roleController.Animator.SetTrigger("AccumulateAttack");
            Dash(new Vector2(forward.x,forward.z));
        }
    }
    
    
    public override void OnStartRoll()
    {
        roleController.AddSkillPower(-DashPowerNeeded);
        roleController.FreezeSkillPower();
    }
    protected override void AnimEvent(GameObject go, string eventName)
    {
        //Debug.Log(eventName);
        base.AnimEvent(go,eventName);
        if(go!=roleController.Animator.gameObject)
        {
            return;
        }
        if(eventName=="StartSFX")
        {
            StartSFX.Play();
            FeedbackManager.Inst.UseFeedBack(roleController, StartShieldDashFeedback);
        }
        if(eventName=="Whoosh")
        {
            DashParticles.Play();
            WhooshSFX.Play();
        }
        if(eventName=="DashGodStart")
        {
            // _health.IsDashingShield = true;
            
        }
        if(eventName=="DashGodEnd")
        {
            // _health.IsDashingShield = false;
        }
        if(eventName=="StartAttack_20")
        {
            _health.SetIsAcceptInterruption(false);
        }
        if(eventName=="EndAttack_20")
        {
            _health.SetIsAcceptInterruption(true);
            roleController.UnFreezeSkillPower();
        }
        //public void ClearDmg()
        //{
        //    dmging = false;
        //    currentDmgAttackInfo = null;
        //    dmgList.Clear();
        //}
    }
  
}
