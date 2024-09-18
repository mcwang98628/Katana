using System.Collections;
using UnityEngine;
using DG.Tweening;

public class AttackIndeicate_Sector : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    private static readonly int Angle = Shader.PropertyToID("_Angle");
    private RoleController role;
    private bool hideOnRoleDie;
    
    
    private static readonly int CoverRange = Shader.PropertyToID("_CoverRange");
    
    private void Update()
    {
        if (role!=null) 
        {
            if (role.IsDie && hideOnRoleDie)
            {
                Recover();
                role = null;
            }
            
        }
    }

    public void Show(RoleController role,Vector3 pos,float ang,float size,float time,Color color, bool hideOnRoleDie=false)
    {
        this.SetRole(role).SetPosition(pos).SetAngle(ang).SetSize(size).SetEnable(true).SetTime(time).
            SetColor(color).DotweenCoverRange(time);
        this.hideOnRoleDie = hideOnRoleDie;
        transform.forward = -role.Animator.transform.forward;
    }

    private AttackIndeicate_Sector SetRole(RoleController roleController)
    {
        role = roleController;
        return this;
    }

    public AttackIndeicate_Sector SetEnable(bool isEnable)
    {
        gameObject.SetActive(isEnable);
        return this;
    }

    public AttackIndeicate_Sector SetColor(Color color)
    {
        meshRenderer.material.SetColor("_BaseColor",new Color(color.r,color.g,color.b,0.2f));
        meshRenderer.material.SetColor("_ColorColor",new Color(color.r,color.g,color.b,0.5f));
        return this;
    }
    
    public AttackIndeicate_Sector SetSize(float size)
    {
        float scale = size * 2f;
        meshRenderer.transform.localScale = Vector3.one * scale;
        return this;
    }

    public AttackIndeicate_Sector SetAngle(float ang)
    {
        meshRenderer.material.SetFloat(Angle,ang);
        return this;
    }

    public AttackIndeicate_Sector SetPosition(Vector3 pos)
    {
        transform.position = pos+new Vector3(0,0.1f,0);
        return this;
    }

    Coroutine coroutine;

    public AttackIndeicate_Sector SetTime(float time)
    {
        coroutine = StartCoroutine(setTime(time));
        meshRenderer.material.SetFloat("_Alpha",0);
        meshRenderer.material.DOFloat(0.3f,"_Alpha",time).SetEase(Ease.Linear);
        return this;
    }

    public AttackIndeicate_Sector DotweenCoverRange(float time)
    {
        meshRenderer.material = GameObject.Instantiate(meshRenderer.material);
        meshRenderer.material.SetFloat(CoverRange, 0);
        DOTween.To(() => meshRenderer.material.GetFloat(CoverRange), value =>
        {
            meshRenderer.material.SetFloat(CoverRange, value);
        }, 1, time).SetEase(Ease.Linear);
        return this;
    }

    IEnumerator setTime(float time)
    {
        yield return new WaitForSeconds(time);
        IndicatorManager.Inst.Recover(this);
        coroutine = null;
    }
    public void Recover()
    {
        if (coroutine!=null)
        {
            StopCoroutine(coroutine);
        }

        IndicatorManager.Inst.Recover(this);
        coroutine = null;
    }
    
}
