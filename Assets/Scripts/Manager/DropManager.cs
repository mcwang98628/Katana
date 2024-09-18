using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DropManager : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnProp
    {
        public GameObject SpawnPrefab;
        public float Possibilty;
    }
    [Header("掉落率")]
    public float Luck = 1;
    public float BreakableObjHpDropRate = 0.1f;
    public float BreakableObjCoinDropRate = 0.3f;
    [Space]
    public float EnemyDeadHpDropRate = 0.1f;
    public float EliteEnemyDeadHpDropRate = 0.5f;
    public float EnemyDeadCoinDropRate = 1f;
    public int EnemyDeadCoinMaxCount = 3;
    [Space]
    public float ChestHpDropRate = 0.5f;
    public float ChestCoinDropRate = 0.8f;
    public int ChestCoinMaxCount = 10;
    public int ChestCoinMinCount = 5;
    public int ChestDeadHpMaxCount = 3;
    [Space]
    [ReadOnly]
    [ShowInInspector]
    float coinDropRateCorrect = 0;//掉落率修正值

    [Space]
    [Header("掉落物")]
    public string PickableCoinSmallName;
    public string PickableCoinMiddleName;
    public string PickableCoinLargeName;
    public string PickableHpName;
    [LabelText("外围钻石")]
    public string PickableDiamondName;
    private string AssetPath = "Assets/BundleAssets/Prefabs/{0}.prefab";


    public int MaxFightRoomCoinCount = 30;
    [ShowInInspector]
    [ReadOnly]
    private int enemyCoinCount = 0;
    public static DropManager Inst { get; private set; }
    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        EventManager.Inst.AddEvent(EventName.OnRoleDead, EnemyDead);
        EventManager.Inst.AddEvent(EventName.OnBreakableObjBreake, BreakAbleObjBreak);
        EventManager.Inst.AddEvent(EventName.FightRoomResetEnemyWave, ResetRoomCoinCount);

    }
    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnRoleDead, EnemyDead);
        EventManager.Inst.RemoveEvent(EventName.OnBreakableObjBreake, BreakAbleObjBreak);
        EventManager.Inst.RemoveEvent(EventName.FightRoomResetEnemyWave, ResetRoomCoinCount);
    }


    void ResetRoomCoinCount(string str, object value)
    {
        enemyCoinCount = 0;
    }

    void BreakAbleObjBreak(string str, object value)
    {
        Vector3 position = (Vector3)value;

        TrySpwanCoin(position, BreakableObjCoinDropRate * Luck);
        TrySpwanHp(position, BreakableObjHpDropRate * Luck);
    }

    void EnemyDead(string str, object value)
    {
        RoleDeadEventData eventData = (RoleDeadEventData)value;
        if (BattleManager.Inst.CurrentPlayer.TemporaryId == (string)eventData.DeadRole.TemporaryId)
        {
            return;
        }

        EnemyType enemyType = DataManager.Inst.EnemyDatas[eventData.DeadRole.UniqueID].EnemyType;
        Vector3 position = eventData.DeadRole.transform.position;

        if (enemyType == EnemyType.Elite || enemyType == EnemyType.Boss)
        {
            // for (int i = 0; i < 3; i++)
            // {
            //     TrySpwanHp(position, EliteEnemyDeadHpDropRate * Luck);
            // }
            //
            // for (int i = 0; i < 10; i++)
            // {
            //     TrySpwanCoin(position, 1 * Luck);
            // }
        }
        else
        {
            for (int i = 0; i < EnemyDeadCoinMaxCount; i++)
            {
                TrySpwanCoin(position, EnemyDeadCoinDropRate * Luck);
            }
            TrySpwanHp(position, EnemyDeadHpDropRate * Luck);
        }
    }

    public void CreateChestDropItem(Vector3 position)
    {
        if (ChestCoinMinCount > ChestCoinMaxCount)
        {
            Debug.LogError("宝箱金钱下限比上限高！");
            return;
        }

        List<GameObject> results = new List<GameObject  >();
        for (int i = 0; i < ChestCoinMaxCount - ChestCoinMinCount; i++)
        {
            TrySpwanCoin(position, ChestCoinDropRate * Luck);
        }
        for (int i = 0; i < ChestCoinMinCount; i++)
        {
            TrySpwanCoin(position, 1, true);
        }
        for (int i = 0; i < ChestDeadHpMaxCount; i++)
        {

            TrySpwanHp(position, ChestHpDropRate * Luck);
        }
    }
        
    void TrySpwanCoin(Vector3 position, float dropRate, bool ignoreLimit = false)
    {
        if (!CheckIfCanSpwanCoin() && ignoreLimit)
            return;

        float rand = Random.value;
        if (rand < dropRate + coinDropRateCorrect)
        {
            Debug.Log("ObjDropRate:"+ dropRate);
            GameObject obj;

            //随机从三种金币中选择一种
            if (rand < dropRate * 0.75f)
            {
                ResourcesManager.Inst.GetAsset<GameObject>(string.Format(AssetPath, PickableCoinSmallName),
                    delegate (GameObject go)
                    {
                        obj = go;
                        enemyCoinCount += 1;
                        InstantiateObj(obj, position);
                    });
            }
            else if (rand < dropRate * 0.95f)
            {
                ResourcesManager.Inst.GetAsset<GameObject>(string.Format(AssetPath, PickableCoinMiddleName),
                    delegate (GameObject go)
                    {
                        obj = go;
                        enemyCoinCount += 2;
                        InstantiateObj(obj, position);
                    });
            }
            else
            {
                ResourcesManager.Inst.GetAsset<GameObject>(string.Format(AssetPath, PickableCoinLargeName),
                    delegate (GameObject go)
                    {
                        obj = go;
                        enemyCoinCount += 3;
                        InstantiateObj(obj, position);
                    });
            }

            // Instantiate(obj, position, Quaternion.identity);
            // coinDropRateCorrect -= 0.05f;
        }
        else
        {
            //coinDropRateCorrect += 0.05f;
        }
    }

    void InstantiateObj(GameObject go, Vector3 position)
    {
        Instantiate(go, position, Quaternion.identity);
        //coinDropRateCorrect -= 0.05f;
    }

    void TrySpwanHp(Vector3 position, float dropRate)
    {
        float rand = Random.value;

        if (rand < dropRate)
        {
            Debug.Log("HpDropRate:"+dropRate);
            ResourcesManager.Inst.GetAsset<GameObject>(string.Format(AssetPath, PickableHpName),
                delegate (GameObject obj)
                {
                    if (obj == null)
                    {
                        Debug.LogError($"没找到：{string.Format(AssetPath, PickableHpName)}");
                        return;
                    }
                    Instantiate(obj, position, Quaternion.identity);
                });
        }
    }

    public bool CheckIfCanSpwanCoin()
    {
        if (enemyCoinCount < MaxFightRoomCoinCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SpwanCoinAt(Vector3 position)
    {
        ResourcesManager.Inst.GetAsset<GameObject>(string.Format(AssetPath, PickableCoinSmallName),
            delegate (GameObject obj)
            {
                // var coin = obj.GetComponent<PickAbleObj_GoldAdd>();
                Instantiate(obj, position, Quaternion.identity);
            });
    }
    public void SpwanDiamondAt(Vector3 position)
    {
        ResourcesManager.Inst.GetAsset<GameObject>(string.Format(AssetPath, PickableDiamondName),
            delegate (GameObject obj)
            {
                // var coin = obj.GetComponent<PickAbleObj_GoldAdd>();
                Instantiate(obj, position, Quaternion.identity);
            });
    }


    //获取物品流派的物体概率
    public Dictionary<int, float> GetItemSchoolItemProbability(List<int> itemPool)
    {
        var itemSchoolIds = GetPlayerItemSchool();
        //物品在流派中出现的次数
        Dictionary<int, int> itemTimes = new Dictionary<int, int>();
        for (int i = 0; i < itemSchoolIds.Count; i++)
        {
            foreach (int itemId in DataManager.Inst.ItemSchool[itemSchoolIds[i]].ItemList)
            {
                if (!itemPool.Contains(itemId))
                {
                    continue;
                }
                if (!itemTimes.ContainsKey(itemId))
                {
                    itemTimes.Add(itemId, 0);
                }
                itemTimes[itemId]++;
            }
        }
        var itemIds = BattleManager.Inst.CurrentPlayer.roleItemController.GetItemsId();
        foreach (int itemId in itemIds)
        {
            if (itemTimes.ContainsKey(itemId))
            {
                itemTimes.Remove(itemId);
            }
        }


        Dictionary<int, float> itemProbability = new Dictionary<int, float>();
        var unlockItems = ArchiveManager.Inst.ArchiveData.TemporaryData.UnlockedItems;
        foreach (int itemId in unlockItems)
        {
            if (!itemPool.Contains(itemId))
            {
                continue;
            }

            if (!itemTimes.ContainsKey(itemId))
            {
                itemTimes.Add(itemId, 0);
            }
            itemTimes[itemId]++;
        }

        List<int> removeList = new List<int>();
        foreach (KeyValuePair<int, int> item in itemTimes)
        {
            if (DataManager.Inst.GetItemScrObj(item.Key).isUnique && itemIds.Contains(item.Key))
            {
                //唯一道具
                removeList.Add(item.Key);
            }
        }

        foreach (int id in removeList)
        {
            itemTimes.Remove(id);
        }

        float itemCount = 0;
        foreach (KeyValuePair<int, int> item in itemTimes)
        {
            itemCount += item.Value;
        }

        foreach (var item in itemTimes)
        {
            float probability = item.Value / itemCount;
            itemProbability.Add(item.Key, probability);
        }

        return itemProbability;
    }

    //获取玩家身上道具能组成的流派
    List<int> GetPlayerItemSchool()
    {
        List<int> itemSchools = new List<int>();
        var itemSchoolIds = GetUnlockItemSchool();
        var itemIds = BattleManager.Inst.CurrentPlayer.roleItemController.GetItemsId();
        for (int i = 0; i < itemIds.Count; i++)
        {
            foreach (int itemSchoolId in itemSchoolIds)
            {
                if (DataManager.Inst.ItemSchool[itemSchoolId].ItemList.Contains(itemIds[i]) && !itemSchools.Contains(itemSchoolId))
                {
                    itemSchools.Add(itemSchoolId);
                }
            }
        }

        return itemSchools;
    }

    //获取已经完全解锁的物品流派
    List<int> GetUnlockItemSchool()
    {
        List<int> itemSchoolIds = new List<int>();
        foreach (var itemSchool in DataManager.Inst.ItemSchool)
        {
            bool schoolUnlock = true;
            for (int i = 0; i < itemSchool.Value.ItemList.Count; i++)
            {
                if (!ArchiveManager.Inst.CheckItemIsUnLock(itemSchool.Value.ItemList[i]))
                {
                    schoolUnlock = false;
                    break;
                }
            }
            if (schoolUnlock)
            {
                itemSchoolIds.Add(itemSchool.Key);
            }
        }
        return itemSchoolIds;
    }



}
