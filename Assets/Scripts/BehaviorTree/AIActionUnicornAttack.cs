using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("独角仙的攻击。")]
public class AIActionUnicornAttack : AIAction
{
    public SharedFloat AttackMoveWarningTime;
    public SharedFloat MoveSpeed;
    public SharedFloat MoveTime;
    public SharedInt AttackState;
    public SharedVector3 Dir;
    public override void OnStart()
    {
        base.OnStart();
        RoleController.InputAttack(AttackState.Value);
        var uAttacker = RoleController.roleAttack as EnemyUnicorn_Attacker;
        if (uAttacker)
        {
            Dir.Value = BattleManager.Inst.CurrentPlayer.transform.position - RoleController.transform.position;
            uAttacker.AttackFunc(Dir.Value,MoveSpeed.Value*MoveTime.Value ,AttackMoveWarningTime.Value);
        }
    }
    

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }

}