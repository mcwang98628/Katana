using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
    [TaskCategory("Unity/Transform")]
    [TaskDescription("Returns Success if the transform is in range.")]
    public class IsInRange : Conditional
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [Tooltip("The interested transform")]
        public SharedFloat range;


        public override void OnStart()
        {
        }

        public override TaskStatus OnUpdate()
        {
            if (targetGameObject == null) {
                Debug.LogWarning("Transform is null");
                return TaskStatus.Failure;
            }
            float distance=Vector3.Distance(transform.position,targetGameObject.Value.transform.position);
            Debug.Log("distance="+distance);
            return distance<range.Value? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            targetGameObject = null;
        }
    }
}