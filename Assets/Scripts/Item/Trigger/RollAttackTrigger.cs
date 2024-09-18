using UnityEngine;

public class RollAttackTrigger:ItemEffectTrigger
{
    public RollAttackTrigger(TriggerType type) : base(type)
    {
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.OnRoleAttack,OnRoleAttack);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.AddEvent(EventName.OnRoleAttack,OnRoleAttack);
    }
    

    private void OnRoleAttack(string arg1, object arg2)
    {
        string tId = (string) arg2;
        if (tId != roleItemController.roleController.TemporaryId || 
            !roleItemController.roleController.roleAttack.IsRollAttacking)
            return;
        
        Root.itemEffect.TriggerEffect(null);
    }
    
    
}