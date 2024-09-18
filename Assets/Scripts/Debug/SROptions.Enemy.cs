using System.ComponentModel;
using UnityEngine;

public partial class SROptions
{
    
    [Category("Enemy")]
    public void CreateEnemyById()
    {
        if (!BattleManager.Inst.GameIsRuning)
        {
            Debug.LogError("游戏未开始。");
            return;
        }

        BattleTool.CreateEnemy(CreateEnemyId, delegate(EnemyController controller)
        {
            controller.transform.position = BattleManager.Inst.CurrentPlayer.transform.position;
        });
    }

    private int createEnemyId;

    [Category("Enemy")]
    public int CreateEnemyId
    {
        get => createEnemyId;
        set => createEnemyId = value;
    }
}