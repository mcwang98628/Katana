using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainPanel_MissionPanel_MissionPrefab : MonoBehaviour
{
    [SerializeField]
    private Image rewardIcon;
    [SerializeField]
    private UIText missionName;
    [SerializeField]
    private Slider ScheduleSlider;
    [SerializeField]
    private Text scheduletext;
    [SerializeField]
    private GameObject receivedIcon;//已领取 ✅图标
    [SerializeField]
    private GameObject getBtn;//领取按钮

    public MissionTabData MissionTabData { get; private set; }
    public void Init(MissionTabData tabData)
    {
        MissionTabData = tabData;

        UpdateUI();
        gameObject.SetActive(true);
    }

    public void UpdateUI()
    {
        // string rewardIconPath;
        // switch (MissionTabData.RewardType)
        // {
        //     case MissionRewardType.Diamond:
        //         rewardIconPath = "Diamond.png";
        //         break;
        //     case MissionRewardType.Soul:
        //         rewardIconPath = "Soul.png";
        //         break;
        //     default:
        //         throw new ArgumentOutOfRangeException();
        // }
        // rewardIcon.sprite = ResourcesManager.Inst.GetAsset<Sprite>(rewardIconPath);
        
        missionName.text = MissionTabData.Name;

        int value = 0;
        switch (MissionTabData.TargetType)
        {
            case MissionTargetType.KillTargetEnemy:
                var killEnemys = ArchiveManager.Inst.ArchiveData.StatisticsData.KillEnemys;
                if (killEnemys.ContainsKey(MissionTabData.TargetValue))
                {
                    value = killEnemys[MissionTabData.TargetValue];
                }
                else
                {
                    value = 0;
                }
                break;
            case MissionTargetType.KillAllEnemy:
                value = ArchiveManager.Inst.ArchiveData.StatisticsData.AllKillEnemyCount;
                break;
        }

        if (value > MissionTabData.TargetNumber)
        {
            value = MissionTabData.TargetNumber;
        }
        float schedule = (float)value / (float)MissionTabData.TargetNumber;
        
        scheduletext.text = $"{value}/{MissionTabData.TargetNumber}";
        ScheduleSlider.value = schedule;

        List<int> completedMissions = ArchiveManager.Inst.ArchiveData.GlobalData.MissionCompletedIdList;

        if (completedMissions.Contains(MissionTabData.Id))
        {
            receivedIcon.SetActive(true);
            getBtn.SetActive(false);
        }
        else
        {
            receivedIcon.SetActive(false);
            getBtn.SetActive(schedule >= 1);
        }
    }

    public void OnGetBtnClick()
    {
        ArchiveManager.Inst.MissionCompleted(MissionTabData.Id);

        UpdateUI();
    }
    
}
