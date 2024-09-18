using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainPanel_ChapterPanel_ChapterPrefab : MonoBehaviour
{

    [SerializeField]
    private UI_MainPanel_ChapterPanel chapterPanel;
    [SerializeField]
    private Image titleBg;
    [SerializeField]
    private Text chapterNameText;
    [SerializeField]
    private Text progressText;
    [SerializeField]
    private Image chapterImage;
    [SerializeField]
    private GameObject selectBg;
    [SerializeField]
    private Button descBtn;
    [SerializeField]
    private Image lockIcon;

    [SerializeField]
    private GameObject redPoint;
    
    [BoxGroup("Fire")] [SerializeField]
    private GameObject fireObject;
    [BoxGroup("Fire")] [SerializeField]
    private Transform fireGroup;
    [BoxGroup("Fire")] [SerializeField]
    private GameObject firePrefab;

    public ChapterTabData TabData => _data;
    private ChapterTabData _data;
    
    private string chapterIconPath = "Assets/Arts/Textures/UISprites/ChapterCovers/{0}.png";
    public void Init(ChapterTabData data)
    {
        _data = data;
        UpdateUI();
        gameObject.SetActive(true);
    }

    public void UpdateUI()
    {
        titleBg.color = _data.TitleBg;
        chapterNameText.text = LocalizationManger.Inst.GetText($"Chapter_{_data.Id}") + " "+ LocalizationManger.Inst.GetText(_data.ChapterName);
        var clearanceData = ArchiveManager.Inst.ArchiveData.StatisticsData.ChapterClearanceDatas;
        int chapterSchedule = 0;
        if (clearanceData.ContainsKey(_data.Id))
        {
            chapterSchedule = clearanceData[_data.Id].Schedule;
        }

        progressText.text = $"{LocalizationManger.Inst.GetText("Progress")}: {chapterSchedule}%";
        ResourcesManager.Inst.GetAsset<Sprite>(string.Format(chapterIconPath,_data.IconName),
            delegate(Sprite sprite)
            {
                chapterImage.sprite = sprite;
            }); 

        var battleArchiveData = ArchiveManager.Inst.ArchiveData.BattleData;
        if (battleArchiveData == null)
        {
            fireObject.SetActive(false);
        }
        else if (battleArchiveData.ChapterData.CurrentCPId == _data.Id)
        {
            fireObject.SetActive(true);
            int levelcount = DataManager.Inst.ChapterTableDatas[_data.Id].LevelCount;
            for (int i = 0; i < levelcount; i++)
            {
                var fireGo = GameObject.Instantiate(firePrefab, fireGroup);
                if (battleArchiveData.ChapterData.CurrentLevelIndex >= i)
                {
                    fireGo.transform.GetChild(0).gameObject.SetActive(false);
                    fireGo.transform.GetChild(1).gameObject.SetActive(true);
                }
                fireGo.SetActive(true);
            }
        }

        bool lostCpId = ArchiveManager.Inst.GetLastNewChapterId() >= _data.Id;
        bool receivedChapterFreeSoul = ArchiveManager.Inst.ArchiveData.GlobalData.ReceiveChapterFreeSoul.Contains(_data.Id);

        redPoint.SetActive(lostCpId && !receivedChapterFreeSoul);
        
        if (!lostCpId)
        {
            GetComponent<Toggle>().interactable = false;
            descBtn.interactable = false;
        }
        else
        {
            lockIcon.gameObject.SetActive(false);
        }
        
    }
    
    public void OnDescBtnClick()
    {
        chapterPanel.infoPanel.Init(TabData.Id);
    }

    public void OnUnlockItemInfoBtnClick()
    {
        UIManager.Inst.Open("InfoPanel",false,new ItemSchoolData(){Desc = "",ItemList = _data.UnlockItemIds,Name = "通关章节自动解锁"});
        // UIManager.Inst.Open("UnLockItemPanel",false,_data.UnlockItemIds,new Action(()=>{}));
        EventManager.Inst.DistributeEvent(TGANames.MainPanelPeekChapterItem,TabData.Id);
    }

    public void OnToggle(bool isOn)
    {
        selectBg.SetActive(isOn);
        if (isOn)
        {
            chapterPanel.SelectChapter(this);
        }

        if (BattleManager.Inst.RuntimeData is ChapterRulesRuntimeData chapterRulesRuntimeData)
        {
            if (chapterRulesRuntimeData.CurrentChapterId != TabData.Id)
            {
                UIManager.Inst.ShowMask(() =>
                {
                    BattleManager.Inst.LoadChapterBattle(TabData.Id);
                    chapterPanel.OnBackBtnClick();
                    UIManager.Inst.HideMask(null);
                });
            }
        }
    }

    private float timer = 0.5f;

    private void Update()
    {
        timer += Time.unscaledDeltaTime;
        if (timer>=0.5f)
        {
            UpdateRedPoint();
            timer -= 0.5f;
        }
    }

    void UpdateRedPoint()
    {
        bool lostCpId = ArchiveManager.Inst.GetLastNewChapterId() >= _data.Id;
        bool receivedChapterFreeSoul = ArchiveManager.Inst.ArchiveData.GlobalData.ReceiveChapterFreeSoul.Contains(_data.Id);

        redPoint.SetActive(lostCpId && !receivedChapterFreeSoul);
    }
    
}
