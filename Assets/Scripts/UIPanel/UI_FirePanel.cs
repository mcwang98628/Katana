using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FirePanel : PanelBase
{
    [SerializeField]
    private Transform group;
    // [SerializeField]
    // private UI_NewMainPanel_ChapterPanel_CheckPoint _checkPointPrefab;

    private Action _goDieCallBack;
    public void OnOpen(Action goDieCallBack)
    {
        // var battleData = ArchiveManager.Inst.ArchiveData.BattleData;
        // int level = -1;
        // if (battleData!=null)
        // {
        //     level = battleData.ChapterData.CurrentLevelIndex;
        // }
        // for (int i = 0; i < 5; i++)
        // {
        //     GameObject.Instantiate(_checkPointPrefab, group).ShowFire(level == i);
        // }

        _goDieCallBack = goDieCallBack;
    }

    public void OnHomeBtnClick()
    {
        UIManager.Inst.ShowMask(()=> {
            BattleManager.Inst.EndBattle();
            TimeManager.Inst.SetTimeScale(1f);
            UIManager.Inst.Close("FirePanel");
            ProcedureManager.Inst.StartProcedure(new MainSceneProcedure());
            UIManager.Inst.HideMask(null);
        });
    }

    public void OnReStartBtnClick()
    {
        if (ArchiveManager.Inst.ArchiveData.BattleData == null)
        {
            UIManager.Inst.Tips.ShowText("没有存档，你还是狗带吧。");
            return;
        }

        if (ArchiveManager.Inst.ArchiveData.GlobalData.Fire <= 0)
        {
            UIManager.Inst.Tips.ShowText("没有余火，你还是狗带吧。");
            return;
        }
        UIManager.Inst.ShowMask(()=> {
            // BattleManager.Inst.EndBattle();
            // TimeManager.Inst.SetTimeScale(1f);
            // UIManager.Inst.Close("FirePanel");
            // BattleManager.Inst.LoadArchiveBattle
            // (
            //     ArchiveManager.Inst.ArchiveData.BattleData
            // );
            // BattleManager.Inst.StartGame();
            // UIManager.Inst.HideMask(null);
            GameManager.Inst.StartCoroutine(waitRestart());
        });
    }

    IEnumerator waitRestart()
    {
        BattleManager.Inst.EndBattle();
        TimeManager.Inst.SetTimeScale(1f);
        UIManager.Inst.Close("FirePanel");
        BattleManager.Inst.LoadArchiveBattle
        (
            ArchiveManager.Inst.ArchiveData.BattleData
        );
        yield return null;
        yield return null;
        BattleManager.Inst.StartGame();
        UIManager.Inst.HideMask(null);
    }

    public void OnGoDieBtnClick()
    {
        ArchiveManager.Inst.SaveBattleData(null);
        UIManager.Inst.Close("FirePanel");
        _goDieCallBack?.Invoke();
    }
    
}
