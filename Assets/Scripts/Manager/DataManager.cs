using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FlexFramework.Excel;
using UnityEngine;

public partial class DataManager : TSingleton<DataManager>
{
    private bool _isInitOk = false;
    public bool IsInit => initNumber == 0;
    private int initNumber;
    public void Init()
    {
        if (_isInitOk)
        {
            return;
        }

        initNumber++;
        InitChapterData();
        InitEventData();
        InitEnemyInfo();
        LoadItemPool();
        InitItemInfo();
        // LoadChapterItemUnLockTab();
        InitBuffInfo();
        InitRoomContentData();
        InitRoomTipsInfoData();
        InitDifficultyCSV();
        InitHeroData();
        InitHeroUpgradeData();
        InitHeroUpgradeColorData();
        InitExpLevelTable();
        InitRuneSlotData(); 
        InitMissionData();
        LoadEquipment();
        LoadAttributesData();
        initNumber--;
        
        
        _isInitOk = true;
        
    }

}



