using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnBossDie : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.Inst.AddEvent(EventName.OnBossDead, OnBossDeadEvent);
    }
    private void OnDisable()
    {
        EventManager.Inst.RemoveEvent(EventName.OnBossDead, OnBossDeadEvent);
    }

    private void OnBossDeadEvent(string arg1, object arg2)
    {
        Destroy(gameObject);
    }


}
