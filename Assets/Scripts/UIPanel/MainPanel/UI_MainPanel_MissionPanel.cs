using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainPanel_MissionPanel : MonoBehaviour
{
    [SerializeField]
    private Transform missionPrefabGroup;
    [SerializeField]
    private UI_MainPanel_MissionPanel_MissionPrefab missionPrefab;
    [SerializeField]
    private Toggle dailyToggle;

    private List<UI_MainPanel_MissionPanel_MissionPrefab> _prefabs = new List<UI_MainPanel_MissionPanel_MissionPrefab>();
    private Dictionary<int, MissionTabData> MissionTabDatas => DataManager.Inst.MissionTabDatas;

    private MissionType _missionType;
    
    public void Init()
    {
        dailyToggle.isOn = true;
        _missionType = MissionType.Daily;
        InitMissionPrefabs();
        gameObject.SetActive(true);
    }

    public void OnDailyToggle(bool isOn)
    {
        if (isOn)
        {
            _missionType = MissionType.Daily;
            InitMissionPrefabs();
        }
    }

    public void OnAchieveToggle(bool isOn)
    {
        if (isOn)
        {
            _missionType = MissionType.Achieve;
            InitMissionPrefabs();
        }
    }

    private void InitMissionPrefabs()
    {
        for (int i = _prefabs.Count-1; i >= 0; i--)
        {
            GameObject.Destroy(_prefabs[i].gameObject);
        }
        _prefabs.Clear();
        
        foreach (var missionTabData in MissionTabDatas)
        {
            if (missionTabData.Value.MissionType != _missionType)
            {
                continue;
            }

            var go = GameObject.Instantiate(missionPrefab, missionPrefabGroup);
            go.Init(missionTabData.Value);
            _prefabs.Add(go);
        }
    }

    public void OnCloseBtnClick()
    {
        gameObject.SetActive(false);
    }
}
