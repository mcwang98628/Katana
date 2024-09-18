using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    [ShowInInspector]
    public Transform Target => _target;
    private Transform _target;
    private RoleController targetRole;

    public MeshRenderer meshRenderer;

    public void SetColor(Color targetColor)
    {
        meshRenderer.sharedMaterial.SetColor("_BaseColor", targetColor);
    }
    public void SetTarget(Transform target)
    {
        if (target == null)
        {
            return;
        }
        _target = target;
        targetRole = GetComponent<RoleController>();
        CapsuleCollider cc = _target.GetComponent<CapsuleCollider>();
        if (cc != null)
        {
            var v3 = cc.radius * _target.localScale;
            transform.localScale = Vector3.one + v3;
        }
        else
        {
            transform.localScale = Vector3.one;
        }
        gameObject.SetActive(_target != null);
        Update();
    }

    //public void SetScale(Vector3 scale)
    //{
    //    //gameObject.transform.localScale = scale;
    //}

    private void Update()
    {
        if (_target == null)
        {
            gameObject.SetActive(false);
            return;
        }
        if (targetRole != null)
        {
            if (targetRole.IsDie)
            {
                gameObject.SetActive(false);
            }
        }

        transform.position = _target.position; //new Vector3(_target.position.x,0,_target.position.z);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
