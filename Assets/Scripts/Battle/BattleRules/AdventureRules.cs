
using System;
using System.Collections;
using UnityEngine;

public class AdventureRules:SimilarChapterRules
{
    public override LevelStructType BattleRulesType => LevelStructType.Adventure;
    protected override string RoomPrefabPath => "Assets/Arts/RoomPrefabs/Rooms/Chapter{0}/{1}.prefab";
    // public override RoomController CurrentRoom { get; }
    public override void StartGame()
    {
    }

    private AdventureRulesData _adventureRulesData;
    public override void LoadBattle(IBattleRulesData rulesData)
    {
        UIManager.Inst.ShowLogoMask(() =>
        {
            
            
            IsLoadOver = false;
            IsArchiveBattle = false;
            isArchiveBattleStartRoom = false;
            if (rulesData is AdventureRulesData adventureRulesData)
            {
                _adventureRulesData = adventureRulesData;
                SetChapterData(0,0,0,adventureRulesData.AdventureStructData);
                LoadRoomConnect();
                EnvironmentManager.Inst.SetEnvironment(_adventureRulesData.AdventureStructData.EnvironmentDatas[CurrentLevelIndex]);
            
                BattleManager.Inst.StartCoroutine(LoadBattle(delegate { 
                    base.StartGame();
                    InitPlayerItem();
                    BattleManager.Inst.StartCoroutine(WaitDoCamera());
                    UIManager.Inst.Open("BattlePanel");
                    ArchiveManager.Inst.UseFire(1);
                    BattleManager.Inst.GameIsRuning = true;
                    EventManager.Inst.DistributeEvent(EventName.OnChapterBattleStart);
                
                    UIManager.Inst.HideLogoMask(null);
                }));
            
                EventManager.Inst.AddEvent(EventName.SelectHero, OnChangeHero);
            }
            else
            {
                Debug.LogError("Err!");
                return;
            }
            
        });
    }
    public override void LoadBattle(IArchiveBattleData archiveBattleData)
    {
        // //没有存档模式.
        // throw new NotImplementedException();
    }

    public override void Update() { }
    
    
    private void SetChapterData(int level,int room,int gold,IBattleLevelStructData levelStructData)
    {
        int cpid = _adventureRulesData.AdventureStructData.ChapterIds[level];
        RuntimeData.SetChapterData(cpid, level, room);
        RuntimeData.SetGold(gold);
        RuntimeData.SetLevelStructData(levelStructData);
    }

    protected override void OnEnterNextLevel()
    {
        int cpid = _adventureRulesData.AdventureStructData.ChapterIds[CurrentLevelIndex];
        RuntimeData.SetCpId(cpid);
        LoadRoomConnect();
        EnvironmentManager.Inst.SetEnvironment(_adventureRulesData.AdventureStructData.EnvironmentDatas[CurrentLevelIndex]);
    }
    
    
    protected override void LoadRoomConnect()
    {
        CreateRoomConnect(string.Format(_adventureRulesData.ConnectPrefabName,CurrentChapterId), connect =>
        {
            if (connectOffsetObject != null)
                GameObject.Destroy(connectOffsetObject.gameObject);
            connectOffsetObject = connect;
        });
    }
    
    
    
}




