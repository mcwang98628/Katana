using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BattlePanel_BuffList : MonoBehaviour
{
    public UI_BattlePanel_BuffPrefab buffPrefab;

    //key = 临时ID
    Dictionary<string,RoleBuffEventData> BuffDatas = new Dictionary<string, RoleBuffEventData>();
    //key = 临时ID
    Dictionary<string,UI_BattlePanel_BuffPrefab> BuffPrefabs = new Dictionary<string, UI_BattlePanel_BuffPrefab>();
    
    private void Awake()
    {
        return;
        EventManager.Inst.AddEvent(EventName.OnRoleAddBuff,OnPlayerAddBuff);
        EventManager.Inst.AddEvent(EventName.OnRoleRemoveBuff,OnPlayerReMoveBuff);
    }
    private void OnDestroy()
    {
        return;
        EventManager.Inst.RemoveEvent(EventName.OnRoleAddBuff,OnPlayerAddBuff);
        EventManager.Inst.RemoveEvent(EventName.OnRoleRemoveBuff,OnPlayerReMoveBuff);
    }

    private void OnPlayerAddBuff(string eventName, object buffEventData)
    {
        RoleBuffEventData rbed = (RoleBuffEventData) buffEventData;
        if (rbed.RoleId != BattleManager.Inst.CurrentPlayer.TemporaryId)
        {
            return;
        }
        UI_BattlePanel_BuffPrefab buffGo = GameObject.Instantiate(buffPrefab, transform);
        buffGo.Init(rbed.BuffId);
        BuffDatas.Add(rbed.BuffTemporaryId,rbed);
        BuffPrefabs.Add(rbed.BuffTemporaryId,buffGo);
    }
    private void OnPlayerReMoveBuff(string eventName, object buffEventData)
    {
        RoleBuffEventData rbed = (RoleBuffEventData) buffEventData;
        if (rbed.RoleId != BattleManager.Inst.CurrentPlayer.TemporaryId)
        {
            return;
        }
        if (!BuffDatas.ContainsKey(rbed.BuffTemporaryId) || !BuffPrefabs.ContainsKey(rbed.BuffTemporaryId))
        {
            return;
        }
        GameObject.Destroy(BuffPrefabs[rbed.BuffTemporaryId].gameObject);
        BuffDatas.Remove(rbed.BuffTemporaryId);
        BuffPrefabs.Remove(rbed.BuffTemporaryId);
    }
}
