using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("史莱姆跳跃攻击。")]
public class AIActionSlimeJumpAttack : AIAction
{
    public SharedFloat WarningTime;
    public SharedVector3 MovePos;
    
    public override void OnStart()
    {
        base.OnStart();
        var uAttacker = RoleController.roleAttack as EnemySlime_Attacker;
        if (uAttacker)
        {
            MovePos.Value = BattleManager.Inst.CurrentPlayer.transform.position;
            uAttacker.FlyAttack(MovePos.Value,1,WarningTime.Value);
        }
    }
    

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }

}
[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("史莱姆跳跃攻击。")]
public class AIActionSlimeDownAttack : AIAction
{
    public SharedVector3 MovePos;
    
    public override void OnStart()
    {
        base.OnStart();
        var uAttacker = RoleController.roleAttack as EnemySlime_Attacker;
        if (uAttacker)
        {
            uAttacker.DownAttack(MovePos.Value);
        }
    }
    

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }

}