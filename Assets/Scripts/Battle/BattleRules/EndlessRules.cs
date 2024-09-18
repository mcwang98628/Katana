using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessRules : BaseRules
{
    public override LevelStructType BattleRulesType => LevelStructType.OneRoomEndless;
    public override RoomController CurrentRoom => _currentRoomObject;
    public EndlessRulesRuntimeData RuntimeData => (EndlessRulesRuntimeData)BattleManager.Inst.RuntimeData;
    
    private EndlessRoom _currentRoomObject;
    private EndlessStructData chapterStructData;
    private ChapterData chapterData;
    
    void AddEvent()
    {
        EventManager.Inst.AddEvent(EventName.OnRoleDead, OnRoleDead);
        EventManager.Inst.AddEvent(EventName.EnterGameOverDoor, OnEnterDoor);
        EventManager.Inst.AddEvent(EventName.EnterLastDoor, OnEnterDoor);
        EventManager.Inst.AddEvent(EventName.EnterDoor, OnEnterDoor);
        
        EventManager.Inst.AddEvent(EventName.EndlessGameOver,OnGameOver);
    }
    void RemoveEvent()
    {
        EventManager.Inst.RemoveEvent(EventName.OnRoleDead, OnRoleDead);
        EventManager.Inst.RemoveEvent(EventName.EnterGameOverDoor, OnEnterDoor);
        EventManager.Inst.RemoveEvent(EventName.EnterLastDoor, OnEnterDoor);
        EventManager.Inst.RemoveEvent(EventName.EnterDoor, OnEnterDoor);
        
        EventManager.Inst.RemoveEvent(EventName.EndlessGameOver,OnGameOver);
    }

    private void OnGameOver(string arg1, object arg2)
    {
        GameOver(true,null);
    }

    #region 游戏结束

    private void OnEnterDoor(string arg1, object arg2)
    {
        //GameOver 胜利
        GameOver(true,null);
    }
    private void OnRoleDead(string arg1, object arg2)
    {
        RoleDeadEventData roleDeadEventData = (RoleDeadEventData) arg2;
        if (roleDeadEventData.DeadRole.TemporaryId == CurrentPlayer.TemporaryId)
        {
            BattleManager.Inst.StartCoroutine(WaitGameOver(roleDeadEventData));
        }
        else if (roleDeadEventData.DeadRole.roleTeamType != RoleTeamType.Player)
        {
            ArchiveManager.Inst.ArchiveData.AddKillEnemyCount(roleDeadEventData.DeadRole.UniqueID, 1);
            RuntimeData.AddKillEnemyNumber(roleDeadEventData.DeadRole.roleTeamType);
        }
    }
    //死亡游戏结束
    IEnumerator WaitGameOver(RoleDeadEventData roleDeadEventData)
    {
        yield return null;
        yield return new WaitForSeconds(2f);
        if (CurrentPlayer.IsDie)
        {
            GameOver(false,roleDeadEventData);
        }
    }
    void GameOver(bool isVictory, RoleDeadEventData roleDeadEventData = null)
    {
        BattleManager.Inst.GameIsRuning = false;
        UIManager.Inst.Close("BattlePanel");
        FeedbackManager.Inst.StopAllCoroutines();
        
        RemoveEvent();
        
        GameOverData gameOverData = new GameOverData();
        gameOverData.isVictory = isVictory; //玩家没死游戏结束就是胜利
        // gameOverData.Gold = RuntimeData.CurrentGold;
        gameOverData.KillEnemyNumber = RuntimeData.KillEnemyNumber;
        gameOverData.KillSEnemyNumber = RuntimeData.KillSEnemyNumber;
        gameOverData.Progress = RuntimeData.Progress;
        gameOverData.ClearRoomNuber = 1;
        if (isVictory)
        {
            gameOverData.Progress = 100;
        }
        if (roleDeadEventData != null)
        {
            gameOverData.KillPlayerEnemyId = roleDeadEventData.AttackerRole.UniqueID;
        }
        if (!isVictory && ArchiveManager.Inst.ArchiveData.BattleData != null)
        {
            UIManager.Inst.Open("FirePanel",true,new Action(() =>
            {
                SettlementReward(gameOverData);
            }));
            return;
        }

        SettlementReward(gameOverData);
    }
    
    //结算奖励
    void SettlementReward(GameOverData gameOverData)
    {
        // float value = 0;
        // if (gameOverData.isVictory)
        // {
        //     value += 200;
        // }
        // ArchiveManager.Inst.ChangeDiamond((int) value);
        UIManager.Inst.Open("BattleOverPanel", true, gameOverData);
        EventManager.Inst.DistributeEvent(EventName.OnBattleOver, gameOverData);
        ArchiveManager.Inst.AddGameTime(BattleManager.Inst.BattleTime);

        // if (gameOverData.isVictory)
        // {
        //     ArchiveManager.Inst.ChapterClearance(CurrentChapterId, 100);
        // }
        // else
        // {
        //     ArchiveManager.Inst.ChapterClearance(CurrentChapterId, BattleData.Progress);
        // }
        
        // //物品解锁
        // BattleTool.CheckItemUnLock();
    }

    #endregion


    public override void StartGame()
    {
        BattleManager.Inst.DoCamera();
        UIManager.Inst.Open("BattlePanel");
        EventManager.Inst.DistributeEvent(EventName.OnEndlessBattleStart);   
        BattleManager.Inst.GameIsRuning = true;
        AddEvent();
        ArchiveManager.Inst.UseFire(1);
        
        _currentRoomObject.StartBattle();
    }

    public override void LoadBattle(IBattleRulesData rulesData)
    {
        if (rulesData is EndlessRulesData endlessRulesData)
        {
            if (_currentRoomObject != null)
            {
                GameObject.Destroy(_currentRoomObject.gameObject);
            }
            endlessRulesData.ChapterData.LoadChapterData(delegate(ChapterData data)
            {
                chapterData = data;
                chapterStructData = (EndlessStructData)BattleTool.GenerateChapterStructData(chapterData);
                RuntimeData.SetLevelStructData(chapterStructData);
                BattleManager.Inst.StartCoroutine(LoadBattle());
            });
        }
    }

    public override void LoadBattle(IArchiveBattleData archiveBattleData)
    {
        throw new System.NotImplementedException();
    }

    public override void EndBattle()
    {
        RemoveEvent();
    }
    
    public override void Update()
    {
        
    }
    
    
    private IEnumerator LoadBattle()
    {
        bool isMask = false;
        UIManager.Inst.ShowMask(() => { isMask = true;});
        while (!isMask)
        {
            yield return null;
        }
        
        ScenesManager.Inst.LoadScene("TestScene");
        yield return null;
        
        ArchiveManager.Inst.CopyUnLcokItems();
        ArchiveManager.Inst.StartBattle();

        //创建房间
        string endlessRoomPath = "Assets/Arts/RoomPrefabs/Rooms/100Challenge/{0}.prefab";
        ResourcesManager.Inst.GetAsset<GameObject>(string.Format(endlessRoomPath,chapterStructData.RoomPerfabName),
            delegate(GameObject go)
            {
                _currentRoomObject = GameObject.Instantiate(go).GetComponent<EndlessRoom>();
                _currentRoomObject.SetWaveData(chapterStructData.WaveDatas);

                InitPlayer();

                _currentRoomObject.SetPlayerPosition();
        
                UIManager.Inst.HideMask(null);
            });
        
        // _currentRoomObject = GameObject.Instantiate(endlessRoomPrefab).GetComponent<EndlessRoom>();
        // _currentRoomObject.SetWaveData(chapterStructData.WaveDatas);
        //
        // InitPlayer();
        //
        // _currentRoomObject.SetPlayerPosition();
        //
        // RuntimeData.ResetEnhancedEnemyProbability();
        // UIManager.Inst.HideMask(null);
    }

    
    
    
    #region 初始化Player

    public void InitPlayer()
    {
        if (IsArchiveBattle)
        {
            //已经设置过CurrentPlayerHeroData了。
        }
        else
        {
            int heroId = ArchiveManager.Inst.ArchiveData.GlobalData.LastSelectHeroID;
            RuntimeBaseData.SetHeroData(heroId);
        }
        CreatePlayerAndCamera();
        InitPlayerAttributes();
        AddPlayerItem();
        PlayerAddGlobalRuneItem();
    }
     
    
    #endregion

    
}
