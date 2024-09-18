using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlackHole : MonoBehaviour
{
    public DmgBuffOnTouch BlackHoleDObj;
    public AudioClip BlackHoleAudio;
    public float MaxInterval = 5;
    public float MinInterval = 1;
    float lastAttackTime;
    float interval;
    RoleController target;
    
    private void Start()
    {
        lastAttackTime = 0;
        interval = Random.Range(MinInterval, MaxInterval);
        target = null;
        EventManager.Inst.AddEvent(EventName.EnterNextRoom, ResetLastAttackTime);
    }
    private void FixedUpdate()
    {
        if (Time.time - lastAttackTime > interval)
        {
            BlackHoleAttack(target);
        }
    }

    void ResetLastAttackTime(string arg1 = "", object arg2 = null)
    {
        interval = Random.Range(MinInterval, MaxInterval);
        lastAttackTime = Time.time;
    }

    void BlackHoleAttack(RoleController target)
    {
        if (target == null)
        {
            return;
        }
            Instantiate(BlackHoleDObj, target.transform.position, Quaternion.identity).GetComponent<DmgBuffOnTouch>().Init(BattleManager.Inst.CurrentPlayer);
        //transform.position = target.transform.position;
        //AudioManager.Inst.PlaySource(BlackHoleAudio);

        //target.HpInjured(new DamageInfo(9999, BattleManager.Inst.CurrentPlayer, transform.position, DmgType.Other));
        //target.transform.DOScale(Vector3.zero,2).SetEase(Ease.Linear);
        ResetLastAttackTime();
        target = null;
    }



}
