using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
    [TaskCategory("Unity/Physics")]
    [TaskDescription("Returns Success if the forward direction has no block.")]
    public class IsNoBlocked : Conditional
    {
        public LayerMask layerMask = -1;
        public SharedFloat rayDistance;


        public override void OnStart()
        {
        }

        public override TaskStatus OnUpdate()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance.Value == -1 ? Mathf.Infinity : rayDistance.Value, layerMask))
            {
                return TaskStatus.Failure;
            }
            else
            {
                return TaskStatus.Success;
            }

        }

        public override void OnReset()
        {
        }
    }
}