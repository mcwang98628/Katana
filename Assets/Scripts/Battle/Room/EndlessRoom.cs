using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EndlessRoom : RoomController
{
    [SerializeField]
    private Transform playerStartPoint;
    [SerializeField]
    private Transform enemyPoints;
    [SerializeField]
    private Transform roomContentPoints;
    [SerializeField]
    private GameObject eventRoomDoor;
    
    //todo 
    private List<EndlessStructWaveData> _waveDatas;
    //当前第几波
    private int _waveIndex;
    private EndlessStructWaveData _currentWaveData => _waveDatas[_waveIndex];
    private bool _notEnd => _waveDatas.Count > _waveIndex;

    private GameObject _currentRoomContent;
    private bool _isGameOver;
    
    public void SetWaveData(List<EndlessStructWaveData> datas)
    {
        _waveDatas = datas;
        _waveIndex = 0;
        _isGameOver = false;
    }

    public void StartBattle()
    {
        WaitNextWave();
    }
    
    

    #region RoomTriggerEvent

    private bool _inBattleRoom;
    private bool _inContentRoom;
    public void OnEnterContentRoom(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return;
        }
        _inContentRoom = true;
    }

    public void OnExitContentRoom(Collider other)
    {
        // if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
        // {
        //     return;
        // }
        // _inContentRoom = false;
    }

    public void OnEnterBattleRoom(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return;
        }
        _inBattleRoom = true;

        if (_waveIndex-1 >0 && _waveDatas[_waveIndex-1].WaveType == EndlessWaveType.Event && _inContentRoom)
        {
            _inContentRoom = false;
            eventRoomDoor.SetActive(true);
            WaitNextWave();
        }
    }

    public void OnExitBattleRoom(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return;
        }
        _inBattleRoom = false;
    }
    

    #endregion
    
    #region 事件

    protected override void Awake()
    {
        base.Awake();
        AddEvent();
        SetPlayerPosition();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        RemoveEvent();
    }

    public void SetPlayerPosition()
    {
        if (BattleManager.Inst.CurrentPlayer!=null)
        {
            BattleManager.Inst.CurrentPlayer.transform.position = playerStartPoint.position;
        }
    }
    
    void AddEvent()
    {
        EventManager.Inst.AddEvent(EventName.OnRoleDead,OnRoleDead);
    }

    void RemoveEvent()
    {
        EventManager.Inst.RemoveEvent(EventName.OnRoleDead,OnRoleDead);
    }

    private void OnRoleDead(string arg1, object arg2)
    {
        RoleDeadEventData deadEventData = (RoleDeadEventData) arg2;
        if (deadEventData.DeadRole.roleTeamType != RoleTeamType.Player)
        {
            if (BattleTool.AllEnemyDie() && !_createEnemying)
            {
                WaitNextWave();
            }
        }
    }
    

    #endregion
    
    #region NextWave

    void WaitNextWave()
    {
        StartCoroutine(waitNextWave());
    }

    IEnumerator waitNextWave()
    {
        yield return new WaitForSeconds(2f);
        NextWave();
    }
    void NextWave()
    {
        if (_notEnd)
        {
            if (_currentRoomContent != null)
            {
                Destroy(_currentRoomContent.gameObject);
            }
            switch (_currentWaveData.WaveType)
            {
                case EndlessWaveType.Event:
                    string ContentFileNamePath = "Assets/Arts/RoomPrefabs/Rooms/RoomContent/{0}.prefab";
                    ConditionInfo conditionInfo = DataManager.Inst.GetConditionInfo(_currentWaveData.RoomContentId);
                    ResourcesManager.Inst.GetAsset<GameObject>(string.Format(ContentFileNamePath, conditionInfo.ContentFileName),
                        delegate(GameObject room)
                        {
                            _currentRoomContent = GameObject.Instantiate(room,transform);
                            _currentRoomContent.transform.position = roomContentPoints.position;
                            eventRoomDoor.SetActive(false); 
                        });
                    break;
                case EndlessWaveType.Enemy:
                    CreateEnemy(_currentWaveData.EnemyList);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //UI提示
            EventManager.Inst.DistributeEvent(EventName.EndlessWaveTips,_currentWaveData.WaveType);
        }
        else
        {
            //GameOver;
            _isGameOver = true;
            EventManager.Inst.DistributeEvent(EventName.EndlessGameOver,_waveIndex);
        }

        _waveIndex++;
    }

    private bool _createEnemying;
    void CreateEnemy(List<int> enemyIds)
    {
        _createEnemying = true;
        for (int i = 0; i < enemyIds.Count; i++)
        {
            BattleTool.CreateEnemy(enemyIds[i], delegate(EnemyController enemyGo)
            {
                enemyGo.transform.position = GetEnemyPoint();
            });
        }

        _createEnemying = false;
    }
    
        #endregion
    
    #region EnemyPoints

    List<int> childCount = new List<int>();
    void InitEnemyPoint()
    {
        childCount.Clear();
        for (int i = 0; i < enemyPoints.childCount; i++)
        {
            childCount.Add(i);
        }
    }
    Vector3 GetEnemyPoint()
    {
        if (childCount.Count<=0)
        {
            InitEnemyPoint();
        }
        int index = childCount[Random.Range(0, childCount.Count)];
        childCount.Remove(index);
        return enemyPoints.GetChild(index).transform.position;
    }
    
    

    #endregion
    
}
