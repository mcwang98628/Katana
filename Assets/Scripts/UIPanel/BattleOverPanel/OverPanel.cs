using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class OverPanel : PanelBase
{
    public BattleOverPanel ProgressPanel;
    


}

public class GameOverData
{
    public bool isVictory;
    public int KillEnemyNumber;
    public int KillSEnemyNumber;
    public int Progress; //0-100
    public int KillPlayerEnemyId = -1;
    public int ClearRoomNuber;//打赢几个房间
    public int LevelIndex;

    public int Exp;

    public int GetDiamond;
    // public int KillEnemyDiamond;
    // public int KillSEnemyDiamond;
    // public int LevelDiamond;
    // public int BattleWinDiamond;

    // public int KillEnemySoul;
    // public int KillSEnemySoul;
    // public int BattleWinSoul;
    // public int LevelSoul;
    // public int SoulValue;


}