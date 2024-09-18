using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.ShieldSkill)]
    [LabelText("持续时间")]
    public float ShieldSkillDuration;
    [ShowIf("EffectType", EffectType.ShieldSkill)]
    [LabelText("转向速度")]
    public float ShieldSkillAng;
    [ShowIf("EffectType", EffectType.ShieldSkill)]
    [LabelText("移动速度")]
    public float ShieldSkillMoveSpeed;
}
public class ShieldSkillEffect : ItemEffect
{
    private float _duration;
    private float _rotateAng;
    private float _moveSpeed;
    public ShieldSkillEffect(float duration,float ang,float moveSpeed)
    {
        this._duration = duration;
        this._rotateAng = ang;
        this._moveSpeed = moveSpeed;
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        EventManager.Inst.AddEvent(EventName.JoyStatus,OnJoyStatus);
    }

    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        EventManager.Inst.RemoveEvent(EventName.JoyStatus,OnJoyStatus);
    }

    private Vector2 _joyValue;
    private void OnJoyStatus(string arg1, object arg2)
    {
        JoyStatusData data = (JoyStatusData) arg2;
        this._joyValue = data.JoyValue;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);

        roleController.StartCoroutine(Move());
    }

    Vector3 RotateDir()
    {
        Vector3 dir = roleController.Animator.transform.forward;
        if (_joyValue != Vector2.zero)
        {
            var forward = roleController.Animator.transform.forward;
            float ang = Vector3.Angle(forward, new Vector3(_joyValue.x, 0, _joyValue.y));
            float value = 1;
            if (Mathf.Abs(ang)>0 && _rotateAng>0)
            {
                value =(_rotateAng*Time.deltaTime)/ang;
                if (value>1)
                {
                    value = 1;
                }
            }
            dir = Vector3.SlerpUnclamped(forward, new Vector3(_joyValue.x, 0, _joyValue.y), value);
        }
        return dir;
    }

    private float timer = 0;
    IEnumerator Move()
    {
        timer = 0;
        roleController.Animator.SetBool("ShieldSkill",true);
        yield return null;
        while (timer < _duration)
        {
            var dir = RotateDir();
            roleController.Rigidbody.velocity = dir.normalized * _moveSpeed;
            roleController.Animator.transform.forward = dir; 
            timer += Time.deltaTime;
            yield return null;
        }
        roleController.Animator.SetBool("ShieldSkill",false);
    }
    
}
