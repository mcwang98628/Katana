
//战斗过程中数据

using System;

public interface IBattleRuntimeData 
{
    /// <summary>
    /// 关卡结构数据
    /// </summary>
    IBattleLevelStructData LevelStructData { get; }
    
    /// <summary>
    /// 本场战斗唯一Id 
    /// </summary>
    Guid BattleGuid { get; }
    
    /// <summary>
    /// 游戏进度 0-100
    /// </summary>
    int Progress { get; }

    bool IsTutorial { get; }

    void SetBattleGuid(Guid guid);
    void SetLevelStructData(IBattleLevelStructData levelStructData);
}
