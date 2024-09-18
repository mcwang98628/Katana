using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerCommonFeedback : Role_Common_FeedBack
{
    [SerializeField][BoxGroup("Player独有")][InlineEditor()]
    protected PlayerFeedBackDataObject PlayerFeedBackDataObject;
    protected override void Awake()
    {
        base.Awake();
        EventManager.Inst.AddEvent(EventName.OnRoleDodgeInjured, OnRoleDodgeInjured);
        EventManager.Inst.AddEvent(EventName.ClearAllEnemy, FinishRoomFeedback);
        EventManager.Inst.AddEvent(EventName.OnRoleGodInjured,OnRoleGodInjured);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.Inst.RemoveEvent(EventName.OnRoleDodgeInjured, OnRoleDodgeInjured);
        EventManager.Inst.RemoveEvent(EventName.ClearAllEnemy, FinishRoomFeedback);
        EventManager.Inst.RemoveEvent(EventName.OnRoleGodInjured,OnRoleGodInjured);
    }
    
    private void OnRoleDodgeInjured(string arg1, object arg2)
    {
        if (PlayerFeedBackDataObject == null)
        {
            return;
        }
        var data = (RoleInjuredInfo)arg2;
        if (data.RoleId != BattleManager.Inst.CurrentPlayer.TemporaryId)
        {
            return;
        }
        if (PlayerFeedBackDataObject.EvadeInjureFeedBack == null)
        {
            return;
        }
        FeedbackManager.Inst.UseFeedBack(roleController,PlayerFeedBackDataObject.EvadeInjureFeedBack);
        
    }
    
    public void FinishRoomFeedback(string arg1,object arg2)
    {
        if (PlayerFeedBackDataObject == null)
        {
            return;
        }
        FeedbackManager.Inst.UseFeedBack(roleController,PlayerFeedBackDataObject.ClearRoomFeedback); 
    }
    
    //无敌受击
    private void OnRoleGodInjured(string arg1, object arg2)
    {
        var data = (RoleInjuredInfo) arg2;
        if (data.RoleId != roleController.TemporaryId)
        {
            return;
        }
        
        if (PlayerFeedBackDataObject.IsGodOnHitParticle != null )
        {
            GameObject CurrentPar =
                PlayerFeedBackDataObject.IsGodOnHitParticle.DuplicateAtPlayer();
            CurrentPar.transform.position += new Vector3(0, 1f, 0);
            CurrentPar.transform.SetParent(roleController.roleNode.Model);
        }
        if(PlayerFeedBackDataObject.IsGodOnHitFeedback!=null)
        {
            FeedbackManager.Inst.UseFeedBack(roleController,PlayerFeedBackDataObject.IsGodOnHitFeedback);
        }
    }

    #region 受伤

    protected override void OnRoleInjured(string arg1, object arg2)
    {
        if (PlayerFeedBackDataObject == null)
        {
            return;
        }
        var data = (RoleInjuredInfo) arg2;
        var dmg = data.Dmg;
        if (data.RoleId != roleController.TemporaryId)
        {
            return;
        }
        
        if (dmg.DmgType == DmgType.Physical ||dmg.DmgType==DmgType.Explosion )
        {
            if (dmg.DmgValue < roleController.MaxHp * 0.025f)
            {
                playSmall();
            }
            else
            {
                playNormal();
            }
        }
        else if (dmg.DmgType == DmgType.Vampire)
        {
            playSmall();
        }
        else
        {
            Debug.Log("使用了缺省的Feedback");
            playNormal();
        }
    }

    void playSmall()
    {
        if (PlayerFeedBackDataObject.SmallOnHitFeedback != null)
            FeedbackManager.Inst.UseFeedBack(roleController, PlayerFeedBackDataObject.SmallOnHitFeedback);
        if(PlayerFeedBackDataObject.SmallOnHitParticle!=null)
            PlayerFeedBackDataObject.SmallOnHitParticle.Duplicate().transform.position = gameObject.transform.position + new Vector3(0, 0.5f, 0);
    }

    void playNormal()
    {
        if (PlayerFeedBackDataObject.NormalOnHitFeedback != null)
            FeedbackManager.Inst.UseFeedBack(roleController, PlayerFeedBackDataObject.NormalOnHitFeedback);
        if (PlayerFeedBackDataObject.NormalOnHitParticle != null)
            Instantiate(PlayerFeedBackDataObject.NormalOnHitParticle.gameObject, gameObject.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity).SetActive(true);
    }
    

    #endregion
}
