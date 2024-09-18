using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BehaviorExpansion/MyAI")]
public class AIRoleRandomTarget : Action
{
    public SharedVector3 OriginV3;
    public SharedVector3 TargetV3;
    public SharedFloat RandomDis;
    
    public override void OnStart()
    {
        TargetV3.Value = Random();
    }

    
    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
            
    }

    Vector3 Random()
    {
        float x = UnityEngine.Random.Range(OriginV3.Value.x - RandomDis.Value,OriginV3.Value.x + RandomDis.Value);
        float y = UnityEngine.Random.Range(OriginV3.Value.y - RandomDis.Value,OriginV3.Value.y + RandomDis.Value);
        return new Vector3(x, 0, y);
    }
    
    
}








