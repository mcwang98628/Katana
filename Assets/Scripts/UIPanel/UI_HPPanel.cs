using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HPPanel : MonoBehaviour
{
    [SerializeField]
    private UI_Hp hpPrefab;
    // [SerializeField]
    // private UI_PowerBar powerBarPrefab;
    
    private Dictionary<string,UI_Hp> Hps = new Dictionary<string, UI_Hp>();
    //记忆玩家的ID
    [HideInInspector]
    public string PlayerID;
    private void Awake()
    {
        EventManager.Inst.AddEvent(EventName.OnRoleDead,OnRoleDead);
        //EventManager.Inst.AddEvent(EventName.EnterNextRoom,ClearAllMonsterHPBar);
        EventManager.Inst.AddEvent(EventName.OnRoleRegistered,OnRoleRegis);

        StartCoroutine(waitInitPlayerHp());
    }

    IEnumerator waitInitPlayerHp()
    {
        yield return null;
        
        if (BattleManager.Inst.CurrentPlayer!=null)
        {
            OnRoleRegis("", BattleManager.Inst.CurrentPlayer);
        }
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnRoleDead,OnRoleDead);
        //EventManager.Inst.RemoveEvent(EventName.EnterNextRoom, ClearAllMonsterHPBar);
        EventManager.Inst.RemoveEvent(EventName.OnRoleRegistered,OnRoleRegis);
    }
    
    private void OnRoleRegis(string eventName, object role_)
    {
        var role = (RoleController) role_;
        if(role.roleTeamType==RoleTeamType.Player)
        {
            PlayerID = role.TemporaryId;
            // UI_PowerBar powerBar = Instantiate(powerBarPrefab, transform);
            // powerBar.SetTarget(role);
        }
        UI_Hp hp = Instantiate(hpPrefab,transform);
        hp.SetTarget(role);
        Hps.Add(role.TemporaryId,hp);
//        var playerRole = BattleManager.Inst.CurrentPlayer;
//        UI_Hp playerhp = Instantiate(hpPrefab,transform);
//        playerhp.SetTarget(playerRole);
//        Hps.Add(playerRole.CurrentId,playerhp);
    }
    private void OnRoleDead(string arg1, object arg2)
    {
        RoleDeadEventData roleDeadEventData = (RoleDeadEventData) arg2;
        if (roleDeadEventData.DeadRole.TemporaryId == BattleManager.Inst.CurrentPlayer.TemporaryId)
        {
            return;
        }
        if (Hps.ContainsKey(roleDeadEventData.DeadRole.TemporaryId))
        {
            UIManager.Inst.StartCoroutine(DelayDestroy( roleDeadEventData.DeadRole.TemporaryId, 0.2f));
        }
    }
    IEnumerator DelayDestroy(string deadRoleID, float time)
    {
        yield return new WaitForSeconds(time);

        Hps[deadRoleID].FadeOff(0.3f);

        yield return new WaitForSeconds(0.3f);
        Destroy(Hps[deadRoleID].gameObject);
        Hps.Remove(deadRoleID);
    }
    public void ClearAllMonsterHPBar(string arg1, object arg2)
    {
        Debug.Log("TryClearHpPanel");
        if (Hps.Count > 0)
        {

            foreach (var hpBar in Hps)
            {
                if (hpBar.Key != PlayerID)
                {
                    Destroy(hpBar.Value.gameObject);
                    Hps.Remove(hpBar.Key);
                }
            }
        }
    }
}
