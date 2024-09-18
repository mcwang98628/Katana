

using System;
using UnityEngine;

public class KeyboardTrigger:ItemEffectTrigger
{
    private string _keyStr;
    private KeyCode _keyCode;
    public KeyboardTrigger(TriggerType type,string keyStr) : base(type)
    {
        _keyStr = keyStr;
        _keyCode = (KeyCode) Enum.Parse(typeof(KeyCode), _keyStr);
    }

    public override void Update(RoleItemController rpe)
    {
        base.Update(rpe);
        if (Input.GetKeyDown(_keyCode))
            Root.itemEffect.TriggerEffect(null);
    }
}