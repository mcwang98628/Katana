using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PausePanel : PanelBase
{
    public GameObject HomeButton;

    public Transform ItemGroup;
    public UI_PausePanel_ItemPrefab ItemPrefab;

    // public Transform FireGroup;
    // public Transform FirePrefab;

    public Toggle Sound;
    public Toggle Bgm;
    
    private void Awake()
    {
        // if (BattleManager.Inst.IsTutorial)
        // {
        //     HomeButton.gameObject.SetActive(false);
        // }

        InitItem();
        // InitFire();
        Sound.isOn = ArchiveManager.Inst.ArchiveData.SettingArchiveData.Sound;
        Bgm.isOn = ArchiveManager.Inst.ArchiveData.SettingArchiveData.Bgm;
    }

    void InitItem()
    {
        foreach (Item item in BattleManager.Inst.CurrentPlayer.roleItemController.Items)
        {
            if (item.ItemType != ItemType.Artifact)
            {
                continue;
            }
            GameObject.Instantiate(ItemPrefab,ItemGroup).Init(item);
        }
    }

    // void InitFire()
    // {
    //     int levelCount;
    //     if (BattleManager.Inst.RuntimeData is ChapterRulesRuntimeData chapterRulesRuntimeData)
    //     {
    //         levelCount = ((ChapterStructData) (chapterRulesRuntimeData.LevelStructData)).RoomList.Count;
    //     }
    //     else
    //     {
    //         return;
    //     }
    //     
    //     var battleData = ArchiveManager.Inst.ArchiveData.BattleData;
    //     int levelIndex = 0;
    //     if (battleData != null)
    //     {
    //         levelIndex = battleData.ChapterData.CurrentLevelIndex;
    //     }
    //
    //     for (int i = 0; i < levelCount; i++)
    //     {
    //         var go = GameObject.Instantiate(FirePrefab, FireGroup);
    //         if (i <= levelIndex)
    //         {
    //             go.GetChild(1).gameObject.SetActive(true);
    //         }
    //         go.gameObject.SetActive(true);
    //     }
    // }

    public void OnClosePanel()
    {
        UIManager.Inst.Close();
    }

    public void OnBackHome()
    {  
        // UIManager.Inst.Open("YesOrNoPanel",true,"BackHomeTitle","BackHomeContent",new Action<bool>(isOk =>
        // {
        //     if (isOk)
        //     {
        //         // ArchiveManager.Inst.SaveBattleData(null);
        //         // UIManager.Inst.ShowMask(()=> {
        //         //     TimeManager.Inst.SetTimeScale(1f);
        //         //     UIManager.Inst.Close();
        //         //     ProcedureManager.Inst.StartProcedure(new MainSceneProcedure());
        //         //     UIManager.Inst.HideMask(null);
        //         // });
        //
        //         // TimeManager.Inst.SetTimeScale(1f);
        //         UIManager.Inst.Close("PausePanel");
        //         GameManager.Inst.StartCoroutine(WaitQuitGame());
        //
        //     }
        // }));
        Application.Quit();
    }

    IEnumerator WaitQuitGame()
    {
        // yield return new WaitForSecondsRealtime(0.1f);
        yield return null;
        if (BattleManager.Inst.BattleRules is SimilarChapterRules rules)
        {
            rules.QuitGame();
        }
    }

    public void OnSoundToggle(bool isON)
    {
        ArchiveManager.Inst.SetSound(isON);
    }

    public void OnMuissToggle(bool isOn)
    {
        ArchiveManager.Inst.SetBgm(isOn);
    }
    
}
