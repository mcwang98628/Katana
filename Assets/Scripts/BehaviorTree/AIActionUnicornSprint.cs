using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("独角仙的冲刺。")]
public class AIActionUnicornSprint : AIAction
{
    public SharedFloat MoveTime;
    public SharedFloat MoveSpeed;
    public SharedVector3 MoveDir;
    private bool isSpring;
    public override void OnStart()
    {
        base.OnStart();
        var uAttacker = RoleController.roleAttack as EnemyUnicorn_Attacker;
        if (uAttacker)
        {
            isSpring = true;
            uAttacker.StartSprint(MoveTime.Value,MoveSpeed.Value,MoveDir.Value, () =>
            {
                isSpring = false;
            });
        }
    }
    

    public override TaskStatus OnUpdate()
    {
        if (isSpring)
        {
            return TaskStatus.Running;
        }
        else
        {
            return TaskStatus.Success;
        }
    }

}