using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderCloud : MonoBehaviour
{
    public ParticleSystem ThunderParticles;
    public AudioClip ThunderAudio;
    public int AttackPower = 100;
    public float AttackRange = 2;
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
    }
    private void FixedUpdate()
    {
        target = BattleTool.FindNearestEnemy(transform, AttackRange);
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < AttackRange)
            {
                if (Time.time - lastAttackTime > interval)
                {
                    ThunderAttack();
                }
            }
        }
    }


    void ThunderAttack()
    {
        ThunderParticles.Play();
        AudioManager.Inst.PlaySource(ThunderAudio);
        target.HpInjured(new DamageInfo(target.TemporaryId,AttackPower,BattleManager.Inst.CurrentPlayer,transform.position, DmgType.Thunder));

        interval = Random.Range(MinInterval, MaxInterval);
        lastAttackTime = Time.time;
        target = null;
    }

}
