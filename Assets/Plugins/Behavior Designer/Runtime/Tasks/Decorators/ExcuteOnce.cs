namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Return Failure after excute first time")]
    [TaskIcon("{SkinColor}InverterIcon.png")]
    public class ExcuteOnce : Decorator
    {
        // The status of the child after it has finished running.
        private TaskStatus executionStatus = TaskStatus.Inactive;
        private bool excuted = false;
        public override bool CanExecute()
        {
            if (excuted)
                return false;
            else
                excuted = true;
            // Continue executing until the child task returns success or failure.
            return executionStatus == TaskStatus.Inactive || executionStatus == TaskStatus.Running;
        }

        public override void OnChildExecuted(TaskStatus childStatus)
        {
            // Update the execution status after a child has finished running.
            executionStatus = childStatus;
        }

        public override TaskStatus Decorate(TaskStatus status)
        {
            if (status == TaskStatus.Success)
            {
                if (!excuted)
                {
                    return TaskStatus.Success;
                }
                else
                {
                    return TaskStatus.Failure;
                }
            }
            return status;
        }

        public override void OnEnd()
        {
            // Reset the execution status back to its starting values.
            executionStatus = TaskStatus.Inactive;
        }
    }
}