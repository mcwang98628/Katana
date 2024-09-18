using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Role_Common_FeedBack : MonoBehaviour
{
    protected RoleController roleController;

    [SerializeField]
    [BoxGroup("通用")]
    [InlineEditor()]
    protected CommonFeedBackDataObject CommonFeedBackDataObject;

    
    protected virtual void Awake()
    {
        roleController = GetComponent<RoleController>();
        EventManager.Inst.AddEvent(EventName.OnRoleDead,OnRoleDead);
        EventManager.Inst.AddEvent(EventName.OnRoleInjured,OnRoleInjured);
    }


    protected virtual void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnRoleDead,OnRoleDead);
        EventManager.Inst.RemoveEvent(EventName.OnRoleInjured,OnRoleInjured);
    }

    protected virtual void OnRoleInjured(string arg1, object arg2) { }

    #region 开场

    protected virtual void OnEnable() { }

    #endregion

    #region 死亡

    private void OnRoleDead(string arg1, object arg2)
    {
        var data = (RoleDeadEventData) arg2;
        if (data.DeadRole.TemporaryId != roleController.TemporaryId)
        {
            return;
        }
        OnDead();
    }
    void OnDead()
    {
        if (CommonFeedBackDataObject == null)
        {
            return;
        }
        if (CommonFeedBackDataObject.deadParticle != null)
        {
            GameObject CurrentDeathPar = Instantiate(CommonFeedBackDataObject.deadParticle.gameObject, gameObject.transform.position, Quaternion.identity);
            CurrentDeathPar.SetActive(true);
            if (roleController.roleTeamType == RoleTeamType.Enemy ||
                roleController.roleTeamType == RoleTeamType.Enemy_Boss ||
                roleController.roleTeamType == RoleTeamType.EliteEnemy)
                CurrentDeathPar.transform.SetParent(gameObject.transform);
        }
        if (CommonFeedBackDataObject.DeathFeedback != null)
        {
            FeedbackManager.Inst.UseFeedBack(roleController, CommonFeedBackDataObject.DeathFeedback);
        }
    }
    
    

    #endregion
    
}
