using System.ComponentModel;
using UnityEngine;

public partial class SROptions
{
    

    [Category("Hero相关")]
    public void DelectHeroData()
    {
        if (ArchiveManager.Inst.ArchiveData.GlobalData.HeroUpgradeDatas.ContainsKey(HeroId))
        {
            HeroUpgradeInfo upgradeInfo = ArchiveManager.Inst.ArchiveData.GlobalData.HeroUpgradeDatas[HeroId];
            upgradeInfo.Level = 0;
            upgradeInfo.ColorLevel = 1;
            upgradeInfo.UseingExp = 0;
        }
        ArchiveManager.Inst.SaveArchive();
        Debug.LogError("删除英雄升级数据");
    }

    private int _heroId;
    [Category("Hero相关")]
    public int HeroId
    {
        get
        {
            return _heroId;
        }
        set
        {
            _heroId = value;
        }
    }
    
    
    [Category("存档！！！！)")]
    public void NewArchive()
    {
        ArchiveManager.Inst.NewArchive();
        Application.Quit();
    }
    
    #if TEST_MODE

    [Category("TestMode(On/Off)")]
    public void TestModeOnOff()
    {
        GameManager.Inst.TestMode = !GameManager.Inst.TestMode; 
        Debug.LogError($"TestMode{GameManager.Inst.TestMode}");
    }
    #endif
}