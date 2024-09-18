
using UnityEngine;

public class AnimatorEventTrigger:ItemEffectTrigger
{
    private string _eventName;

    public AnimatorEventTrigger(TriggerType type, string eventName) : base(type)
    {
        _eventName = eventName;
    }


    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddAnimatorEvent(OnAnimEvent);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveAnimatorEvent(OnAnimEvent);
    }

    private void OnAnimEvent(GameObject gameObject, string eventName)
    {
        if (gameObject != roleItemController.roleController.Animator.gameObject || 
            eventName != _eventName)
            return;

        
        Root.itemEffect.TriggerEffect(new ItemEffectTriggerValue()
            {
                TargetId = roleItemController.roleController.TemporaryId,
                TargetPosition = roleItemController.roleController.transform.position,
                TargetRole = roleItemController.roleController
            }
        ); 
    }
}