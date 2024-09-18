using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainPanel_ChapterPanel : MonoBehaviour
{
    [SerializeField]
    private UI_MainPanel mainPanel;
    [SerializeField]
    private UI_MainPanel_ChapterPanel_ChapterPrefab chapterPrefab;
    [SerializeField]
    private Transform chapterGroup;
    [SerializeField]
    private GameObject delectArchiveBtn;

    [SerializeField]
    private UI_MainPanel_MissionPanel missionPanel;
    
    private List<UI_MainPanel_ChapterPanel_ChapterPrefab> _prefabs = new List<UI_MainPanel_ChapterPanel_ChapterPrefab>();
    private UI_MainPanel_ChapterPanel_ChapterPrefab _currentSelectChapter;
    [SerializeField]
    public UI_MainPanel_ChapterInfoPanel infoPanel;
    public int LastChapterId { get; private set; }
    
    private void Start()
    {
        Init();
    }

    // private void OnEnable()
    // {
    //     mainPanel.SetCameraRender(true);
    // }
    //
    // private void OnDisable()
    // {
    //     mainPanel.SetCameraRender(false);
    // }

    public void Init()
    {
        LastChapterId = ArchiveManager.Inst.ArchiveData.GlobalData.LastSelectChapterId;
        bool isHaveBattleArchive = ArchiveManager.Inst.ArchiveData.BattleData != null;
        // delectArchiveBtn.SetActive(isHaveBattleArchive);
        foreach (var chapterTableData in DataManager.Inst.ChapterTableDatas)
        {
            if (chapterTableData.Value.Id <= 0)
            {
                continue;
            }
            
            var go = GameObject.Instantiate(chapterPrefab, chapterGroup);
            go.Init(chapterTableData.Value);
            _prefabs.Add(go);
            
            if (!isHaveBattleArchive && LastChapterId == chapterTableData.Value.Id)
            {
                go.GetComponent<Toggle>().isOn = true;
            }
            else if (isHaveBattleArchive && chapterTableData.Value.Id == ArchiveManager.Inst.ArchiveData.BattleData.ChapterData.CurrentCPId)
            {
                go.GetComponent<Toggle>().isOn = true;
            }
        }

    }
    
    public void StartBattle()
    {
        var battleArchiveData = ArchiveManager.Inst.ArchiveData.BattleData;
        if (battleArchiveData != null && battleArchiveData.ChapterData.CurrentCPId == _currentSelectChapter.TabData.Id)
        {
            BattleManager.Inst.LoadArchiveBattle(battleArchiveData);
            EventManager.Inst.AddEvent(EventName.OnChapterRulesLoadBattleOver,OnLoadBattleOver);
        } 
        else if(battleArchiveData != null)
        {   
            UIManager.Inst.Open("YesOrNoPanel",false,"¥清除存档¥","是否要清除存档,进入其他章节",new Action<bool>(isOk =>
            {
                if (isOk)
                {
                    ArchiveManager.Inst.SaveBattleData(null);
                    BattleManager.Inst.LoadChapterBattle(_currentSelectChapter.TabData.Id);
                    EventManager.Inst.AddEvent(EventName.OnChapterRulesLoadBattleOver,OnLoadBattleOver);
                }
            }));
        }
        else
        {
            BattleManager.Inst.LoadChapterBattle(_currentSelectChapter.TabData.Id);
            EventManager.Inst.AddEvent(EventName.OnChapterRulesLoadBattleOver,OnLoadBattleOver);
        }
        
    }

    private void OnLoadBattleOver(string arg1, object arg2)
    {
        EventManager.Inst.RemoveEvent(EventName.OnChapterRulesLoadBattleOver,OnLoadBattleOver);
        BattleManager.Inst.StartGame();
    }

    public void SelectChapter(UI_MainPanel_ChapterPanel_ChapterPrefab chapterPrefab)
    {
        _currentSelectChapter = chapterPrefab;
    }
    
    /// <summary>
    /// 删除战斗中存档
    /// </summary>
    public void DeleteBattleArchive()
    {  
        UIManager.Inst.Open("YesOrNoPanel",false,"¥清除存档¥","是否要清除存档？",new Action<bool>(isOk =>
        {
            if (isOk)
            {
                ArchiveManager.Inst.SaveBattleData(null);
                UpdatePrefab();
                delectArchiveBtn.SetActive(ArchiveManager.Inst.ArchiveData.BattleData != null);
            }            
        }));
    }

    private void UpdatePrefab()
    {
        foreach (var prefab in _prefabs)
        {
            prefab.UpdateUI();
        }
    }
    
    
    public void OnMissionBtnClick()
    {
        missionPanel.Init();
    }

    public void OnBackBtnClick()
    {
        this.gameObject.SetActive(false);
    }
}
