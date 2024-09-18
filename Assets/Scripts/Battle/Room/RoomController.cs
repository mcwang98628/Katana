using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

//房间基类
public class RoomController : MonoBehaviour
{
    protected RoomDoor[] Doors;
    //掉落物品
    public RoomType RoomType => CurrentRoomData.RoomType;
    
    private GameObject currentRoomContent;
    public RoomData CurrentRoomData { get; private set; }

    [SerializeField][LabelText("房间入口")]
    private Transform Entrance;

    public Transform RoomEnter => Entrance;
    
    protected virtual void Awake()
    {
        Doors = GetComponentsInChildren<RoomDoor>();
    }

    public void ShowHideEnterObstacleBox(bool isShow)
    {
        if (Entrance == null)
        {
            Entrance = transform.Find("Entrance_Z");
            if (Entrance == null)
            {
                Transform roomframe = transform.Find("RoomFrame");
                if (roomframe!=null)
                {
                    Entrance = roomframe.Find("Entrance_Z");
                }
            }
        }
        if (Entrance == null)
        {
            return;
        }
        Entrance.gameObject.SetActive(isShow);
    }

    public void SetRoomInfo(RoomData _roomData)
    {
        this.CurrentRoomData = _roomData;
        StartCoroutine(waitAwakeCheck());
    }

    IEnumerator waitAwakeCheck()
    {
        yield return null;
        AwakeCheck();
    }
    void AwakeCheck()
    {
        if (RoomType != RoomType.TreasureRoom 
            && RoomType != RoomType.EventRoom 
            && RoomType != RoomType.ShopRoom)
        {
            return;
        }
        var list = DataManager.Inst.GetConditionInfos(RoomType);
        List<ConditionInfo> ConditionInfos = CheckRoomConditionList(list);
        if (ConditionInfos == null || ConditionInfos.Count == 0)
        {
            Debug.LogError("Err:没有满足条件的Content");
            return;
        }

        if (RoomType == RoomType.EventRoom || RoomType == RoomType.ShopRoom || RoomType == RoomType.TreasureRoom)
        {
            for (int i = 0; i < ConditionInfos.Count; i++)
            {
                if (BattleManager.Inst.RuntimeData.CheckContainsUsingConditionInfoId(ConditionInfos[i].Id) && ConditionInfos[i].IsOnly)
                {
                    ConditionInfos.RemoveAt(i);
                    i--;
                }
            }
            
            if (ConditionInfos.Count == 0)
            {
                Debug.LogError("#Err# 除了已使用的之外 没有可用的Content");
            }
        }
        
        
        int index = Random.Range(0, ConditionInfos.Count);
        ConditionInfo conditionInfo = ConditionInfos[index];
        ConditionInfos.RemoveAt(index);

        if (RoomType == RoomType.EventRoom || RoomType == RoomType.ShopRoom || RoomType == RoomType.TreasureRoom)
            BattleManager.Inst.RuntimeData.AddUsingConditionInfoId(conditionInfo.Id);

        
        CurrentRoomData.SetConditionInfoId(conditionInfo.Id);
        string ContentFileNamePath = "Assets/Arts/RoomPrefabs/Rooms/RoomContent/{0}.prefab";
        
        ResourcesManager.Inst.GetAsset<GameObject>(
            string.Format(ContentFileNamePath, conditionInfo.ContentFileName), delegate(GameObject roomContent)
            {
                currentRoomContent = GameObject.Instantiate(roomContent,transform);
                currentRoomContent.transform.localScale = Vector3.one;
            });
    }
    

    List<ConditionInfo> CheckRoomConditionList(List<ConditionInfo> list)
    {
        List<ConditionInfo> MeetList = new List<ConditionInfo>();
        for (int i = 0; i < list.Count; i++)
        {
            if (CheckRoomCondition(list[i]))
            {
                MeetList.Add(list[i]);
            }
        }

        return MeetList;
    }

    bool CheckRoomCondition(ConditionInfo ci)
    {
        float hpPercentage;
        switch (ci.ConditionType)
        {
            case ConditionType.None:
                return true;
            case ConditionType.GoldGreater:
                int gold = int.Parse(ci.ConditionValue);
                return BattleManager.Inst.CurrentGold >= gold;
            case ConditionType.HpPercentageGreater:
                float greaterHpPercentage = float.Parse(ci.ConditionValue);
                hpPercentage = BattleManager.Inst.CurrentPlayer.CurrentHp / BattleManager.Inst.CurrentPlayer.MaxHp;
                return hpPercentage > greaterHpPercentage;
            case ConditionType.HpPercentageLess:
                float lessHpPercentage = float.Parse(ci.ConditionValue);
                hpPercentage = BattleManager.Inst.CurrentPlayer.CurrentHp / BattleManager.Inst.CurrentPlayer.MaxHp;
                return hpPercentage < lessHpPercentage;
            case ConditionType.LevelIndexGreater:
                if (BattleManager.Inst.RuntimeData is ChapterRulesRuntimeData chapterRulesRuntimeData)
                {
                    int levelIndex = int.Parse(ci.ConditionValue);
                    return chapterRulesRuntimeData.CurrentLevelIndex > levelIndex;
                }
                else
                {
                    return false;
                }
            case ConditionType.HaveItemList:
                var items = ci.ConditionValue.Split('-');
                foreach (string itemId in items)
                {
                    int id = int.Parse(itemId);
                    if (!BattleManager.Inst.CurrentPlayer.roleItemController.IsHaveItem(id))
                    {
                        return false;
                    }
                }
                return true;
        }

        return false;
    }

    public Vector3 GetDoorPos()
    {
        return Doors[0].transform.position;
    }
    
    public void InitDoor(RoomData roomData,bool isLastRoom)
    {
        Doors[0].Init(roomData,isLastRoom);
    }

    public void InitGameOverDoor()
    {
        for (int i = 0; i < Doors.Length; i++)
        {
            Doors[i].InitGameOverDoor();
        }
    }
    
    
    //开门
    protected void OpenTheDoor()
    {
        EventManager.Inst.DistributeEvent(EventName.OpenRoomDoor);
        EventManager.Inst.DistributeEvent(EventName.CollectRoomProps);
        
        for (int i = 0; i < Doors.Length; i++)
        {
            OpenDoor(Doors[i]);
        }

        
    }

    
    void OpenDoor(RoomDoor door)
    {
        if (!door.IsInit)
        {
            return;
        }
        door.OpenDoor();
    }
    protected IEnumerator OpenDoor(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        OpenTheDoor();
    }

    protected virtual void OnDestroy() { // BattleManager.Inst.DestroyAllEnemyObj();
    }
    
    

}


