using System;
using UnityEngine;


public class PlayerInputAction : MonoBehaviour
{
#if UNITY_ANDROID || UNITY_IOS
    public Action<JoyStatusData> OnInputEvent = delegate(JoyStatusData data) {  };
    public virtual void OnInput(JoyStatusData statusData)
    {
        OnInputEvent.Invoke(statusData);
    }
#else
    public Action OnInputEvent = delegate() {  };
    public virtual void OnInput()
    {
        OnInputEvent.Invoke();
    }
#endif
}
