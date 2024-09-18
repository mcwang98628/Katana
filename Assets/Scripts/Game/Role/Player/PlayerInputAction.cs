using System;
using UnityEngine;


public class PlayerInputAction : MonoBehaviour
{
    public Action<JoyStatusData> OnInputEvent = delegate(JoyStatusData data) {  };
    public virtual void OnInput(JoyStatusData statusData)
    {
        OnInputEvent.Invoke(statusData);
    }
}
