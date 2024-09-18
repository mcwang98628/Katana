using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class TrapFlameThrower : MonoBehaviour
{
    public DmgBuffOnTrigger DmgBuffOnTrigger;
    [LabelText("每次触发最大生命值%"),Range(0,1)]
    public float damageHpRatio;

    //移动
    [Header("移动")]
    public bool IsMove;
    [ShowIf("IsMove")] public Transform StartPoint;
    [ShowIf("IsMove")] public Transform EndPoint;
    [ShowIf("IsMove")] public float MoveSpeed;
    Vector3 moveDirection;
    float moveTime;
    //间歇
    [Header("间歇")]
    public bool IsIntermittent;
    [ShowIf("IsIntermittent")] public ParticleSystem Particle;
    [ShowIf("IsIntermittent")] public float Interval;
    [ShowIf("IsIntermittent")] public float Delay;


    float timer;
    bool IsOn;
    void Start()
    {
        DmgBuffOnTrigger.Init(BattleManager.Inst.CurrentPlayer, damageHpRatio);
        DmgBuffOnTrigger.ActiveTrigger();
        timer = Time.time;
        if (IsMove)
        {
            DmgBuffOnTrigger.transform.position = StartPoint.transform.position;
            moveDirection = (EndPoint.transform.position - StartPoint.transform.position).normalized;
            moveTime = Vector3.Distance(EndPoint.transform.position, StartPoint.transform.position) / MoveSpeed;
        }
        if(IsIntermittent)
        {
            timer=Time.time+Delay;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMove)
        {
            if (Time.time > timer + moveTime)
            {
                moveDirection = -moveDirection;
                timer = Time.time;
            }
            DmgBuffOnTrigger.transform.position += moveDirection * MoveSpeed * Time.deltaTime;
        }
        if (IsIntermittent)
        {
            if (Time.time > timer + Interval)
            {
                SwtichTrap();
                if (!IsOn)
                {
                    StartCoroutine(DelayShowIndicator());
                }
                timer = Time.time;
            }
        }
    }
    IEnumerator DelayShowIndicator()
    {

        yield return new WaitForSeconds(Interval - 1f);
        DmgBuffOnTrigger.transform.DOShakeScale(1f, 0.15f, 50, 60, false);
    }
    void SwtichTrap()
    {
        if (IsOn)
        {
            DmgBuffOnTrigger.CloseTrigger();
            Particle.Stop();
        }
        else
        {
            DmgBuffOnTrigger.ActiveTrigger();
            Particle.Play();
        }
        IsOn = !IsOn;
    }
}
