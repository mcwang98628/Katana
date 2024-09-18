using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;
public class Surround_Obj : MonoBehaviour
{

    public AudioClip TriggerSFX;
    public int Proirity = 0;
    [SerializeField]
    [LabelText("伤害")]
    protected int dmgValue;
    [SerializeField]
    [LabelText("判定半径")]
    protected float distance;
    [LabelText("最短触发间隔(秒)")]
    protected float hitTime = 0.4f;
    [LabelText("触发后消失时间")]
    protected float triggerInterval =0;
    
    protected bool canTrigger=true;

    protected Dictionary<string, RoleController> Enemys => BattleManager.Inst.EnemyTeam; 
    
    protected Dictionary<string,float> hitEnemyDatas = new Dictionary<string, float>();

    [LabelText("法球特效")]
    public ParticleSystem OrbEffect;
    public ParticleSystem ActiveEffect;

    public Surround_Obj SmallObj;

    public virtual void Start()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.5f);
    }

    public void SetDmgValue(int value)
    {
        dmgValue = value;
    }

    public virtual void Update()
    {
        if (!canTrigger)
            return;

        foreach (var enemy in Enemys)
        {
            if (enemy.Value == null)
            {
                continue;
            }
            if(enemy.Value.IsDie)
            {
                continue;                
            }
            var pos = enemy.Value.transform.position - transform.position;
            pos.y=0;
            if (pos.magnitude <= distance)
            {
                if (hitEnemyDatas.ContainsKey(enemy.Key))
                {
                    if (Time.time - hitEnemyDatas[enemy.Key] >= hitTime)
                    {
                        hitEnemyDatas[enemy.Key] = Time.time;
                        Trigger(enemy.Key);
                    }
                }
                else
                {
                    Trigger(enemy.Key);
                    hitEnemyDatas.Add(enemy.Key,Time.time);
                }
            }
        }
    }

    protected virtual void Trigger(string targetRoleId)
    {
        if (TriggerSFX != null)
        {
            TriggerSFX.Play(Proirity);
        }

        if(ActiveEffect!=null)
        ActiveEffect.Play();
        if(OrbEffect!=null)
        OrbEffect.Stop();
        canTrigger = false;
        StartCoroutine(RecoverOrb(triggerInterval));
    }

    IEnumerator RecoverOrb(float time)
    {
        yield return new WaitForSeconds(time);
        if(OrbEffect!=null)
        OrbEffect.Play();
        canTrigger = true;
    }
}
