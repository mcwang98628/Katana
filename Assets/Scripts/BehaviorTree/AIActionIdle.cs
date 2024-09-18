using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
public class AIActionIdle : AIAction
{
    public SharedFloat IdleTime;
    float timer;
    public override void OnStart()
    {
        RoleController.InputMove(Vector2.zero);
        timer = Time.time;
    }

    public override TaskStatus OnUpdate()
    {
        if (Time.time - timer > IdleTime.Value)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Running;
        }

    }

}