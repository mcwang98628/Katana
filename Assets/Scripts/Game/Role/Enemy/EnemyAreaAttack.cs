using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAreaAttack : EnemyAttack
{

    public DmgBuffOnTrigger DmageArea;
    public ParticleSystem Particle;
    protected override void Start()
    {
        base.Start();
        DmageArea.Init(roleController, (int)(roleController.AttackPower));
    }

    protected override void AnimEvent(GameObject go, string eventName)
    {
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
        base.AnimEvent(go, eventName);

        if (eventName.Contains(AnimatorEventName.DmgStart_))
        { 
            StartDamageArea();
        }
        else if (eventName.Contains(AnimatorEventName.DmgEnd_))
        {
            EndDamageArea();
        }
        else if (eventName.Contains(AnimatorEventName.ShowDamageAreaIndicate_))
        {
            eventName = eventName.Replace(AnimatorEventName.ShowDamageAreaIndicate_, "");
            var values = eventName.Split('_');
            var r = float.Parse(values[0]);
            var t = float.Parse(values[1]);
            Vector3 pos = DmageArea.transform.position;
            pos.y = 0.1f;
            IndicatorManager.Inst.ShowAttackIndicator().Show(roleController, pos, 360, r, t, Color.red);
        }
    }

    public virtual void StartDamageArea()
    {
        DmageArea.ActiveTrigger();
        Particle.Play();
    }
    public virtual void EndDamageArea()
    {
        DmageArea.CloseTrigger();
        Particle.Stop();
    }
    public void OnDead()
    {
        EndDamageArea();
    }



}
