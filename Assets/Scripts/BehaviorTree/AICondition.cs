using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

public class AICondition : Conditional
{
    protected RoleController RoleController;
    protected RoleController Target;

    public override void OnAwake()
    {
        base.OnAwake();
        RoleController = transform.GetComponent<RoleController>();
        //Target = BattleManager.Inst.CurrentPlayer;
        Target = BattleTool.FindNearestEnemy(RoleController);
    }
    public override void OnStart()
    {
        //Target = BattleManager.Inst.CurrentPlayer;
        Target = BattleTool.FindNearestEnemy(RoleController);

    }
}