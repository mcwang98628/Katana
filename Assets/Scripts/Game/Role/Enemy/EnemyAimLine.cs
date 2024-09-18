using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyAimLine : MonoBehaviour
{

    protected RoleController roleController;
    public LineRenderer LineRender;
    public Transform ProjectSpawnPoint;
    bool isShowLine;
    float lineWidth;

    void Start()
    {
        isShowLine = false;
        roleController = GetComponent<RoleController>();
        EventManager.Inst.AddAnimatorEvent(AnimEvent);
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveAnimatorEvent(AnimEvent);
    }

    void ShowLine(float time)
    {
        
        lineWidth = 1.0f;
        DOTween.To(() => lineWidth, x => lineWidth = x, 0.0f, time)
            .SetEase(Ease.InCubic)
            .OnComplete(()=>isShowLine=false);

        isShowLine = true;


    }
    private void FixedUpdate()
    {
        if (isShowLine)
        {
            LineRender.gameObject.SetActive(true);
            LineRender.SetPosition(0, ProjectSpawnPoint.position);
            RaycastHit hit;
            if (Physics.Raycast(ProjectSpawnPoint.position, roleController.Animator.transform.forward, out hit,0<<LayerMask.NameToLayer("Enemy")))
            {
                LineRender.SetPosition(1, hit.point);
            }
            else
            {
                LineRender.SetPosition(1, ProjectSpawnPoint.position + 12 * roleController.Animator.transform.forward);
            }

            LineRender.widthMultiplier = lineWidth;
        }
        else
        {
            LineRender.gameObject.SetActive(false);
        }
    }

    protected void AnimEvent(GameObject go, string eventName)
    {
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
        if (eventName.Contains("ShowAimLine_"))
        {
            eventName = eventName.Replace("ShowAimLine_", "");
            float showTime = float.Parse(eventName);
            ShowLine(showTime);

        }
    }
}
