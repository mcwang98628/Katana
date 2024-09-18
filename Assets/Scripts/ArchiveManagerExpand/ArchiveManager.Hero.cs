using UnityEngine;

public partial class ArchiveManager : TSingleton<ArchiveManager>
{

    public ArchiveErrorType HeroUpgradeColorLevel(int heroId)
    {
        var heroUpgrade = GetHeroUpgradeData(heroId);
        if (heroUpgrade == null)
        {
            Debug.LogError("严重错误！！！");
            return ArchiveErrorType.ErrorNotFound;
        }
        HeroUpgradeData currentUpgrade = heroUpgrade.GetHeroUpgradeData();
        if (ArchiveData.GlobalData.Soul < currentUpgrade.NeedExp)
        {
            return ArchiveErrorType.SourShortage;
        }
        if (IsCanUpgradeColorLevel(currentUpgrade))
        {
            var heroUpgradeData = 
                DataManager.Inst.GetHeroUpgradeData(
                    currentUpgrade.HeroId, 
                    currentUpgrade.ColorLevel + 1, 
                    0);
            
            if (heroUpgradeData != null)
                archiveData.GlobalData.HeroUpgradeDatas[currentUpgrade.HeroId].SetHeroUpgradeData(heroUpgradeData.Value);
            
            ChangeSoul(-currentUpgrade.NeedExp);
            
            EventManager.Inst.DistributeEvent(EventName.HeroUpgradedeColorLevel,heroId);
            EventManager.Inst.DistributeEvent(TGANames.HeroLevelUpgrade,heroId);
            SaveArchive();
            return ArchiveErrorType.NoError;
        }

        return ArchiveErrorType.MaxLevel;
    }
    public ArchiveErrorType HeroUpgrade(int heroId,int useExp)
    {
        var heroUpgrade = GetHeroUpgradeData(heroId);
        if (heroUpgrade == null)
        {
            Debug.LogError("严重错误！！！");
            return ArchiveErrorType.ErrorNotFound;
        }
        HeroUpgradeData currentUpgrade = heroUpgrade.GetHeroUpgradeData();

        if (archiveData.GlobalData.Soul <= 0)
        {
            return ArchiveErrorType.SourShortage;
        }

        if (IsCanUpgradeLevel(currentUpgrade))
        {
            var useingExp = archiveData.GlobalData.HeroUpgradeDatas[currentUpgrade.HeroId].UseingExp;
            if (useingExp < currentUpgrade.NeedExp)
            {
                var value = currentUpgrade.NeedExp - useingExp;
                if (value < useExp)
                    useExp -= (useExp - value);
                if (archiveData.GlobalData.Soul < useExp)
                    useExp = archiveData.GlobalData.Soul;
                
                archiveData.GlobalData.HeroUpgradeDatas[currentUpgrade.HeroId].UseingExp += useExp;
                ChangeSoul(-useExp);
            }

            if (useingExp >= currentUpgrade.NeedExp)
            {
                //清空经验
                archiveData.GlobalData.HeroUpgradeDatas[currentUpgrade.HeroId].UseingExp -= currentUpgrade.NeedExp;
                
                
                var heroUpgradeData = 
                    DataManager.Inst.GetHeroUpgradeData(
                        currentUpgrade.HeroId, 
                        currentUpgrade.ColorLevel, 
                        currentUpgrade.Level + 1);
                if (heroUpgradeData != null)
                    archiveData.GlobalData.HeroUpgradeDatas[currentUpgrade.HeroId].SetHeroUpgradeData(heroUpgradeData.Value);
                
                EventManager.Inst.DistributeEvent(EventName.HeroUpgradede);
                EventManager.Inst.DistributeEvent(TGANames.HeroLevelUpgrade,heroId);
                SaveArchive();
            }
            return ArchiveErrorType.NoError;
        }
        
        return ArchiveErrorType.MaxLevel;
    }

    
    /// <summary>
    /// 获取当前存档的英雄升级数据
    /// </summary>
    /// <param name="heroId">英雄Id</param>
    public HeroUpgradeInfo GetHeroUpgradeData(int heroId)
    {
        if (ArchiveData.GlobalData.HeroUpgradeDatas.ContainsKey(heroId))
            return ArchiveData.GlobalData.HeroUpgradeDatas[heroId];

        return null;
    }

    public bool IsCanUpgradeLevel(HeroUpgradeData heroUpgradeData)
    {
        return DataManager.Inst.GetHeroUpgradeData(
            heroUpgradeData.HeroId, 
            heroUpgradeData.ColorLevel,
            heroUpgradeData.Level + 1) != null;
    }
    public bool IsCanUpgradeColorLevel(HeroUpgradeData heroUpgradeData)
    {
        return DataManager.Inst.GetHeroUpgradeData(
            heroUpgradeData.HeroId, 
            heroUpgradeData.ColorLevel+1,
            1) != null;
    }
    
    public ArchiveErrorType BuyHero(int heroId)
    {
        var data = DataManager.Inst.GetHeroData(heroId);
        
        if (data.Price > archiveData.GlobalData.Diamond)
        {
            return ArchiveErrorType.DiamondShortage;
        }
        if (archiveData.GlobalData.UnLockHeros.Contains(heroId))
        {
            return ArchiveErrorType.AlreadyOwned;
        }
        else
        {
            archiveData.GlobalData.UnLockHeros.Add(heroId);
            // ReSharper disable once PossibleInvalidOperationException
            archiveData.GlobalData.HeroUpgradeDatas.Add(heroId,new HeroUpgradeInfo());
            archiveData.GlobalData.HeroUpgradeDatas[heroId].SetHeroUpgradeData(DataManager.Inst.GetHeroUpgradeData(heroId,1,0).Value);
            ChangeDiamond(-data.Price);
            EventManager.Inst.DistributeEvent(EventName.OnBuyHero,heroId);
            ChangeExp(120);
            SaveArchive();
            return ArchiveErrorType.NoError;
        }
    }
    public void SetLastSelectHero(int heroId)
    {
        archiveData.GlobalData.LastSelectHeroID = heroId;
        EventManager.Inst.DistributeEvent(EventName.SelectHero,heroId);
        SaveArchive();
    }

}