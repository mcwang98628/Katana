using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainPanel_BattlePanel : MonoBehaviour
{
    [SerializeField]
    private UI_MainPanel_MissionPanel missionPanel;
    [SerializeField]
    private UI_MainPanel_ChapterPanel chapterPanel;
    [SerializeField]
    private UIText chapterText;    
    [SerializeField]
    private UIText chapterName;    
    [SerializeField]
    private Text progressText;
    [SerializeField]
    private UI_MainPanel_Tips _tips;
    // [SerializeField]
    // private GameObject redPoint;
    
    private void Awake()
    {
        EventManager.Inst.AddEvent(EventName.LoadChapter,OnLoadChapter);
        Init(DataManager.Inst.GetCpData(ArchiveManager.Inst.ArchiveData.GlobalData.LastSelectChapterId));
    }

    // private void OnEnable()
    // {
    //     UpdateRedPoint();
    // }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.LoadChapter,OnLoadChapter);
    }
    
    private void OnLoadChapter(string arg1, object arg2)
    {
        int cpId = (int) arg2;
        Init(DataManager.Inst.GetCpData(cpId));
    }
    
    public void Init(ChapterTabData data)
    {
        chapterText.text = "Chapter_" + data.Id;
        chapterName.text = data.ChapterName;
        
        int currentLevel = 0;
        if (ArchiveManager.Inst.ArchiveData.StatisticsData.ChapterClearanceDatas.ContainsKey(data.Id))
        {
            var cpData = ArchiveManager.Inst.ArchiveData.StatisticsData.ChapterClearanceDatas[data.Id];
            currentLevel = ArchiveManager.Inst.ArchiveData.StatisticsData.ChapterClearanceDatas[data.Id].LastLevel;
            if (currentLevel >= data.LevelCount || cpData.Count > 0)
                currentLevel = data.LevelCount;
            
        }
        

        progressText.text = $"{LocalizationManger.Inst.GetText("Progress")}：{currentLevel}/{data.LevelCount}";
    }
    
    public void OnStartBtnClick()
    {
        if (ArchiveManager.Inst.ArchiveData.GlobalData.Fire < 1)
        {
            _tips.Show(UI_Asset.AssetType.Fire);
            return;
        }
        BattleManager.Inst.StartGame();
    }
    
    public void OnMissionBtnClick()
    {
        missionPanel.Init();
    }
    
    public void OnChapterBtnClick()
    {
        chapterPanel.gameObject.SetActive(true);
        
        EventManager.Inst.DistributeEvent(TGANames.MainPanelOpenChapterPanel);
    }

    // private float timer = 0.5f;
    // private void Update()
    // {
    //     timer += Time.unscaledDeltaTime;
    //     if (timer >= 0.5f)
    //     {
    //         UpdateRedPoint();
    //         timer -= 0.5f;
    //     }
    // }

    // void UpdateRedPoint()
    // {
    //     int currentCpid = ArchiveManager.Inst.GetLastNewChapterId();
    //     foreach (KeyValuePair<int,ChapterTabData> chapterTabData in DataManager.Inst.ChapterTableDatas)
    //     {
    //         if (chapterTabData.Key <= 0)
    //             continue;
    //         
    //         bool receivedChapterFreeSoul = ArchiveManager.Inst.ArchiveData.GlobalData.ReceiveChapterFreeSoul.Contains(chapterTabData.Key);
    //         if (!receivedChapterFreeSoul && currentCpid > chapterTabData.Key)
    //         {
    //             redPoint.SetActive(true);
    //             return;
    //         }
    //     }
    //     redPoint.SetActive(false);
    // }
}
