using System.Collections.Generic;

public partial class SDKManager
{
    void AddBattleEvent()
    {
        EventManager.Inst.AddEvent(EventName.OnChapterBattleStart,OnBattleStart);
        EventManager.Inst.AddEvent(EventName.OnBattleOver,OnBattleOver);
        EventManager.Inst.AddEvent(EventName.EnterNextRoom,OnEnterNextRoom);
        EventManager.Inst.AddEvent(EventName.OnRoleInjured,OnPlayerInjured);
        EventManager.Inst.AddEvent(EventName.OnRoleDodgeInjured,OnRoleDodgeInjured);
        EventManager.Inst.AddEvent(TGANames.BattleTreasureChestItem,OnTreasureChestItem);
        EventManager.Inst.AddEvent(TGANames.BattleTreasureChestSkip,OnTreasureChestSkip);
        EventManager.Inst.AddEvent(TGANames.BattleShopBuyItem,ShopBuyItem);
    }
    void RemoveBattleEvent()
    {
        
    }

    private Dictionary<string, object> BattleCommonData()
    {
        var dic = GetCommonData();
        
        int heroId = BattleManager.Inst.CurrentPlayer.UniqueID;
        HeroUpgradeInfo info = ArchiveManager.Inst.ArchiveData.GlobalData.HeroUpgradeDatas[heroId];
        int heroLevel = info.Level + info.ColorLevel*4;
        ChapterRulesRuntimeData runtimeData = (ChapterRulesRuntimeData) BattleManager.Inst.RuntimeData;
        dic.Add(TGANames.ChapterID,runtimeData.CurrentChapterId);
        dic.Add(TGANames.LevelID,runtimeData.CurrentLevelIndex);
        dic.Add(TGANames.RoomID,runtimeData.CurrentRoomIndex);
        
        dic.Add(TGANames.IsArchiveContinueBattle,BattleManager.Inst.IsArchiveBattle);
        dic.Add(TGANames.HeroId,heroId);
        dic.Add(TGANames.HeroLevel,heroLevel);
        dic.Add(TGANames.BattleGuid,BattleManager.Inst.BattleGuid);
        
        dic.Add(TGANames.IsChapterMode,
            BattleManager.Inst.RuntimeData.LevelStructData.LevelStructType == LevelStructType.Chapter);
        dic.Add(TGANames.Gold,BattleManager.Inst.CurrentGold);
        dic.Add(TGANames.BattleTime,BattleManager.Inst.BattleTime);
        dic.Add(TGANames.CurrentHp,BattleManager.Inst.CurrentPlayer.CurrentHp);

        if (BattleManager.Inst.RuntimeData is ChapterRulesRuntimeData chapterRuntimeData)
        {
            List<string> roomTypes = new List<string>();
            foreach (RoomType roomType in chapterRuntimeData.PassingRooms)
            {
                roomTypes.Add(roomType.ToString());
            }
            dic.Add(TGANames.PassingRoomTypes,roomTypes);
            
            dic.Add(TGANames.KillEnemyNumber,chapterRuntimeData.KillEnemyNumber);
            dic.Add(TGANames.KillEliteEnemyNumber,chapterRuntimeData.KillSEnemyNumber);
        }
        
        List<int> items = new List<int>();
        foreach (Item item in BattleManager.Inst.CurrentPlayer.roleItemController.Items)
        {
            items.Add(item.ID);
        }
        dic.Add(TGANames.Items,items);

        #region 装备

        List<int> equipIdList = new List<int>();
        List<int> equipScoreList = new List<int>();
        List<EquipmentQuality> equipQualityList = new List<EquipmentQuality>();
        var equip = ArchiveManager.Inst.ArchiveData.equipmentArchiveData150.Slot1;
        if (equip != null)
        {
            equipIdList.Add(equip.Id);
            equipScoreList.Add(equip.Score);
            equipQualityList.Add(equip.Quality);
        }
        equip = ArchiveManager.Inst.ArchiveData.equipmentArchiveData150.Slot2;
        if (equip != null)
        {
            equipIdList.Add(equip.Id);
            equipScoreList.Add(equip.Score);
            equipQualityList.Add(equip.Quality);
        }
        equip = ArchiveManager.Inst.ArchiveData.equipmentArchiveData150.Slot3;
        if (equip != null)
        {
            equipIdList.Add(equip.Id);
            equipScoreList.Add(equip.Score);
            equipQualityList.Add(equip.Quality);
        }
        
        dic.Add("EquipIdList",equipIdList);
        dic.Add("EquipScoreList",equipScoreList);
        dic.Add("EquipmentQualityList",equipQualityList);
        

        #endregion
        
        
        return dic;
    }
    
    /// <summary>
    /// 游戏开始
    /// </summary>
    /// <param name="heroId"></param>
    /// <param name="heroLevel"></param>
    private void OnBattleStart(string eventName,object arg)
    {
        var data = BattleCommonData();
        TGAnalytics.Track(TGANames.GameStart, data);
    }

    private void OnBattleOver(string arg1, object arg2)
    {
        GameOverData overData = (GameOverData) arg2;
        var data = BattleCommonData();
        data.Add(TGANames.IsVictory,overData.isVictory);
        data.Add(TGANames.BattleProgress,overData.Progress);
        TGAnalytics.Track(TGANames.GameOver, data);
    }

    private void OnEnterNextRoom(string arg1, object arg2)
    {
        var data = BattleCommonData();
        TGAnalytics.Track(TGANames.EnterRoom, data);
    }

    private void OnPlayerInjured(string arg1, object arg2)
    {
        var data = BattleCommonData();
        RoleInjuredInfo info = (RoleInjuredInfo) arg2;
        if (BattleManager.Inst.CurrentPlayer.TemporaryId != info.RoleId)
            return;

        data.Add(TGANames.EnemyId,info.Dmg.AttackerID);
        
        TGAnalytics.Track(TGANames.PlayerInjured,data);
    }
    
    private void OnRoleDodgeInjured(string arg1, object arg2)
    {
        var data = BattleCommonData();
        RoleInjuredInfo info = (RoleInjuredInfo) arg2;
        if (BattleManager.Inst.CurrentPlayer.TemporaryId != info.RoleId)
            return;
        
        data.Add(TGANames.EnemyId,info.Dmg.AttackerID);
        TGAnalytics.Track(TGANames.PlayerDodgeInjured,data);
    }

    private void OnTreasureChestItem(string arg1,object arg2)
    {
        int itemId = (int)arg2;
        var data = BattleCommonData();
        data.Add(TGANames.ItemId,itemId);
        TGAnalytics.Track(TGANames.BattleTreasureChestItem,data);
    }
    
    private void OnTreasureChestSkip(string arg1,object arg2)
    {
        var data = BattleCommonData();
        TGAnalytics.Track(TGANames.BattleTreasureChestSkip,data);
    }
    
    
    /// <summary>
    /// 局内商店购买信息
    /// </summary>
    private void ShopBuyItem(string arg1,object arg2)
    {
        int itemId = (int)arg2;
        var data = BattleCommonData();
        data.Add(TGANames.ItemId,itemId);
        TGAnalytics.Track(TGANames.BattleShopBuyItem,data);
    }
    
    
    
    
}