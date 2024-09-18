using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// 类章节模式
/// </summary>
public abstract class SimilarChapterRules:BaseRules
{
    public abstract override LevelStructType BattleRulesType { get; }
    
    protected abstract string RoomPrefabPath { get; }

    public SimilarChapterRules()
    {
        EventManager.Inst.AddEvent(EventName.SelectHero, OnChangeHero);
        EventManager.Inst.AddEvent(EventName.HeroUpgradedeColorLevel, UpdatePlayer);
        EventManager.Inst.AddEvent(EventName.HeroUpgradede, UpdatePlayer);
    }
    public override void StartGame()
    {
        TimeManager.Inst.SetTimeScale(1);
        AddEvent();
    }

    public override void EndBattle()
    {
        RemoveEvent();
        if (ConnectOffsetObject != null)
            GameObject.Destroy(ConnectOffsetObject.gameObject);
        
        if (CurrentRoom != null)
            GameObject.Destroy(CurrentRoom.gameObject);
        
    }

    public abstract override void LoadBattle(IBattleRulesData rulesData);

    public abstract override void LoadBattle(IArchiveBattleData archiveBattleData);

    public abstract override void Update();
    
    
    public override RoomController CurrentRoom => currentRoomObject;
    protected RoomController currentRoomObject;
    protected RoomController nextRoomObject;

    protected void InitPlayer()
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
    }
    
    
    protected virtual IEnumerator LoadBattle(Action callBack = null)
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

        StartRoom(delegate { InitPlayer(); });
        

        UIManager.Inst.HideMask(null);

        while (CurrentPlayer == null)
        {
            yield return null;
        }
        callBack?.Invoke();

        yield return null;
        EventManager.Inst.DistributeEvent(EventName.OnChapterRulesLoadBattleOver);
    }

    protected IEnumerator WaitDoCamera()
    {
        yield return null;
        
        BattleManager.Inst.DoCamera();
    }


    #region RuntimeData

    public ChapterRulesRuntimeData RuntimeData => (ChapterRulesRuntimeData)BattleManager.Inst.RuntimeData;
    public ChapterStructData CurrentChapterStructData => (ChapterStructData)RuntimeData.LevelStructData;
    
    public int CurrentChapterId => RuntimeData.CurrentChapterId;
    public int CurrentLevelIndex => RuntimeData.CurrentLevelIndex;
    public int CurrentRoomIndex => RuntimeData.CurrentRoomIndex;
    public bool IsTutorial => RuntimeData.IsTutorial;
    
    #endregion
    
    #region 初始化Player

    public override void UpdateHeroData()
    {
        if (IsArchiveBattle)
        {
            return;
        }
        
        int heroId = ArchiveManager.Inst.ArchiveData.GlobalData.LastSelectHeroID;
        if (RuntimeBaseData.CurrentHeroId == heroId)
        {
            return;
        }
        RuntimeBaseData.SetHeroData(heroId);
        InitPlayer();
    }


    protected void InitPlayerItem()
    {
        InitPlayerAttributes();
        if (IsTutorial)
        {
            //复活
            CurrentPlayer.roleItemController.AddItem(
                DataManager.Inst.ParsingItemObj(DataManager.Inst.GetItemScrObj(253)), isOk => { });
        }
        if (IsArchiveBattle)
        {
            PlayerAddArchiveItem();
        }
        else
        {
            AddPlayerItem();
            PlayerAddGlobalRuneItem();
        }
        AddAttributes();
        PlayerAddEquipment();
    }
    void PlayerAddArchiveItem()
    {
        var playerData = ArchiveManager.Inst.ArchiveData.BattleData.PlayerData;
        CurrentPlayer.roleItemController.RemoveAllItem();
        for (int i = 0; i < playerData.Items.Count; i++)
        {
            CurrentPlayer.roleItemController
                .AddItem(
                    DataManager.Inst.ParsingItemObj(DataManager.Inst.GetItemScrObj(playerData.Items[i])),
                    isOk => { }
                );
        }
        CurrentPlayer.SetCurrentHp(playerData.CurrentHp);
    }
    
    #endregion
    
    #region 游戏结束

    private void OnEnterGameOverDoor(string arg1, object arg2)
    {
        GameOver(true,null);
    }

    private void OnRoleDead(string arg1, object arg2)
    {
        RoleDeadEventData roleDeadEventData = (RoleDeadEventData) arg2;
        if (roleDeadEventData.DeadRole.TemporaryId == CurrentPlayer.TemporaryId)
        {
            if (IsTutorial) //复活
            {
                BattleManager.Inst.StartCoroutine(WaitResurrection());
            }
            else
            {
                BattleManager.Inst.StartCoroutine(WaitGameOver(roleDeadEventData));
            }
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
    
    //延迟复活
    IEnumerator WaitResurrection()
    {
        yield return new WaitForSecondsRealtime(2f);
        BattleManager.Inst.Resurrection();
    }

    public void QuitGame()
    {
        GameOver(false, null);
    }
    void GameOver(bool isVictory, RoleDeadEventData roleDeadEventData = null)
    {
        BattleManager.Inst.GameIsRuning = false;
        UIManager.Inst.Close("BattlePanel");
        FeedbackManager.Inst.StopAllCoroutines();
        
        RemoveEvent();
        
        GameOverData gameOverData = new GameOverData();
        gameOverData.isVictory = isVictory; //玩家没死游戏结束就是胜利
        gameOverData.KillEnemyNumber = RuntimeData.KillEnemyNumber;
        gameOverData.KillSEnemyNumber = RuntimeData.KillSEnemyNumber + (isVictory?1:0);
        gameOverData.Progress = RuntimeData.Progress;
        gameOverData.ClearRoomNuber = RuntimeData.ClearRoomNumber;
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
        //Unlock Item
        if (gameOverData.isVictory)
        {
            foreach (int itemId in DataManager.Inst.ChapterTableDatas[CurrentChapterId].UnlockItemIds)
            {
                ArchiveManager.Inst.UnLockItem(itemId);
            }
        }

        gameOverData.LevelIndex = CurrentLevelIndex;

        // gameOverData.Exp = 0;
        // gameOverData.SoulValue = 0;
        
        // bool clearChapter =
        //     ArchiveManager.Inst.ArchiveData.StatisticsData.ChapterClearanceDatas.ContainsKey(CurrentChapterId);
        // if (gameOverData.isVictory && 
        //     (!clearChapter || 
        //      ArchiveManager.Inst.ArchiveData.StatisticsData.ChapterClearanceDatas[CurrentChapterId].Schedule < 100))
        // {
        //     // gameOverData.Exp += 120 + CurrentChapterId * 80;
        //     // gameOverData.SoulValue += 80 + CurrentChapterId * 20;
        // }

        // int soulLevelCount = 0;//新纪录用的 Level层数差
        // if (!clearChapter)
        // {
        //     soulLevelCount = CurrentLevelIndex + 1;
        // }else
        // {
        //     soulLevelCount = CurrentLevelIndex - ArchiveManager.Inst.ArchiveData.StatisticsData
        //         .ChapterClearanceDatas[CurrentChapterId].LastLevel;
        // }
        // if (soulLevelCount < 0)
        //     soulLevelCount = 0;

        // gameOverData.SoulValue += soulLevelCount * 30;
        //
        // gameOverData.KillEnemyDiamond = gameOverData.KillEnemyNumber * 1;
        // gameOverData.KillSEnemyDiamond = gameOverData.KillSEnemyNumber*30;
        // gameOverData.LevelDiamond = CurrentLevelIndex * 20 + (gameOverData.isVictory ? 20 : 0);
        // gameOverData.BattleWinDiamond = gameOverData.isVictory?100:0;
        //
        // gameOverData.KillEnemySoul = Mathf.CeilToInt(gameOverData.KillEnemyNumber * 0.1f);
        // gameOverData.KillSEnemySoul = Mathf.CeilToInt(gameOverData.KillSEnemyNumber * 20);
        // gameOverData.BattleWinSoul = gameOverData.isVictory?50:0;
        // gameOverData.LevelSoul = CurrentLevelIndex * 10 + (gameOverData.isVictory ? 10 : 0);


        gameOverData.GetDiamond = RuntimeData.DiamondCount;
        ArchiveManager.Inst.ArchiveData.TemporaryData.AddDiamondValue = gameOverData.GetDiamond;
        
        // ArchiveManager.Inst.ArchiveData.TemporaryData.AddSoulValue 
        //     = gameOverData.SoulValue + gameOverData.KillEnemySoul + gameOverData.KillSEnemySoul + gameOverData.BattleWinSoul;
        // ArchiveManager.Inst.ArchiveData.TemporaryData.AddExpValue = gameOverData.Exp;
        
        ArchiveManager.Inst.ArchiveData.TemporaryData.EquipmentList.AddRange(BattleManager.Inst.RuntimeData.GetEquipmentList);
        
        if (gameOverData.isVictory)
        {
            int chapterId = RuntimeData.CurrentChapterId + 1;
            if (DataManager.Inst.ChapterTableDatas.ContainsKey(chapterId))
            {
                ArchiveManager.Inst.ArchiveData.GlobalData.LastSelectChapterId = chapterId;
            }
        }
        
        UIManager.Inst.Open("BattleOverPanel", true, gameOverData);
        EventManager.Inst.DistributeEvent(EventName.OnBattleOver, gameOverData);
        ArchiveManager.Inst.AddGameTime(BattleManager.Inst.BattleTime);

        if (gameOverData.isVictory)
        {
            ArchiveManager.Inst.ChapterClearance(CurrentChapterId, 100,CurrentLevelIndex);
            if (IsTutorial)
            {
                ArchiveManager.Inst.ArchiveData.GlobalData.ThroughTutorial = true;
            }
        }
        else
        {
            ArchiveManager.Inst.ChapterClearance(CurrentChapterId, RuntimeData.Progress,CurrentLevelIndex);
        }
        
        ArchiveManager.Inst.SaveArchive();
    }
    

    #endregion

    #region 创建房间

    //从存档进入 第一个房间
    protected bool isArchiveBattleStartRoom;

    protected virtual void OnEnterNextLevel()
    {
        
    }

    void SetCurrentRoom(RoomController room)
    {
        currentRoomObject = room;
        if (currentRoomObject.RoomType == RoomType.StartRoom)
            SetConnectPosition(currentRoomObject.GetDoorPos());
    }
    
    protected virtual void StartRoom(Action callBack)
    {
        if (currentRoomObject != null)
        {
            GameObject.Destroy(currentRoomObject.gameObject);
        }

        if (nextRoomObject != null)
        {
            GameObject.Destroy(nextRoomObject.gameObject);
        }

        if (CurrentLevelIndex != 0 && !isArchiveBattleStartRoom)
        {
            switch (RuntimeData.LevelStructData.LevelStructType)
            {
                case LevelStructType.None:
                    break;
                case LevelStructType.Chapter:
                    UIManager.Inst.Open("NextLevelPanel", true, CurrentLevelIndex);
                    break;
                case LevelStructType.OneRoomEndless:
                    break;
                case LevelStructType.Adventure:
                    UIManager.Inst.Open("NextLevelPanel_Adventure", true, CurrentLevelIndex);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            } 
            EventManager.Inst.DistributeEvent(EventName.EnterNextLevel);
            OnEnterNextLevel();
        }
        if (isArchiveBattleStartRoom && IsArchiveBattle)
        {
            isArchiveBattleStartRoom = false;
        }
        
        CreateRoomObject(CurrentChapterStructData.RoomList[CurrentLevelIndex][CurrentRoomIndex],
            delegate(RoomController roomController)
            {
                // currentRoomObject = roomController;
                SetCurrentRoom(roomController);
                currentRoomObject.transform.position = Vector3.zero;
        
                RuntimeData.PassingRooms.Add(RoomType.StartRoom);
        
                BattleManager.Inst.StartCoroutine(RestPlayerTrans("Entrance_Z"));
                if (RuntimeData.IsTutorial)
                    BattleManager.Inst.StartCoroutine(waitBattleGuide());
                    
                callBack?.Invoke();
            });
        
        
    }

    IEnumerator waitBattleGuide()
    {
        yield return null;
        BattleGuide.Inst.StartRoomGuide(RoomType.StartRoom,RuntimeData.GetPassingRoomTimes(RoomType.StartRoom));
    }
    IEnumerator RestPlayerTrans(string enterPoint)
    {
        yield return null;
        if (CurrentPlayer != null)
        {
            var v3 = GameObject.Find(enterPoint).transform.position;
            v3.y = 0;
            CurrentPlayer.transform.position = v3;
        }
    }

    #endregion
    
    #region 房间移动


    public RoomConnect ConnectOffsetObject => connectOffsetObject;
    protected RoomConnect connectOffsetObject;

    protected abstract void LoadRoomConnect();
    
    protected void CreateRoomConnect(string prefabName,Action<RoomConnect> callback)
    {
        //"Assets/Arts/RoomPrefabs/Rooms/Chapter{0}/Room_Chapter{1}_Connect.prefab"
        
        //创建连接
        ResourcesManager.Inst.GetAsset<GameObject>($"Assets/Arts/RoomPrefabs/Rooms/Connects/{prefabName}.prefab",
            delegate(GameObject prefab)
            {
                var roomConnect = GameObject.Instantiate(prefab, new Vector3(0, -10, 0), Quaternion.Euler(Vector3.zero))
                    .GetComponent<RoomConnect>();
                GameObject.DontDestroyOnLoad(roomConnect.gameObject);
                callback.Invoke(roomConnect);
            });
    }

    private void OnEnterLastDoor(string arg1, object arg2)
    {
        UIManager.Inst.ShowMask(() =>
        {
            RuntimeData.EnterNextRoom();
            StartRoom(delegate
            {
                UIManager.Inst.HideMask(null);
            });
        });
    }
    private void OnEnterNextRoomEvent(string arg1, object arg2)
    {
        OnEnterNextRoom();
    }
    private void OnShowNextRoom(string arg1, object arg2)
    {
        ShowNextRoom();
    }

    private void OnHideCurrentRoom(string arg1, object arg2)
    {
        HideCurrentRoom();
    }
    private void OnEnterDoor(string arg1, object arg2)
    {
        EnterDoorData doorData = (EnterDoorData) arg2;
        OnEnterDoor(doorData.Position,doorData.RoomData);
    }
    
    /// <summary>
    /// 进门时
    /// </summary>
    /// <param name="exitPoint"></param>
    /// <param name="roomInfo"></param>
    public void OnEnterDoor(Vector3 exitPoint, RoomData roomInfo)
    {
        EventManager.Inst.DistributeEvent(EventName.ExitRoom);
        // currentPlayer.SetAcceptInput(false);
        // Vector3 offset = new Vector3(0, 0, 2.29f);
        // ConnectOffsetObject.transform.position = new Vector3(exitPoint.x, -10, exitPoint.z) + offset;
        SetConnectPosition(exitPoint);
        //走廊上升
        ConnectOffsetObject.Show();

        CreateRoomObject(roomInfo, delegate(RoomController roomController)
        {
            nextRoomObject = roomController;
            BattleManager.Inst.StartCoroutine(WaitMoveRoom(exitPoint, nextRoomObject));
        });
    }

    public void SetConnectPosition(Vector3 pos)
    {
        Vector3 offset = new Vector3(0, 0, 2.29f);
        // ConnectOffsetObject.transform.position = new Vector3(pos.x, -10, pos.z) + offset;
        ConnectOffsetObject.SetPosition(new Vector3(pos.x, -10, pos.z) + offset);
    }

    IEnumerator WaitMoveRoom(Vector3 exitPoint, RoomController nextRoomObject)
    {
        nextRoomObject.transform.position = new Vector3(0, -10, 0);
        yield return null;
        var enterPos = RoomFindChild(nextRoomObject.transform, "Entrance_Z").position;
        var roomEnterOffset = nextRoomObject.transform.position - enterPos;
        var offset = new Vector3(0, 0, ConnectOffsetObject.ConnectLength);
        Vector3 targetV3 = exitPoint + new Vector3(0, 0, 2.29f) + offset;
        targetV3 += roomEnterOffset;
        nextRoomObject.transform.position = new Vector3(targetV3.x, -10, targetV3.z);
    }

    public void HideCurrentRoom()
    {
        //上一个房间下降
        currentRoomObject.transform.DOMoveY(-10f, .75f).SetEase(Ease.InBack).OnComplete(() =>
        {
            GameObject.Destroy(currentRoomObject.gameObject);
        });

        EventManager.Inst.DistributeEvent(EventName.UI_EnterNextRoom, nextRoomObject.CurrentRoomData);
    }

    public void ShowNextRoom()
    {
        var room = nextRoomObject;
        //下一个房间上升
        nextRoomObject.transform.DOMoveY(0, .75f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            SetConnectPosition(room.GetDoorPos());
        });
        nextRoomObject.ShowHideEnterObstacleBox(false);
    }

    public void OnEnterNextRoom()
    {
        nextRoomObject.ShowHideEnterObstacleBox(true);
        //////
        if (currentRoomObject != null)
        {
            currentRoomObject.DOKill(true);
            GameObject.Destroy(currentRoomObject.gameObject);
        }

        // currentRoomObject = nextRoomObject;
        SetCurrentRoom(nextRoomObject);
        nextRoomObject = null;

        if (currentRoomObject.RoomType == RoomType.FightRoom ||
            currentRoomObject.RoomType == RoomType.BossFightRoom)
        {
            ((FightRoom) currentRoomObject).StartCreateEnemy();
        }

        ConnectOffsetObject.Hide();
        RuntimeData.EnterNextRoom();
        EventManager.Inst.DistributeEvent(EventName.EnterNextRoom, currentRoomObject.CurrentRoomData);
        if (currentRoomObject.RoomType == RoomType.FightRoom)
        {
            EventManager.Inst.DistributeEvent(EventName.EnterFightRoom, currentRoomObject.CurrentRoomData);
        }

        var roomType = currentRoomObject.CurrentRoomData.RoomType;
        RuntimeData.PassingRooms.Add(roomType);
        if (RuntimeData.IsTutorial)
        {
            BattleGuide.Inst.StartRoomGuide(roomType,RuntimeData.GetPassingRoomTimes(roomType));
        }
    }
    
    Transform RoomFindChild(Transform trans, string name)
    {
        Transform child = trans.Find(name);
        if (child == null)
        {
            Transform roomframe = trans.Find("RoomFrame");
            if (roomframe != null)
            {
                child = roomframe.Find(name);
            }
        }

        return child;
    }

    
    
    protected void CreateRoomObject(RoomData roomInfo,Action<RoomController> callBack)
    {
        if (string.IsNullOrEmpty(roomInfo.RoomPrefabName))
        {
            Debug.LogError("#Err# 房间NULL错误!");
            return;
        }
        string roomPrefabPath = string.Format(RoomPrefabPath, CurrentChapterId, roomInfo.RoomPrefabName);

        ResourcesManager.Inst.GetAsset<GameObject>(roomPrefabPath, delegate(GameObject roomPrefab)
        {
            if (roomPrefab == null)
            {
                Debug.LogError(roomPrefabPath);
            }
            var roomController = GameObject.Instantiate(roomPrefab, new Vector3(0, -10, 0), roomPrefab.transform.rotation)
                .GetComponent<RoomController>();
            //设置房间类型
            roomController.SetRoomInfo(roomInfo);

            //战斗房间独享逻辑
            if (roomInfo.RoomType == RoomType.FightRoom ||
                roomInfo.RoomType == RoomType.BossFightRoom)
            {
                if (roomController is FightRoom room &&
                    roomInfo.EnemyWaves != null)
                {
                    List<List<int>> enemyNames = new List<List<int>>();
                    for (int i = 0; i < roomInfo.EnemyWaves.Count; i++)
                    {
                        enemyNames.Add(new List<int>());
                        for (int j = 0; j < roomInfo.EnemyWaves[i].Count; j++)
                        {
                            enemyNames[i].Add(roomInfo.EnemyWaves[i][j]);
                        }
                    }

                    room.SetEnemyList(enemyNames);
                }
            }

            if (roomInfo.CurrentFloorIndex >= CurrentChapterStructData.RoomList.Count)
            {
                Debug.LogError("Err:不太对...");
                roomController.InitGameOverDoor();
            }
            else
            {
                int levelIndex, RoomIndex;
                if (CurrentChapterStructData.RoomList[roomInfo.CurrentFloorIndex].Count > roomInfo.CurrentRoomIndex + 1)
                {
                    RoomIndex = roomInfo.CurrentRoomIndex + 1;
                    levelIndex = roomInfo.CurrentFloorIndex;
                }
                else
                {
                    levelIndex = roomInfo.CurrentFloorIndex + 1;
                    RoomIndex = 0;
                }

                if (CurrentChapterStructData.RoomList.Count > levelIndex)
                {
                    roomController.InitDoor(CurrentChapterStructData.RoomList[levelIndex][RoomIndex],
                        (RoomIndex == 0 && levelIndex != 0));
                }
                else
                {
                    roomController.InitGameOverDoor();
                }
            }
            
            callBack?.Invoke(roomController);
        });
        
    }
    
#endregion
    
    #region Event

    
    protected virtual void AddEvent()
    {
        EventManager.Inst.AddEvent(EventName.OnRoleDead, OnRoleDead);
        EventManager.Inst.AddEvent(EventName.EnterGameOverDoor, OnEnterGameOverDoor);
        EventManager.Inst.AddEvent(EventName.EnterLastDoor, OnEnterLastDoor);
        EventManager.Inst.AddEvent(EventName.EnterDoor, OnEnterDoor);
        EventManager.Inst.AddEvent(EventName.HideCurrentRoom, OnHideCurrentRoom);
        EventManager.Inst.AddEvent(EventName.ShowNextRoom, OnShowNextRoom);
        EventManager.Inst.AddEvent(EventName.OnEnterNextRoom, OnEnterNextRoomEvent);
    }

    protected virtual void RemoveEvent()
    {
        EventManager.Inst.RemoveEvent(EventName.SelectHero, OnChangeHero);
        EventManager.Inst.RemoveEvent(EventName.HeroUpgradedeColorLevel, UpdatePlayer);
        EventManager.Inst.RemoveEvent(EventName.HeroUpgradede, UpdatePlayer);
        EventManager.Inst.RemoveEvent(EventName.OnRoleDead, OnRoleDead);
        EventManager.Inst.RemoveEvent(EventName.EnterGameOverDoor, OnEnterGameOverDoor);
        EventManager.Inst.RemoveEvent(EventName.EnterLastDoor, OnEnterLastDoor);
        EventManager.Inst.RemoveEvent(EventName.EnterDoor, OnEnterDoor);
        EventManager.Inst.RemoveEvent(EventName.HideCurrentRoom, OnHideCurrentRoom);
        EventManager.Inst.RemoveEvent(EventName.ShowNextRoom, OnShowNextRoom);
        EventManager.Inst.RemoveEvent(EventName.OnEnterNextRoom, OnEnterNextRoomEvent);
    }
    protected void OnChangeHero(string arg1, object arg2)
    {
        int heroId = (int) arg2;
        if (CurrentPlayer.UniqueID != heroId)
        {
            InitPlayer();
        }
    }
    
    protected void UpdatePlayer(string arg1, object arg2)
    {
        InitPlayer();
    }
    

    #endregion

}