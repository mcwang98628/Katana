using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneProcedure : IProcedure
{

    public void Start()
    {
        ResourcesManager.Inst.GetAsset<AudioClip>("Assets/AssetsPackage/Music/Main_title_temp.mp3", delegate(AudioClip clip)
        {
            AudioManager.Inst.PlayBGM(clip,0.7f);
        });
        UIManager.Inst.ShowMask(() =>
        {
            if (ArchiveManager.Inst.ArchiveData.StatisticsData.ChapterClearanceDatas.ContainsKey(0) &&
                ArchiveManager.Inst.ArchiveData.StatisticsData.ChapterClearanceDatas[0].Count>0)
            {
                GameManager.Inst.StartCoroutine(StartMainPanel());
                Debug.Log("#流程# MainScene流程");
            }
            else
            {
                UIManager.Inst.ShowMask(() =>
                {
                    GameManager.Inst.StartCoroutine(StartTutorialsBattle());
                });
            }
            UIManager.Inst.HideMask(null);
        });

    }

    IEnumerator StartMainPanel()
    {
        bool maskIsShow = false;
        UIManager.Inst.ShowMask(() =>
        {
            maskIsShow = true;
        });
        while (!maskIsShow)
        {
            yield return null;
        }
        ScenesManager.Inst.LoadScene("NullScene");
        yield return null;
        
        yield return null;
        UIManager.Inst.CloseAll(); 
        UIManager.Inst.Open("MainPanel");
        
        BattleManager.Inst.LoadChapterBattle(ArchiveManager.Inst.ArchiveData.GlobalData.LastSelectChapterId);
        
        while (!BattleManager.Inst.BattleRules.IsLoadOver)
        {
            yield return null;
        }
        yield return null;
        yield return null;
        UIManager.Inst.HideMask(delegate { 
            EventManager.Inst.DistributeEvent(EventName.OnAppStart);
            
        });
    }

    IEnumerator StartTutorialsBattle()
    {
        UIManager.Inst.CloseAll();
        yield return null;
        BattleManager.Inst.LoadChapterBattle(0);
        yield return new WaitForSecondsRealtime(0.3f);

        while (!BattleManager.Inst.BattleRules.IsLoadOver)
        {
            yield return null;
        }
        
        UIManager.Inst.Open("BattlePanel");
        BattleManager.Inst.StartGame();
        BattleManager.Inst.DoCamera();
        yield return null;
        yield return null;
        UIManager.Inst.HideMask(delegate
        {
            EventManager.Inst.DistributeEvent(EventName.OnAppStart);
        });
    }

    public void End()
    {
        UIManager.Inst.Close("MainPanel");
    }

    public void Update()
    {
        
    }
}
