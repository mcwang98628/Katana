using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AttackIndeicate_Rect : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer meshRenderer;
    private RoleController target;
    private Vector3 offset;
 
    public void Show(RoleController _target,Vector3 offset,Vector3 dir,Vector3 initial,Vector3 targetV3,float time,Action callBack=null,bool autoRecover = true)
    {
        gameObject.SetActive(true);
        target = _target;
        this.offset = offset;
        transform.position = target.transform.position+offset;
        transform.forward = dir;
        transform.localScale = initial;
        transform.DOScale(targetV3, time-0.2f>0?time-0.2f:time).SetEase(Ease.OutCubic);
        /*
       transform.DOScale(targetV3, time).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            if (callBack!=null)
            {
                callBack.Invoke();
            }
            IndicatorManager.Inst.Recover(this);
        });
        */
        SetColor(new Color(1, 0, 0, 0));
        // meshRenderer.material.SetColor("_BaseColor",new Color(1, 0, 0, 0) );
        meshRenderer.material.DOColor(new Color(1,0,0,0.3f),"_BaseColor",time-0.05f).SetEase(Ease.OutQuart);

        if (autoRecover)
        {
            StartCoroutine(ShowCallBack(time, callBack));
        }

        DotweenCoverRange(time);
    }
    public void Show(Transform targetTrans,Vector3 initial,Vector3 targetV3,float time,Action callBack=null,bool autoRecover = true)
    {
        gameObject.SetActive(true);
        target = null;
        this.offset = Vector3.zero;
        transform.position = new Vector3(targetTrans.position.x,0.1f,targetTrans.position.z);
        transform.parent=targetTrans;
        transform.forward = targetTrans.forward;
        transform.localScale = initial;
        transform.DOScale(targetV3, time-0.2f>0?time-0.2f:time).SetEase(Ease.OutCubic);
        SetColor(new Color(1, 0, 0, 0));
        meshRenderer.material.DOColor(new Color(1,0,0,0.3f),"_BaseColor",time-0.05f).SetEase(Ease.OutQuart);

        if (autoRecover)
        {
            StartCoroutine(ShowCallBack(time, callBack));
        }

        DotweenCoverRange(time);
    }
    
    public void SetColor(Color color)
    {
        meshRenderer.material.SetColor("_BaseColor",new Color(color.r,color.g,color.b,0.35f));
        meshRenderer.material.SetColor("_ColorColor",new Color(color.r,color.g,color.b,0.5f));
    }
    private static readonly int CoverRange = Shader.PropertyToID("_CoverRange");
    public void DotweenCoverRange(float time)
    {
        DOTween.To(() => meshRenderer.material.GetFloat(CoverRange), value =>
        {
            meshRenderer.material.SetFloat(CoverRange, value);
        }, 1, time);
    }
    IEnumerator ShowCallBack(float delayTime ,Action callBack)
    {
        yield return new WaitForSeconds(delayTime);
        if (callBack != null)
        {
            callBack.Invoke();
        }
        IndicatorManager.Inst.Recover(this);

    }
    public void ClearTarget()
    {
        target = null;
    }
    private void Update()
    {
        if (target != null)
        {
            // transform.position = target.transform.position+offset;
            
            
            if(target.IsDie)
            {
                target = null;
                IndicatorManager.Inst.Recover(this);
            }
        }
    }
}
