using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EndlessRulesRuntimeData : BattleRuntimeBaseData
{
    public override int CurrentChapterId { get; }
    
    public override IBattleLevelStructData LevelStructData => _endlessStructData;
    private EndlessStructData _endlessStructData;

    public override int Progress { get; } = 10;
    public override bool IsTutorial => false;

    public override void SetLevelStructData(IBattleLevelStructData levelStructData)
    {
        if (levelStructData is EndlessStructData endlessStructData)
        {
            _endlessStructData = endlessStructData;
        }
        else
        {
            throw new NotImplementedException();
        }
    }

}
