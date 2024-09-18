using System.ComponentModel;
using UnityEngine;

public partial class SROptions
{
    [Category("资源相关")]
    public void AddGold()
    {
        if (!BattleManager.Inst.GameIsRuning)
        {
            Debug.LogError("游戏还未开始。");
            return;
        }
        BattleManager.Inst.AddGold(GoldCount);
    }

    private int goldCount;

    [Category("资源相关")]
    public int GoldCount
    {
        get => goldCount;
        set => goldCount = value;
    }
    
    
    
    [Category("外围资源")]
    public void AddDiamond()
    {
        ArchiveManager.Inst.ChangeDiamond(DiamondCount);
    }

    private int diamondCount;

    [Category("外围资源")]
    public int DiamondCount
    {
        get => diamondCount;
        set => diamondCount = value;
    }
    [Category("外围资源")]
    public void AddEXP()
    {
        ArchiveManager.Inst.ChangeExp(ExpValue);
    }

    private int expValue;

    [Category("外围资源")]
    public int ExpValue
    {
        get => expValue;
        set => expValue = value;
    }
    [Category("外围资源")]
    public void AddSoul()
    {
        ArchiveManager.Inst.ChangeSoul(SoulValue);
    }

    private int soulValue;

    [Category("外围资源")]
    public int SoulValue
    {
        get => soulValue;
        set => soulValue = value;
    }
}