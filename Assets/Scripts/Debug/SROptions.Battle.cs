using System.Collections;
using System.ComponentModel;
using UnityEngine;

public partial class SROptions
{
    
    [Category("战斗相关")]
    public void PlayerSetGodTrue()
    {
        if (BattleManager.Inst.CurrentPlayer == null)
        {
            Debug.LogError("玩家角色不存在");
            return;
        }
        BattleManager.Inst.CurrentPlayer.SetGod(true);
        Debug.Log("Debug 设置玩家无敌 true");
    }
    
    [Category("战斗相关")]
    public void PlayerSetGodFalse()
    {
        if (BattleManager.Inst.CurrentPlayer == null)
        {
            Debug.LogError("玩家角色不存在");
            return;
        }
        BattleManager.Inst.CurrentPlayer.SetGod(false);
        Debug.Log("Debug 设置玩家无敌 false");
    }

    private Coroutine _autoKillEnemy = null;
    [Category("战斗相关")]
    public void AutoKillEnemyStart()
    {
        if (!BattleManager.Inst.GameIsRuning)
        {
            Debug.LogError("游戏未开始。");
            return;
        }

        if (_autoKillEnemy != null)
        {
            Debug.LogError("秒怪已经开启了。。");
            return;
        }

        _autoKillEnemy = GameManager.Inst.StartCoroutine(AutoKillEnemy());
        Debug.Log("Debug 自动秒怪 开启");
    }
    
    [Category("战斗相关")]
    public void AutoKillEnemyEnd()
    {
        if (!BattleManager.Inst.GameIsRuning)
        {
            Debug.LogError("游戏未开始。");
            return;
        }

        if (_autoKillEnemy == null)
        {
            Debug.LogError("秒怪还未开启。。");
            return;
        }
        GameManager.Inst.StopCoroutine(_autoKillEnemy);
        _autoKillEnemy = null;
        Debug.Log("Debug 自动秒怪 关闭");
    }
    

    IEnumerator AutoKillEnemy()
    {
        yield return null;
        while (true)
        {
            foreach (var enemy in BattleManager.Inst.EnemyTeam)
            {
                if (enemy.Value.IsDie)
                {
                    continue;
                }
                
                DamageInfo dmg = new DamageInfo(
                    enemy.Value.TemporaryId,
                    9999,
                    BattleManager.Inst.CurrentPlayer, 
                    BattleManager.Inst.CurrentPlayer.transform.position,
                    DmgType.Other, false,false,0,0);
                enemy.Value.HpInjured(dmg);
            }
            yield return null;
        }
    }
}