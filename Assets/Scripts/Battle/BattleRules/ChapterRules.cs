using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChapterRules : SimilarChapterRules
{
    public override LevelStructType BattleRulesType { get; } = LevelStructType.Chapter;
    
    protected override string RoomPrefabPath => "Assets/Arts/RoomPrefabs/Rooms/Chapter{0}/{1}.prefab";

    public override void StartGame()
    {
        base.StartGame();
        if (!IsArchiveBattle)
        {
            ArchiveManager.Inst.SaveBattleData(null);
        }
        InitPlayerItem();
        BattleManager.Inst.StartCoroutine(WaitDoCamera());
        UIManager.Inst.Open("BattlePanel");
        ArchiveManager.Inst.UseFire(1);
        if (IsArchiveBattle)
        {
            EventManager.Inst.DistributeEvent(EventName.OnArchiveBattleStart);
        }
        else
        {
            EventManager.Inst.DistributeEvent(EventName.OnChapterBattleStart);   
        }
        BattleManager.Inst.GameIsRuning = true;
    }

    private ChapterRulesData _chapterRulesData;
    public override void LoadBattle(IBattleRulesData rulesData)
    {
        IsLoadOver = false;
        IsArchiveBattle = false;
        isArchiveBattleStartRoom = false;
        if (rulesData is ChapterRulesData chapterRulesData)
        {
            _chapterRulesData = chapterRulesData;
            BattleTool.GenerateChapterStructData(chapterRulesData.ChapterId, delegate(ChapterStructData data)
            {
                SetChapterData(chapterRulesData.ChapterId,0,0,0,data);
                BattleManager.Inst.StartCoroutine(LoadBattle());
                EventManager.Inst.AddEvent(EventName.SelectHero, OnChangeHero);
            });
        }
        else
        {
            Debug.LogError("Err!");
            return;
        }
        
    }

    private BattleArchiveData_1_4_0 archiveData;
    public override void LoadBattle(IArchiveBattleData archiveBattleData)
    {
        IsLoadOver = false;
        IsArchiveBattle = true;
        isArchiveBattleStartRoom = true;
        archiveData = (BattleArchiveData_1_4_0) archiveBattleData;
        _chapterRulesData = new ChapterRulesData(){
            ChapterId = archiveData.ChapterData.CurrentCPId,
            ConnectPrefabName = $"Room_Chapter{archiveData.ChapterData.CurrentCPId}_Connect"
        };

        BattleTool.GenerateChapterStructData(archiveData.ChapterData.CurrentCPId, delegate(ChapterStructData data)
        {
            SetChapterData(
                archiveData.ChapterData.CurrentCPId,
                archiveData.ChapterData.CurrentLevelIndex,
                archiveData.ChapterData.CurrentRoomIndex,
                archiveData.PlayerData.CurrentGold,
                data
            );
        
            RuntimeBaseData.SetHeroData(archiveData.PlayerData.HeroId);
            BattleManager.Inst.StartCoroutine(LoadBattle());
        });
    }

    public override void Update() { }

    private void SetChapterData(int cpId,int level,int room,int gold,IBattleLevelStructData levelStructData)
    {
        RuntimeData.SetChapterData(cpId, level, room);
        RuntimeData.SetGold(gold);
        RuntimeData.SetDiceCount(3);
        RuntimeData.SetLevelStructData(levelStructData);
    }



    protected override IEnumerator LoadBattle(Action callBack = null)
    {
        LoadRoomConnect();
        yield return base.LoadBattle();
        
        if (IsArchiveBattle)
        {
            RuntimeData.SetGold(archiveData.PlayerData.CurrentGold); 
            
            if (currentRoomObject.RoomType == RoomType.FightRoom ||
                currentRoomObject.RoomType == RoomType.BossFightRoom)
            {
                ((FightRoom) currentRoomObject).StartCreateEnemy();
            }
        }
        callBack?.Invoke();
    }

    protected override void LoadRoomConnect()
    {
        if (_chapterRulesData == null)
        {
            Debug.LogError("错误。。。");
        }
        CreateRoomConnect(_chapterRulesData.ConnectPrefabName, connect =>
        {
            connectOffsetObject = connect;
        });
    }

    protected override void StartRoom(Action callBack)
    {
        DataManager.Inst.GetCpData(CurrentChapterId).LoadChapterData(delegate(ChapterData cpDataObj)
        {
            switch (cpDataObj.levelStructType)
            {
                case LevelStructType.Chapter:
                    if (cpDataObj.IsTutorial)
                    {
                        EnvironmentManager.Inst.SetEnvironment(cpDataObj.EnvironmentData[CurrentLevelIndex]);
                    }
                    else
                    {
                        EnvironmentManager.Inst.SetEnvironment(cpDataObj.EnvironmentData[CurrentLevelIndex]);
                    }
                    break;
                case LevelStructType.OneRoomEndless:
                    break;
            }
        });
        base.StartRoom(callBack);
        
    }
}
