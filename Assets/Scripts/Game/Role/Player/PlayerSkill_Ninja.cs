using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerSkill_Ninja : PlayerSkill
{
    private RoleController enemyTarget;
    private bool IsAutoAttack = false;
    public BuffLifeCycle BuffLifeCycle;
    public BuffScriptableObject MainBuff;
    public BuffScriptableObject AdditionalBuff;
    void Blink()
    {
        enemyTarget = BattleTool.FindNearestEnemy(roleController);
        if(!enemyTarget)
            return;
        if(Vector3.Distance(enemyTarget.transform.position,roleController.transform.position)<2)
            return;
        
        Vector3 dir = (enemyTarget.transform.position - roleController.transform.position).normalized;
        roleController.Animator.transform.forward = dir;
        roleController.Animator.SetTrigger("Blink");
        BattleManager.Inst.CurrentCamera.PosLerp = 0.05f;
        StartCoroutine(BlinkEnd((0.15f)));
    }

    IEnumerator BlinkEnd(float time)
    {
        yield return new WaitForSeconds(time);
        Vector3 dir = (enemyTarget.transform.position - roleController.transform.position).normalized;

        float hitDisFwd=9, hitDisBwd=9;
        RaycastHit hit;
        Ray ray = new Ray(enemyTarget.transform.position+Vector3.up*0.2f,-dir);
        if(Physics.Raycast (ray,out hit,3))
        {
            hitDisFwd = Vector3.Distance(hit.point,enemyTarget.transform.position);
        }
        ray = new Ray(enemyTarget.transform.position+Vector3.up*0.2f,dir);
        if(Physics .Raycast (ray,out hit,3))
        {
            hitDisBwd = Vector3.Distance(hit.point,enemyTarget.transform.position);
        }
        if (hitDisFwd > hitDisBwd)
        {
            dir = -dir;
        }

        BattleManager.Inst.CurrentCamera.StartCameraLerpMove();
        roleController.transform.position = enemyTarget.transform.position + dir*0.5f;
        BattleManager.Inst.CurrentCamera.PosLerp = 1f;
    }

    private void FixedUpdate()
    {
        if(IsAutoAttack)
            roleController.InputAttack();
    }

    void CrazyMode(float time)
    {
        StartCoroutine(AutoAttack(time));
        RoleBuff buff = DataManager.Inst.ParsingBuff(MainBuff,BuffLifeCycle);
        roleController.roleBuffController.AddBuff(buff,roleController);
        if (roleColorLevel >= 3)
        {
            RoleBuff buff2 = DataManager.Inst.ParsingBuff(AdditionalBuff,BuffLifeCycle);
            roleController.roleBuffController.AddBuff(buff2,roleController);
        }
    }

    IEnumerator AutoAttack(float time)
    {
        IsAutoAttack = true;
        yield return new WaitForSecondsRealtime(time);
        IsAutoAttack = false;
        Debug.LogError("停止攻击");
    }

    protected override void AnimEvent(GameObject go, string eventName)
    {
        if (go != roleController.Animator.gameObject)
        {
            return;
        }

        if (eventName.Contains("Katana_BlinkNearEnemy"))
        {
            if(roleColorLevel>=2)
                Blink();
                
        }

        if (eventName.Contains("Katana_Crazy_"))
        {  
            eventName = eventName.Replace("Katana_Crazy_", "");
            float time = float.Parse(eventName);
            CrazyMode(time);
        }

    }
}

