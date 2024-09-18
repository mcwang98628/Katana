using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Bullet : AOETouch
{
    [BoxGroup("Bullet",true,true)] 
    [SerializeField][LabelText("击中敌人后销毁")]
    private bool destroyOnHit = false;
    [SerializeField] [LabelText("碰撞到指定Layer后销毁")]
    private LayerMask hitDestroyLayer;
    [BoxGroup("Bullet",true,true)]
    [SerializeField] [LabelText("飞行速度")] 
    private float flySpeed = 1;
    [BoxGroup("Bullet",true,true)]
    [SerializeField] [LabelText("使用速度曲线")]
    private bool useSpeedCurve=false;
    [BoxGroup("Bullet",true,true)]
    [SerializeField] [LabelText("速度曲线")] [ShowIf("useSpeedCurve")]
    private AnimationCurve speedCurve;
    [BoxGroup("Bullet",true,true)]
    [SerializeField] [LabelText("使用缩放曲线")]
    private bool useScaleCurve = false;
    [BoxGroup("Bullet",true,true)]
    [SerializeField] [LabelText("速度曲线")] [ShowIf("useScaleCurve")]
    private AnimationCurve scaleCurve;
    [BoxGroup("Bullet",true,true)]
    [SerializeField] [LabelText("使用移动扰动曲线")]
    private bool useMoveDisturb;
    [BoxGroup("Bullet",true,true)]
    [SerializeField] [LabelText("移动扰动曲线")] [ShowIf("useMoveDisturb")]
    private AnimationCurve moveDisturbCurve;

    [HideInInspector]
    public bool IsFlying = true;
    
    private float _initRadius;
    private Vector3 _initForward;
    private Vector3 _initScale;


    public void Init(RoleController role,Vector3 forward)
    {
        transform.forward = forward;
        base.Init(role);
        
        _initRadius = radius;
        _initForward = transform.forward;
        _initScale = transform.localScale;
        
        
        float curve = scaleCurve.Evaluate((Time.time - InitTime));
        transform.localScale = _initScale * curve;
        radius = _initRadius * curve;
    }

    protected virtual void FixedUpdate()
    {
        if (!IsFlying)
            return;
        if(useSpeedCurve)
        {
            transform.position += transform.forward * flySpeed*speedCurve.Evaluate((Time.time - InitTime)) * Time.fixedDeltaTime;
        }
        else
        {
            transform.position += transform.forward * flySpeed * Time.fixedDeltaTime;
        }
        if (useScaleCurve)
        {
            float curve = scaleCurve.Evaluate((Time.time - InitTime));
            transform.localScale = _initScale * curve;
            radius = _initRadius * curve;
        }

        if (useMoveDisturb)
        {
            transform.forward = _initForward + transform.right * moveDisturbCurve.Evaluate((Time.time - InitTime));
        }
        
        bool isHit =  Physics.Raycast(transform.position,transform.forward, flySpeed * Time.fixedDeltaTime,hitDestroyLayer);
        if (isHit)
        {
            Destroy(gameObject);
        }

    }

    protected override void DoDmgAndBuff(List<RoleController> targets)
    {
        base.DoDmgAndBuff(targets);
        if (targets.Count > 0 && destroyOnHit)
        {
            StartCoroutine(waitDestroy());
        }
    }

    IEnumerator waitDestroy()
    {
        yield return null;
        Destroy(gameObject);
    }
}
