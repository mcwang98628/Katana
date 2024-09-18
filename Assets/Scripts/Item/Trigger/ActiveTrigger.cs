using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTrigger : ItemEffectTrigger
{
    public ActiveTrigger(TriggerType type) : base(type) {}

    public void Active()
    {
        if (roleItemController.roleController.IsDie)
        {
            return;
        }
        Root.itemEffect.TriggerEffect(null);
    }

    public void OnBtnTouch(bool isDown)
    {
        if (roleItemController.roleController.IsDie)
        {
            return;
        }
        Root.itemEffect.OnBtnTouch(isDown);
    }

    public void OnDrag(bool isDown,Vector2 dir)
    {
        
        if (roleItemController.roleController.IsDie)
        {
            return;
        }
    }
}
