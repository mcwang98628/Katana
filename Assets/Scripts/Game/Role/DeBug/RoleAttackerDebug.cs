using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class RoleAttackerDebug : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer AttackIndicatorMeshRenderer;
    [LabelText("Attack 类型")]
    private AttackType AttackType = AttackType.Sector;
    [LabelText("Attack 距离/半径")]
    private float AttackRadius;
    [ShowIf("AttackType",AttackType.Sector)] [LabelText("Attack 角度")]
    private float AttackAngle;

    private static readonly int Angle = Shader.PropertyToID("_Angle");

    private RoleController roleController;
    private void Awake()
    {
        roleController = GetComponent<RoleController>();
    }

    private void Update()
    {
        if (!isEnable)
        {
            return;
        }

        if (roleController.IsDie)
        {
            SetEnable(false);
        }

        AttackIndicatorMeshRenderer.transform.forward = roleController.Animator.transform.forward;
        switch (AttackType)
        {
            case AttackType.Round:
                AttackIndicatorMeshRenderer.material.SetFloat(Angle,360);
                break;
            case AttackType.Sector:
                AttackIndicatorMeshRenderer.material.SetFloat(Angle,AttackAngle);
                break;
        }
        float scale = AttackRadius * 1f;
        AttackIndicatorMeshRenderer.transform.localScale = Vector3.one * scale;
    }

    private bool isEnable;
    public void SetEnable(bool enable)
    {
        isEnable = enable;
        AttackRadius = 0;
        AttackIndicatorMeshRenderer.gameObject.SetActive(isEnable);
    }

    public void SetValue(AttackType attackType,float attackRadius,float attackAngle)
    {
        AttackType = attackType;
        AttackRadius = attackRadius;
        AttackAngle = attackAngle;
    }
}
