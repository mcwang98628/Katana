public partial class SDKManager
{
    void AddMainPanelEvent()
    {
        EventManager.Inst.AddEvent(TGANames.MainPanelOpenChapterPanel,OpenChapterPanel);
        EventManager.Inst.AddEvent(TGANames.MainPanelPeekChapterItem,PeekChapterItem);
        EventManager.Inst.AddEvent(TGANames.MainPanelPeekChapterInfo,PeekChapterInfo);
        EventManager.Inst.AddEvent(TGANames.MainPanelGetChapterInfoSoul,GetChapterInfoSoul);
        EventManager.Inst.AddEvent(TGANames.MainPanelBuyRune,BuyRune);
        EventManager.Inst.AddEvent(TGANames.MainPanelBuyHero,BuyHero);
        EventManager.Inst.AddEvent(TGANames.HeroLevelUpgrade,BuyHeroLevel);
        EventManager.Inst.AddEvent(TGANames.MainPanelSelectHero,SelectHero);
        EventManager.Inst.AddEvent(TGANames.MainPanelPeekHeroSkillInfo,PeekHeroSkillInfo);
        EventManager.Inst.AddEvent(TGANames.MainPanelOpenArtifactPanel,OpenArtifactPanel);
        EventManager.Inst.AddEvent(TGANames.MainPanelOpenMonsterArtifactPanel,OpenMonsterArtifactPanel);
        EventManager.Inst.AddEvent(TGANames.MainPanelOpenItemArtifactPanel,OpenItemArtifactPanel);
        EventManager.Inst.AddEvent(TGANames.MainPanelOpenItemBuildArtifactPanel,OpenItemBuildArtifactPanel);
        
        EventManager.Inst.AddEvent(TGANames.MainPanelShop,OpenMainPanelShop);
        EventManager.Inst.AddEvent(TGANames.EquipmentPanel,OpenEquipmentPanel);
        EventManager.Inst.AddEvent(TGANames.WearEquipment,WearEquipment);
        EventManager.Inst.AddEvent(TGANames.UnloadEquipment,UnloadEquipment);
        EventManager.Inst.AddEvent(TGANames.BuyEquipment,BuyEquipment);
    }

    void RemoveMainPanelEvent()
    {
        EventManager.Inst.RemoveEvent(TGANames.MainPanelOpenChapterPanel,OpenChapterPanel);
        EventManager.Inst.RemoveEvent(TGANames.MainPanelPeekChapterItem,PeekChapterItem);
        EventManager.Inst.RemoveEvent(TGANames.MainPanelPeekChapterInfo,PeekChapterInfo);
        EventManager.Inst.RemoveEvent(TGANames.MainPanelGetChapterInfoSoul,GetChapterInfoSoul);
        EventManager.Inst.RemoveEvent(TGANames.MainPanelBuyRune,BuyRune);
        EventManager.Inst.RemoveEvent(TGANames.MainPanelBuyHero,BuyHero);
        EventManager.Inst.RemoveEvent(TGANames.HeroLevelUpgrade,BuyHeroLevel);
        EventManager.Inst.RemoveEvent(TGANames.MainPanelSelectHero,SelectHero);
        EventManager.Inst.RemoveEvent(TGANames.MainPanelPeekHeroSkillInfo,PeekHeroSkillInfo);
        EventManager.Inst.RemoveEvent(TGANames.MainPanelOpenArtifactPanel,OpenArtifactPanel);
        EventManager.Inst.RemoveEvent(TGANames.MainPanelOpenMonsterArtifactPanel,OpenMonsterArtifactPanel);
        EventManager.Inst.RemoveEvent(TGANames.MainPanelOpenItemArtifactPanel,OpenItemArtifactPanel);
        EventManager.Inst.RemoveEvent(TGANames.MainPanelOpenItemBuildArtifactPanel,OpenItemBuildArtifactPanel);
        
        EventManager.Inst.RemoveEvent(TGANames.MainPanelShop,OpenMainPanelShop);
        EventManager.Inst.RemoveEvent(TGANames.EquipmentPanel,OpenEquipmentPanel);
        EventManager.Inst.RemoveEvent(TGANames.WearEquipment,WearEquipment);
        EventManager.Inst.RemoveEvent(TGANames.UnloadEquipment,UnloadEquipment);
        EventManager.Inst.RemoveEvent(TGANames.BuyEquipment,BuyEquipment);
    }
    

    //进入章节选择Panel
    public void OpenChapterPanel(string arg1, object arg2)
    {
        var data = GetCommonData();
        TGAnalytics.Track(TGANames.MainPanelOpenChapterPanel, data);
    }
    //查看章节Item
    public void PeekChapterItem(string arg1, object arg2)
    {
        var data = GetCommonData();
        int chapterId = (int) arg2;
        data.Add(TGANames.ChapterID,chapterId);
        TGAnalytics.Track(TGANames.MainPanelPeekChapterItem, data);
    }
    //查看章节文案
    public void PeekChapterInfo(string arg1, object arg2)
    {
        var data = GetCommonData();
        int chapterId = (int) arg2;
        data.Add(TGANames.ChapterID,chapterId);
        TGAnalytics.Track(TGANames.MainPanelPeekChapterInfo, data);
    }
    //获取章节文案中的魂
    public void GetChapterInfoSoul(string arg1, object arg2)
    {
        var data = GetCommonData();
        int chapterId = (int) arg2;
        data.Add(TGANames.ChapterID,chapterId);
        TGAnalytics.Track(TGANames.MainPanelGetChapterInfoSoul, data);
    }
    //购买附文
    public void BuyRune(string arg1, object arg2)
    {
        var data = GetCommonData();
        TGAnalytics.Track(TGANames.MainPanelBuyRune, data);
    }
    //购买英雄
    public void BuyHero(string arg1, object arg2)
    {
        var data = GetCommonData();
        data.Add(TGANames.HeroId,(int)arg2);
        TGAnalytics.Track(TGANames.MainPanelBuyHero, data);
    }
    //升级hero
    public void BuyHeroLevel(string arg1, object arg2)
    {
        var data = GetCommonData();
        int heroId = (int) arg2;
        data.Add(TGANames.HeroId, heroId);
        HeroUpgradeInfo upgradeInfo = ArchiveManager.Inst.ArchiveData.GlobalData.HeroUpgradeDatas[heroId];
        int level = upgradeInfo.Level + upgradeInfo.ColorLevel * 4;
        data.Add(TGANames.HeroLevel, level);
        TGAnalytics.Track(TGANames.HeroLevelUpgrade, data);
    }
    //选择英雄
    public void SelectHero(string arg1, object arg2)
    {
        var data = GetCommonData();
        int heroId = (int) arg2;
        data.Add(TGANames.HeroId,heroId);
        TGAnalytics.Track(TGANames.MainPanelSelectHero, data);
    }
    //查看英雄技能
    public void PeekHeroSkillInfo(string arg1, object arg2)
    {
        var data = GetCommonData();
        TGAnalytics.Track(TGANames.MainPanelPeekHeroSkillInfo, data);
    }
    //图鉴
    public void OpenArtifactPanel(string arg1, object arg2)
    {
        var data = GetCommonData();
        TGAnalytics.Track(TGANames.MainPanelOpenArtifactPanel, data);
    }
    //怪物图鉴
    public void OpenMonsterArtifactPanel(string arg1, object arg2)
    {
        var data = GetCommonData();
        TGAnalytics.Track(TGANames.MainPanelOpenMonsterArtifactPanel, data);
    }
    //道具图鉴
    public void OpenItemArtifactPanel(string arg1, object arg2)
    {
        var data = GetCommonData();
        TGAnalytics.Track(TGANames.MainPanelOpenItemArtifactPanel, data);
    }
    ////道具Build图鉴
    public void OpenItemBuildArtifactPanel(string arg1, object arg2)
    {
        var data = GetCommonData();
        TGAnalytics.Track(TGANames.MainPanelOpenItemBuildArtifactPanel, data);
    }

    /// <summary>
    /// 主界面商店
    /// </summary>
    public void OpenMainPanelShop(string arg1, object arg2)
    {
        var data = GetCommonData();
        TGAnalytics.Track(TGANames.MainPanelShop, data);
    }

    /// <summary>
    /// 主界面装备界面
    /// </summary>
    public void OpenEquipmentPanel(string arg1, object arg2)
    {
        var data = GetCommonData();
        TGAnalytics.Track(TGANames.EquipmentPanel, data);
    }


    /// <summary>
    /// 购买装备
    /// </summary>
    public void BuyEquipment(string arg1, object arg2)
    {
        Equipment equip = (Equipment) arg2;
        
        var data = GetCommonData();
        data.Add("EquipId",equip.Id);
        data.Add("EquipScore",equip.Score);
        data.Add("EquipQuality",equip.Quality);
        TGAnalytics.Track(TGANames.BuyEquipment, data);
    }

    /// <summary>
    /// 穿上装备
    /// </summary>
    public void WearEquipment(string arg1, object arg2)
    {
        Equipment equip = (Equipment) arg2;
        
        var data = GetCommonData();
        data.Add("EquipId",equip.Id);
        data.Add("EquipScore",equip.Score);
        data.Add("EquipQuality",equip.Quality);
        TGAnalytics.Track(TGANames.WearEquipment, data);
    }

    /// <summary>
    /// 卸下装备
    /// </summary>
    public void UnloadEquipment(string arg1, object arg2)
    {
        Equipment equip = (Equipment) arg2;
        
        var data = GetCommonData();
        data.Add("EquipId",equip.Id);
        data.Add("EquipScore",equip.Score);
        data.Add("EquipQuality",equip.Quality);
        TGAnalytics.Track(TGANames.UnloadEquipment, data);
    }
}