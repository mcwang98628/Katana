using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTriggerEffect : ItemEffect
{

    private string paraName;
    private float time;
    public AnimatorTriggerEffect(string paraName, float time)
    {
        this.paraName = paraName;
        this.time = time;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        if (time > 0)
        {
            roleController.Animator.SetBool(paraName, true);
            roleController.StartCoroutine(ResetBool(time));
        }
        else
        {
            roleController.Animator.SetTrigger(paraName);
        }
    }

    IEnumerator ResetBool(float time)
    {
        yield return new WaitForSeconds(time);
        roleController.Animator.SetBool(paraName, false);
    }
 
}
