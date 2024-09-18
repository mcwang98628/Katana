using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
    [TaskCategory("Unity/Time")]
    [TaskDescription("若当前时间大于传入时间加上附加时间，则返回success")]
    public class CompareTime : Conditional
    {
        [Tooltip("The Last Time to compare")]
        public SharedFloat LastTime;
        [Tooltip("Add time")]
        public SharedFloat AddTime;

        public override TaskStatus OnUpdate()
        {
            return Time.time>(LastTime.Value+AddTime.Value) ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            LastTime = 0;
            AddTime = 0;
        }
    }
}