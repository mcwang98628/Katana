using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Inst { get; private set; }

    //战场 规则
    public IBattleRules BattleRules { get; private set; }
    //战场过程中 数据
    public BattleRuntimeBaseData RuntimeData { get; private set; }

    public bool IsArchiveBattle => BattleRules.IsArchiveBattle;
    public RoomController CurrentRoom => BattleRules.CurrentRoom;

    [HideInInspector] //游戏是否开始
    public bool GameIsRuning = false;
    //本次战斗的Guid
    public Guid BattleGuid => RuntimeData.BattleGuid;

    public int CurrentGold => RuntimeData.CurrentGold;


    public bool IsShowToturialClick;

    public bool IsTutorial => RuntimeData.IsTutorial;

    public void Init()
    {
        Inst = this;
        EventManager.Inst.AddEvent(EventName.OnRoleDead,OnBossDead);
    }



    private void OnBossDead(string arg1, object arg2)
    {
        RoleDeadEventData roleDead = (RoleDeadEventData) arg2;
        if (roleDead.DeadRole.roleTeamType == RoleTeamType.Enemy_Boss || 
            roleDead.DeadRole.roleTeamType == RoleTeamType.EliteEnemy)
        {
            EventManager.Inst.DistributeEvent(EventName.OnBossDead,arg2);
        }
    }


    #region 战斗角色 物体 注册引用

    public PlayerController CurrentPlayer { get; private set; }
    public CameraController CurrentCamera { get; private set; }

    public Dictionary<string, RoleController> EnemyTeam { get; private set; } = new Dictionary<string, RoleController>();
    public Dictionary<string, RoleController> PlayerTeam { get; private set; } = new Dictionary<string, RoleController>();

    public Dictionary<BattleObjectType, Dictionary<Guid, IBattleObject>> BattleObject { get; private set; } = new Dictionary<BattleObjectType, Dictionary<Guid, IBattleObject>>();


    //投掷物
    public List<MonoBehaviour> BattleThrow => battleThrow;
    private List<MonoBehaviour> battleThrow = new List<MonoBehaviour>();

    public void AddBattleThrow(MonoBehaviour gameObject)
    {
        battleThrow.Add(gameObject);
    }

    public void RemoveBattleThrow(MonoBehaviour gameObject)
    {
        if (!battleThrow.Contains(gameObject))
        {
            return;
        }

        battleThrow.Remove(gameObject);
    }

    public void RoleRegistered(RoleController role)
    {
        if (role.roleTeamType == RoleTeamType.Player)
        {
            PlayerTeam.Add(role.TemporaryId, role);
        }
        else
        {
            EnemyTeam.Add(role.TemporaryId, role);
        }
        EventManager.Inst.DistributeEvent(EventName.OnRoleRegistered, role);
    }

    public void RoleUnRegistered(RoleController role)
    {
        if (role.roleTeamType == RoleTeamType.Player)
        {
            if (PlayerTeam.ContainsKey(role.TemporaryId))
            {
                PlayerTeam.Remove(role.TemporaryId);
            }
        }
        else
        {
            if (EnemyTeam.ContainsKey(role.TemporaryId))
            {
                EnemyTeam.Remove(role.TemporaryId);
            }
        }
    }

    public void BattleObjectRegistered(IBattleObject battleObject)
    {
        if (!BattleObject.ContainsKey(battleObject.BattleObjectType))
        {
            BattleObject.Add(battleObject.BattleObjectType, new Dictionary<Guid, IBattleObject>());
        }

        if (BattleObject[battleObject.BattleObjectType].ContainsKey(battleObject.TemporaryId))
        {
            Debug.LogError($"Err:重复注册BattleObject,{battleObject.ObjectTransform.gameObject.name}");
            return;
        }
        BattleObject[battleObject.BattleObjectType].Add(battleObject.TemporaryId, battleObject);
    }

    public void BattleObjectUnRegistered(IBattleObject battleObject)
    {
        if (
            !BattleObject.ContainsKey(battleObject.BattleObjectType) ||
            !BattleObject[battleObject.BattleObjectType].ContainsKey(battleObject.TemporaryId)
            )
        {
            Debug.LogError($"Err:查无此BattleObject,{battleObject.ObjectTransform.gameObject.name}");
            return;
        }

        BattleObject[battleObject.BattleObjectType].Remove(battleObject.TemporaryId);
        
        if (battleObject.ObjectTransform == null)
            return;
        
        EventManager.Inst.DistributeEvent(EventName.OnBattleObjectDestroy,battleObject.ObjectTransform.position);
    }

    public void ClearEnemyTeam()
    {
        EnemyTeam.Clear();
    }

    public void ClearPlayerTeam()
    {
        PlayerTeam.Clear();
    }

    public void SetPlayer(PlayerController playerController)
    {
        CurrentPlayer = playerController;
    }

    public void SetCamera(CameraController cameraController)
    {
        CurrentCamera = cameraController;
    }

    #endregion


    #region 计时器

    public float BattleTime;

    IEnumerator BattleTimer()
    {
        BattleTime = 0;
        while (GameIsRuning)
        {
            yield return null;
            BattleTime += Time.unscaledDeltaTime;
        }
    }

    #endregion



    #region 开始游戏

    private Coroutine timerCoroutine;
    public void StartGame()
    {
        GameIsRuning = true;
        ProcedureManager.Inst.StartProcedure(new BattleSceneProcedure());
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        timerCoroutine = StartCoroutine(BattleTimer());

        BattleRules.StartGame();
    }


    //进入存档
    public void LoadArchiveBattle(BattleArchiveData_1_4_0 archiveData)
    {
        if (BattleRules != null)
        {
            BattleRules.EndBattle();
        }
        //重置战斗数据
        ResetBattleData();
        BattleRules = new ChapterRules();
        RuntimeData = new ChapterRulesRuntimeData();
        RuntimeData.SetBattleGuid(archiveData.Guid);
        BattleRules.LoadBattle(archiveData);
    }

    //正常进入普通章节
    public void LoadChapterBattle(int cpId)
    {
        if (BattleRules != null)
        {
            BattleRules.EndBattle();
        }
        ResetBattleData();
        BattleRules = new ChapterRules();//TODO TEST
        // BattleRules = new AdventureRules();//TODO TEST
        RuntimeData = new ChapterRulesRuntimeData();
        RuntimeData.SetBattleGuid(Guid.NewGuid());

        BattleRules.LoadBattle(new ChapterRulesData()
        {
            ChapterId = cpId,
            ConnectPrefabName = $"Room_Chapter{cpId}_Connect"
        });
        // //TODO TEST
        // BattleTool.GenerateChapterStructData(cpId, delegate(ChapterStructData data)
        // {
        //     BattleRules.LoadBattle(new AdventureRulesData()
        //     {
        //         AdventureStructData = new AdventureStructData(){RoomList = data.RoomList},
        //         ConnectPrefabName = "Room_Chapter0_Connect"
        //     });
        // });
        

        // var cpData = DataManager.Inst.GetCpData(cpId);
        // InitDifficulity(cpData.DifficulityMaxHp,cpData.DifficulityAttackPower);

        if (cpId > 0)
        {
            ArchiveManager.Inst.ArchiveData.GlobalData.LastSelectChapterId = cpId;
            ArchiveManager.Inst.SaveArchive();
        }
        EventManager.Inst.DistributeEvent(EventName.LoadChapter,cpId);
    }

    public void LoadEndlessBattle(int cpId)
    {
        if (BattleRules != null)
        {
            BattleRules.EndBattle();
        }
        BattleRules = new EndlessRules();
        RuntimeData = new EndlessRulesRuntimeData();
        ResetBattleData();
        var cpData = DataManager.Inst.GetCpData(cpId);
        BattleRules.LoadBattle(new EndlessRulesData() { ChapterData = cpData });
        // InitDifficulity(cpData.DifficulityMaxHp,cpData.DifficulityAttackPower);

    }

    public void LoadAdventureBattle()
    {
        if (BattleRules != null)
        {
            BattleRules.EndBattle();
        }
        ResetBattleData();
        BattleRules = new AdventureRules();
        RuntimeData = new AdventureRulesRuntimeData();
        RuntimeData.SetBattleGuid(Guid.NewGuid());

        
        ResourcesManager.Inst.GetAsset<ChapterData_Adventure>("Assets/AssetsPackage/ChapterDatas/ChapterData_Adventure.asset",
            delegate(ChapterData_Adventure adventure)
            {
                BattleRules.LoadBattle(new AdventureRulesData()
                {
                    AdventureStructData = LevelStructGenerator.GenerateAdventureStruct(adventure),
                    ConnectPrefabName = "Room_Chapter{0}_Connect"
                });
            });
    }
    
    
    public void DoCamera()
    {
        DOTween.To(() => CurrentCamera.Distance, value => CurrentCamera.Distance = value, 18, 0.5f);
    }


    #endregion

    ///复活
    public bool Resurrection(float value = 1f)
    {
        if (!CurrentPlayer.IsDie)
        {
            return false;
        }

        TreatmentData td = new TreatmentData(CurrentPlayer.MaxHp*value, CurrentPlayer.TemporaryId, true);
        CurrentPlayer.HpTreatment(td);

        if (RuntimeData is ChapterRulesRuntimeData runtimeData)
        {
            if (runtimeData.CurrentChapterId == 0)
            {
                //复活
                CurrentPlayer.roleItemController.AddItem(
                    DataManager.Inst.ParsingItemObj(DataManager.Inst.GetItemScrObj(253)), isOk => { });
            }
        }

        ResourcesManager.Inst.GetAsset<FeedBackObject>(
            "Assets/AssetsPackage/ScriptObject_Combat/FeedBack/PlayerRevive.asset",
            delegate(FeedBackObject backObject)
            {
                FeedbackManager.Inst.UseFeedBack(CurrentPlayer, backObject);
                EventManager.Inst.DistributeEvent(EventName.PlayerRevive);
            });
        return true;
    }


    /// <summary>
    /// 添加金币
    /// </summary>
    public void AddGold(int value)
    {
        RuntimeData.AddGold(value);
    }

    public void AddDiamond(int value)
    {
        RuntimeData.AddDiamond(value);
    }


    #region 章节流程




    /// <summary>
    /// 重置战斗数据
    /// </summary>
    private void ResetBattleData()
    {
        if (DropManager.Inst != null)
        {
            DropManager.Inst.Luck = 1;
        }
        GameIsRuning = false;

        ClearEnemyTeam();
        ClearPlayerTeam();
        SetPlayer(null);
        SetCamera(null);

        // BattleData.IsShowToturialClick = false;
        InteractManager.Inst.Reset();
        TimeManager.Inst.SetTimeScale(1f);
    }


    public void SetShowToturialClick()
    {
        IsShowToturialClick = true;
    }

    public void EndBattle()
    {
        DestroyAllEnemyObj();
        ResetBattleData();
    }

    public void DestroyAllEnemyObj()
    {
        foreach (KeyValuePair<string, RoleController> roleController in EnemyTeam)
        {
            if (roleController.Value != null)
            {
                Destroy(roleController.Value.gameObject);
            }
        }

        ClearEnemyTeam();
    }



    #endregion



    public void UpdatePlayer()
    {
        BattleRules.UpdateHeroData();
    }


    
    


}