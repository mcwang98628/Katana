using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleTool 
{
    #region 查找敌人

    ///查找最近的敌人
    public static RoleController FindNearestEnemy(Transform transform,float findDis = 99999999)
    {
        float dis = 999999999f;
        RoleController minRole = null;
        Dictionary<string, RoleController> targets;
        targets = BattleManager.Inst.EnemyTeam;
        foreach (KeyValuePair<string, RoleController> target in targets)
        {
            if (target.Value.IsDie)
            {
                continue;
            }
            Vector3 v3 = target.Value.transform.position - transform.position;
            if (v3.magnitude > findDis)
            {
                continue;
            }
            if (v3.magnitude < dis)
            {
                dis = v3.magnitude;
                minRole = target.Value;
            }
        }
        return minRole;
    }
    ///查找最近的敌人
    public static RoleController FindNearestEnemy(RoleController role, float findDis = 99999999)
    {
        var roleList = GetEnemysInDistance(role,findDis);
         float dis = 999999999f;
        RoleController minRole = null;
        for (int i = 0; i < roleList.Count; i++)
        {
            float magnitude = (roleList[i].transform.position - role.transform.position).magnitude;
            if (magnitude < dis)
            {
                dis = magnitude;
                minRole = roleList[i];
            }
        }
        return minRole;
    }
    ///随机敌人
    public static RoleController GetRandomEnemy(RoleController role,float distance=999)
    {
        RoleController result = null;
        var roleList = GetEnemysInDistance(role,distance);
        if (roleList.Count > 0)
            result = roleList[Random.Range(0, roleList.Count)];

        return result;
    }
    public static List<RoleController> GetEnemysInDistance(RoleController role, float Dis)
    {
        List<RoleController> result = new List<RoleController>();
        Dictionary<string, RoleController> targets;
        if (role.roleTeamType == RoleTeamType.Player)
        {
            targets = BattleManager.Inst.EnemyTeam;
        }
        else
        {
            targets = BattleManager.Inst.PlayerTeam;
        }
        
        foreach (KeyValuePair<string, RoleController> target in targets)
        {
            if (target.Value.IsDie)
            {
                continue;
            }
            Vector3 v3 = target.Value.transform.position - role.transform.position;
            if (v3.magnitude < Dis)
            {
                result.Add(target.Value);
            }

        }
        return result;
    }
    public static List<RoleController> GetRandomEnemys(RoleController role,int maxCount,int maxDistance=11)
    {
        if (maxCount < 1)
            return null;

        List<RoleController> result = new List<RoleController>();
        var roleList = GetEnemysInDistance(role,maxDistance);
        for (int i = 0; i < maxCount; i++)
        {
            if (roleList.Count < 1)
                break;
            int rand = Random.Range(0, roleList.Count);
            result.Add(roleList[rand]);
            roleList.RemoveAt(rand);
        }
        return result;
    }
    
    #endregion
    
    public static void CreateEnemy(EnemyInfoData enemyInfo,Action<EnemyController> callback)
    {
        if (enemyInfo == null)
        {
            return;
        }
        string path = "Assets/AssetsPackage/EnemyPrefabs/{0}.prefab";
        ResourcesManager.Inst.GetAsset<GameObject>(String.Format(path, enemyInfo.EnemyPrefabName),
            delegate(GameObject obj)
            {
                if (obj == null)
                {
                    Debug.LogError("怪物预制体找不到" + enemyInfo.EnemyPrefabName);
                }
                
                EnemyController enemyprefab = obj.GetComponent<EnemyController>();
                EnemyController enemyGo = GameObject.Instantiate(enemyprefab);
                enemyGo.SetUniqueID(enemyInfo.EnemyID);
                enemyGo.transform.localScale = new Vector3(1f / enemyGo.transform.lossyScale.x, 1f / enemyGo.transform.lossyScale.y, 1f / enemyGo.transform.lossyScale.z);
                enemyGo.Init();
                
                
                // //增强怪逻辑
                // if (enemyInfo.EnemyType != EnemyType.Boss&&enemyInfo.EnemyType!= EnemyType.Elite&&
                //     BattleManager.Inst.RuntimeData.RandomEnhancedEnemy())
                // {
                //     enemyGo.transform.localScale = Vector3.one * 1.25f;
                //     // AttributeBonus maxHp = new AttributeBonus();
                //     // maxHp.Type = AttributeType.MaxHp;
                //     // maxHp.Value = enemyGo.OriginalMaxHp<240? enemyGo.OriginalMaxHp*3f: enemyGo.OriginalMaxHp*2f;
                //     // enemyGo.AddAttributeBonus(maxHp);
                //     // AttributeBonus attackSpeed = new AttributeBonus();
                //     // attackSpeed.Type = AttributeType.AttackSpeed;
                //     // attackSpeed.Value = enemyGo.OriginalAttackSpeed * 0.5f;
                //     // enemyGo.AddAttributeBonus(attackSpeed);
                //     enemyGo.roleHealth.SetIsAcceptInterruption(false);
                //     enemyGo.HpTreatment(new TreatmentData(9999,enemyGo.TemporaryId));
                //     enemyGo.roleNode.Set_RimColor(Color.red*0.6f);
                //     ResourcesManager.Inst.GetAsset<GameObject>("Assets/AssetsPackage/CommonFX/VFX_Enemy_StrongFire.prefab",
                //         delegate(GameObject fxPrefab)
                //         {
                //             GameObject.Instantiate(fxPrefab,enemyGo.transform).transform.localPosition = new Vector3(0,0.1f,0);
                //         });
                // }

                
                
                
                if (BattleManager.Inst.RuntimeData is ChapterRulesRuntimeData cpRntimeData)
                {
                    var diffData = DataManager.Inst.GetEnemyDiffData(enemyInfo.EnemyID,cpRntimeData.CurrentChapterId, cpRntimeData.CurrentLevelIndex+1);
                    if (diffData != null)
                    {
                        enemyGo.InitAttacker(diffData.AttackPower,1);
                        enemyGo.InitMaxHp(diffData.MaxHp);
                    }
                }
                
                callback?.Invoke(enemyGo);
            });
    }
    
    
    public static void CreateEnemy(int enemyId,Action<EnemyController> callback)
    {
        var info = DataManager.Inst.GetEnemyInfo(enemyId);
        if (info == null)
        {
            Debug.LogError("Err:EnemyID不存在"+enemyId);
            return;
        }
        CreateEnemy(info,callback);
    }
    public static bool AllEnemyDie()
    {
        Dictionary<string, RoleController> targets;
        targets = BattleManager.Inst.EnemyTeam;
        foreach (KeyValuePair<string, RoleController> target in targets)
        {
            if (!target.Value.IsDie)
            {
                return false;
            }
        }
        return true;
    }
    
    
    /// <summary>
    /// 世界方向转化本地的动画方向
    /// </summary>
    /// <param name="dirV2">世界方向</param>
    public static Vector2 GetAnimDir(Transform animTransform,Vector2 dirV2)
    {
        var forward = animTransform.forward;
        var right = animTransform.right;
        Vector2 v2forward = new Vector2(forward.x, forward.z);
        Vector2 v2right = new Vector2(right.x, right.z);
        float y = Vector2.Dot(v2forward, dirV2.normalized);
        float x = Vector2.Dot(v2right, dirV2.normalized);
        return new Vector2(x, y);
    }
    /// <summary>
    /// 本地方向转世界方向
    /// </summary>
    /// <param name="dirV2"></param>
    /// <returns></returns>
    public static Vector2 GetWorldDir(Transform animTransform,Vector2 dirV2)
    {
        float angle = Vector3.Angle(animTransform.forward, Vector3.forward); //求出两向量之间的夹角
        Vector3 normal = Vector3.Cross(animTransform.forward, Vector3.forward);//叉乘求出法线向量
        angle *= Mathf.Sign(Vector3.Dot(normal, Vector3.up));  //求法线向量与物体上方向向量点乘，结果为1或-1，修正旋转方向
        Vector3 newVec = Quaternion.AngleAxis(-angle, Vector3.up) * new Vector3(dirV2.x, 0, dirV2.y);
        return new Vector2(newVec.x, newVec.z);
    }


    // public static void CheckItemUnLock()
    // {
    //     if (BattleManager.Inst.RuntimeData == null || BattleManager.Inst.RuntimeData.LevelStructData.LevelStructType != LevelStructType.Chapter)
    //     {
    //         return;
    //     }
    //     ChapterRulesRuntimeData runtimeData = (ChapterRulesRuntimeData)BattleManager.Inst.RuntimeData;
    //     int currentLevelId = runtimeData.CurrentLevelIndex + 1;
    //     int currentRoomId = runtimeData.CurrentRoomIndex;
    //     foreach (ChapterItemUnLockData unLockData in DataManager.Inst.ChapterItemUnLocks)
    //     {
    //         if (unLockData.ChapterId != runtimeData.CurrentChapterId)
    //             continue;
    //         if (unLockData.LevelId<currentLevelId)
    //         {
    //             ArchiveManager.Inst.UnLockItem(unLockData.ItemId);
    //         }
    //         else if (unLockData.LevelId == currentLevelId && unLockData.RoomId <= currentRoomId)
    //         {
    //             ArchiveManager.Inst.UnLockItem(unLockData.ItemId);
    //         }
    //     }
    // }
    
    /// <summary>
    /// 生成章节结构数据
    /// </summary>
    /// <param name="cpId">章节ID</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">章节结构数据</exception>
    public static void GenerateChapterStructData(int cpId,Action<ChapterStructData> callback)
    {
        DataManager.Inst.GetCpData(cpId).LoadChapterData(delegate(ChapterData data)
        {
            callback?.Invoke((ChapterStructData)GenerateChapterStructData(data));
        });
    }
    public static IBattleLevelStructData GenerateChapterStructData(ChapterData chapterData)
    {
        IBattleLevelStructData cpData = null;
        var cpDataObj = chapterData;
        switch (cpDataObj.levelStructType)
        {
            case LevelStructType.Chapter:
                // if (chapterData.IsTutorial)
                // {
                //     cpData = LevelStructGenerator.GenerateTutorialChapterStruct(cpDataObj);
                // }
                // else
                // {
                //     cpData = LevelStructGenerator.GenerateChapterStruct(cpDataObj);
                // }
                cpData = LevelStructGenerator.GenerateChapterStruct(cpDataObj);
                break;
            case LevelStructType.OneRoomEndless:
                cpData = LevelStructGenerator.GenerateEndlessChapterStruct(cpDataObj);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return cpData;
    }

    
    public static void CreatePlayer(string prefabName,Action<PlayerController> callback)
    {
        ResourcesManager.Inst.GetAsset<GameObject>($"Assets/AssetsPackage/PlayerPrefabs/{prefabName}.prefab",
            delegate(GameObject prefab)
            {
                GameObject playerGo = GameObject.Instantiate(prefab);
                var player = playerGo.GetComponent<PlayerController>();
                player.Init();
                
                callback?.Invoke(player);
            });
    }

    public static void CreatePlayerCamera(Action<CameraController> callback)
    {
        ResourcesManager.Inst.GetAsset<GameObject>("Assets/BundleAssets/Prefabs/BattleCamera.prefab",
            delegate(GameObject prefab)
            {
                var cc = GameObject.Instantiate(prefab).GetComponent<CameraController>();
                callback?.Invoke(cc);
            });
    }

    
}
