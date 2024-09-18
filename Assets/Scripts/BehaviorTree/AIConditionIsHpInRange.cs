using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("判断血量是否在范围内.")]
public class AIConditionIsHpInRange : AICondition
{
    public SharedFloat MinHp;    
    public SharedFloat MaxHp;

    public override TaskStatus OnUpdate()
    {
        float HpPercent=RoleController.CurrentHp/RoleController.MaxHp;
        

        if (HpPercent > MinHp.Value && HpPercent < MaxHp.Value)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
    
}