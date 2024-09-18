using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

//所有交互体的基类，一般不直接使用
public abstract class InteractObj : MonoBehaviour
{

    public bool CanInteract => canInteract;
    protected bool canInteract = true;

    private void Start()
    {
        Init();
    }


    protected virtual void Init()
    {
        InteractManager.Inst.AddInteractObj(this);
        canInteract = true;
    }

    protected virtual void OnDestroy()
    {
        InteractManager.Inst.RemoveInteractObj(this);
    }
    
    public virtual void InteractStart()
    {
        if (!canInteract)
        {
            return;
        }
        canInteract = false;
    }
    public virtual void InteractEnd()
    {
        
    }
    public virtual void SelectStart()
    {
    }

    public virtual void SelectEnd()
    {
    }
    protected IEnumerator DelaySetCanIntact(bool value, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        SetCanInteract(value);
    }
    public void SetCanInteract(bool value)
    {
        canInteract = value;
    }
    protected void DestroyInteractObj()
    {
        transform.DOScale(Vector3.zero,0.3f);
        Destroy(gameObject,0.3f);
    }

}
