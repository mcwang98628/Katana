using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("吸血攻击。")]
public class AIActionVampireAttack : AIAction
{
    public SharedFloat attackTime;
    public SharedFloat attackDis;

    private bool isOver;
    public override void OnStart()
    {
        base.OnStart();
        var uAttacker = RoleController.roleAttack as EnemyVampireAttack;
        if (uAttacker)
        {
            isOver = false;
            uAttacker.VampireAttack(BattleManager.Inst.CurrentPlayer,attackTime.Value,attackDis.Value,
                () =>
                {
                    isOver = true;
                });
        }
        else
        {
            isOver = true;
        }
    }
    

    public override TaskStatus OnUpdate()
    {
        if (isOver)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Running;
        }
    }

}