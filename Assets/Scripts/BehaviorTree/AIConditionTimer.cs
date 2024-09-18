using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
[TaskDescription("Timer比较特殊，即是判断也是行为。作为判断是判断是否到达间隔时间，行为是到达后设置新的间隔")]
public class AIConditionTimer : AICondition
{
    public SharedFloat Interval = 0;
    public SharedFloat Timer;
    public override TaskStatus OnUpdate()
    {
        if(Time.time>Timer.Value+Interval.Value)
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }
    
}