using System;
using Sirenix.OdinInspector;
using UnityEngine;


public class TrapArrowThrower : MonoBehaviour
{
    public DmgBuffOnTouch DmgBuffOnTrigger;
    [LabelText("每次触发最大生命值%"),Range(0,1)]
    public float damageHpRatio;

    public bool IsIntermittent;
    [ShowIf("IsIntermittent")] public float Interval;

    private float timer;

    private void Start()
    {
        timer = Time.time;
    }

    private void Update()
    {
        if (IsIntermittent)
        {
            if (Time.time>timer+Interval)
            {
                SpawnTrap();
                timer = Time.time;
            }
        }
    }

    void SpawnTrap()
    {
        var go = Instantiate(DmgBuffOnTrigger, transform.position, Quaternion.identity);
        go.InitRatio(BattleManager.Inst.CurrentPlayer, damageHpRatio);
    }
}