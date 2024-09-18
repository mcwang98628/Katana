//战斗房
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;

using Random = UnityEngine.Random;
public class FightRoom : RoomController
{
    // //随机生成敌人相关
    [HideInInspector]
    public Transform Enemypoints;
    private List<List<int>> enemyData = new List<List<int>>();

    
    
    protected int currentEnemvWaveID;

    GameObject spwanVFX;
    void Start()
    {
        //获取出怪点
        Enemypoints = transform.Find("EnemySpawnPoints");
        EventManager.Inst.AddEvent(EventName.OnRoleDead, OnRoleDead);

        ResourcesManager.Inst.GetAsset<GameObject>("Assets/BundleAssets/Prefabs/VFX_Enemy_Appear.prefab",
            delegate(GameObject o)
            {
                spwanVFX = o;
            });
    }

    
    public bool CheckRoomFinished()
    {
        if (BattleTool.AllEnemyDie())
        {
            return currentEnemvWaveID == enemyData.Count;
        }
        return false;
    }

    //当角色死亡时调用
    protected void OnRoleDead(string eventName, object value)
    {
        if (CheckRoomFinished())
        {
            StartCoroutine(WaitDistributeEvent());
            StartCoroutine(WaitCreateReward(1));
            StartCoroutine(OpenDoor(1.5f));
        }
    }

    protected virtual IEnumerator WaitCreateReward(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        switch (CurrentRoomData.RoomInfoConfig.RoomRewardType)
        {
            case RoomRewardType.None:
                break;
            case RoomRewardType.Item:
                ResourcesManager.Inst.GetAsset<GameObject>("Assets/BundleAssets/Prefabs/InteractObj_Chest_Normal.prefab",
                    delegate(GameObject o)
                    {
                        GameObject dropItemChest = o;
                        if (dropItemChest != null) //找到最近的一个刷怪点 
                        {
                            Instantiate(dropItemChest, Doors[0].transform.position, Quaternion.identity).transform.SetParent(transform);
                        }
                    });
                break;
            case RoomRewardType.Money:
                GoldEffect goldEffect = new GoldEffect(CurrentRoomData.RoomInfoConfig.RewardMoney);
                goldEffect.TriggerEffect(new ItemEffectTriggerValue()
                {
                    TargetPosition = Doors[0].transform.position
                });
                break;
            case RoomRewardType.Hp:
                ResourcesManager.Inst.GetAsset<GameObject>("Assets/BundleAssets/Prefabs/PickABleObj_Potion.prefab",
                    delegate(GameObject o)
                    {
                        GameObject dropItemChest = o;
                        if (dropItemChest != null)
                        {
                            int count = CurrentRoomData.RoomInfoConfig.HpCount;//Random.Range(2, 4);
                            for (int i = 0; i < count; i++)
                            {
                                Instantiate(dropItemChest, 
                                    Doors[0].transform.position+new Vector3(Random.Range(-1f,1f),0,Random.Range(-1f,1f)),
                                    Quaternion.identity).transform.SetParent(transform);
                            }
                        }
                    });
                break;
            case RoomRewardType.GlobalDiamond:
                for (int i = 0; i < CurrentRoomData.RoomInfoConfig.GlobalDiamondCount; i++)
                    DropManager.Inst.SpwanDiamondAt(BattleManager.Inst.CurrentPlayer.transform.position);
                break;
        }
    }

    private IEnumerator WaitDistributeEvent()
    {
        yield return null;
        EventManager.Inst.DistributeEvent(EventName.ClearAllEnemy);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.Inst.RemoveEvent(EventName.OnRoleDead, OnRoleDead);
    }



    public void SetEnemyList(List<List<int>> enemyids)
    {
        this.enemyData = enemyids;

        // //动态怪物数量
        // for (int i = 0; i < enemyData.Count; i++)
        // {
        //     int enemyCount = (int)(enemyData[i].Count * BattleManager.Inst.DynamicEnemyCount - enemyData[i].Count);
        //     for (int j = 0; j < enemyCount; j++)
        //     {
        //         enemyData[i].Add(enemyData[i][Random.Range(0, enemyData[i].Count)]);
        //     }
        // }
    }

    public void StartCreateEnemy()
    {
        StartCoroutine(checkNextWaveEnemy());
    }

    IEnumerator checkNextWaveEnemy()
    {
        yield return null;
        currentEnemvWaveID = 0;
        if (enemyData.Count > 1)
        {
            EventManager.Inst.DistributeEvent(EventName.FightRoomWaveTips,$"{currentEnemvWaveID+1}/{enemyData.Count}");
        }
        yield return createEnemy();
        currentEnemvWaveID++;

        while (currentEnemvWaveID < enemyData.Count)
        {
            yield return null;
            yield return new WaitForSecondsRealtime(3.25f);
            if (BattleTool.AllEnemyDie())
            {
                if (enemyData.Count > 1)
                {
                    EventManager.Inst.DistributeEvent(EventName.FightRoomWaveTips,$"{currentEnemvWaveID+1}/{enemyData.Count}");
                }
                yield return createEnemy();
                currentEnemvWaveID++;
            }
        }
    }


    IEnumerator createEnemy()
    {
        if (Enemypoints == null)
        {
            Enemypoints = transform.Find("EnemySpawnPoints");
        }

        if (Enemypoints == null)
        {
            Debug.LogError("战斗房间没有找到刷怪点。");
            yield break;
        }
        
        //用于重置金钱上限
        EventManager.Inst.DistributeEvent(EventName.FightRoomResetEnemyWave,null);

        initEnemyPoint();
        var enemyList = enemyData[currentEnemvWaveID];

        if (RoomType == RoomType.FightRoom)
        {
            float enemyTime = 2.5f / enemyList.Count;
            enemyTime = enemyTime > 0.4f ? 0.4f : enemyTime;
            for (int i = 0; i < enemyList.Count; i++)
            {
                yield return StartCoroutine(DelaySpwanEnemy(enemyList[i], enemyTime));
            }
        }
        else if (RoomType == RoomType.BossFightRoom)
        {
            if (BattleManager.Inst.RuntimeData is ChapterRulesRuntimeData runtimeData)
            {
                if (!runtimeData.IsTutorial)
                {
                    BattleGuide.Inst.StartGuide(new List<BattleGuideSequenceData>()
                    {
                        new BattleGuideSequenceData()
                        {
                            GuideType = BattleGuideType.MoveCamera,
                            ShowTime = 2,
                            Force = true,
                            Text = Enemypoints.GetChild(0).gameObject.name
                        }
                    });
                }
            }
            yield return StartCoroutine(DelaySpwanEnemy(enemyList[0], 2));
        }
    }

    IEnumerator DelaySpwanEnemy(int enemyId, float time)
    {
        yield return new WaitForSeconds(time);
        Vector3 enemyPos = getEnemyPoint();
        if (spwanVFX)
        {
            Instantiate(spwanVFX, enemyPos + Vector3.up * 0.1f, Quaternion.identity);
        }
        BattleTool.CreateEnemy(enemyId, delegate(EnemyController enemy)
        {
            if (enemy == null)
            {
                return;
            }
            enemy.transform.position = enemyPos;
            enemy.transform.SetParent(transform);
        });
    }
    
    List<int> childCount = new List<int>();
    void initEnemyPoint()
    {
        childCount.Clear();
        for (int i = 0; i < Enemypoints.childCount; i++)
        {
            childCount.Add(i);
        }
    }
    Vector3 getEnemyPoint()
    {
        if (childCount.Count <= 0)
        {
            initEnemyPoint();
        }
        int index = childCount[Random.Range(0, childCount.Count)];
        if(RoomType==RoomType.BossFightRoom)
        {
            index=0;
        }
        childCount.Remove(index);
        return Enemypoints.GetChild(index).transform.position;
    }

}