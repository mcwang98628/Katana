using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossHpBar : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private UIText BossName;
    [SerializeField]
    private Slider hpBgBar;
    [SerializeField]
    private Slider hpBar;
    
    private void Awake()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.alpha = 0;
        EventManager.Inst.AddEvent(EventName.OnRoleRegistered,OnNewBoss);
        EventManager.Inst.AddEvent(EventName.OnRoleInjured,OnBossInjured);
        EventManager.Inst.AddEvent(EventName.OnRoleTreatment,OnBossTreatment);
    }


    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnRoleRegistered,OnNewBoss);
        EventManager.Inst.RemoveEvent(EventName.OnRoleInjured,OnBossInjured);
        EventManager.Inst.RemoveEvent(EventName.OnRoleTreatment,OnBossTreatment);
    }

    private EnemyController currentBoss;
    private void OnNewBoss(string arg1, object arg2)
    {
        var role = (RoleController) arg2;
        if (role is EnemyController enemy &&
            (enemy.TeamType == RoleTeamType.Enemy_Boss || 
             enemy.TeamType == RoleTeamType.EliteEnemy) )
        {
            gameObject.SetActive(true);
            _canvasGroup.DOFade(1, 0.2f);
            currentBoss = enemy;
            var enemyInfo = DataManager.Inst.GetEnemyInfo(enemy.UniqueID);
            BossName.text = enemyInfo.EnemyName;
            hpBgBar.DOValue(1, 0.3f);
            hpBar.DOValue(1, 0.5f);
        }
    }
    
    private void OnBossInjured(string arg1, object arg2)
    {
        if (currentBoss == null)
        {
            return;
        }
        if (((RoleInjuredInfo)arg2).RoleId != currentBoss.TemporaryId)
        {
            return;
        }

        OnBossHpChange();
    }
    

    private void OnBossTreatment(string arg1, object arg2)
    {
        if (currentBoss == null)
        {
            return;
        }
        if (((TreatmentData)arg2).RoleId != currentBoss.TemporaryId)
        {
            return;
        }

        OnBossHpChange();
    }

    void OnBossHpChange()
    {
        var value = currentBoss.CurrentHp / currentBoss.MaxHp;
        hpBar.DOValue(value, 0.1f);
        hpBgBar.DOValue(value, 0.3f).OnComplete(() =>
        {
            if (currentBoss.IsDie)
            {
                _canvasGroup.DOFade(0, 0.2f);
                currentBoss = null;
            }
        });
    }
}
