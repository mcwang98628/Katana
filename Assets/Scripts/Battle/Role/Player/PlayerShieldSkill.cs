using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShieldSkill : PlayerSkill
{
    public GameObject ChargeWaveObj;
    public float ChargeWaveDmgMul;
    public GameObject AttackWaveObj;
    public Transform AttackWavePos;
    public float AttackWaveObjMul;
    //三级大。
    public DmgBuffOnTouch UltimateLevel1;
    public DmgBuffOnTouch UltimateLevel2;
    public DmgBuffOnTouch UltimateLevel3;
    public Transform UltimateStartPos;
    public float UltimateMul;

    protected override void AnimEvent(GameObject go, string eventName)
    {
        base.AnimEvent(go, eventName);
        if(go!=roleController.Animator.gameObject)
        {
            return;
        }
        if (roleColorLevel >= 2)
        {
        //二级以上往上蓄力会发送一道冲击波
            if (eventName == "FinishCharge")
            {
                GameObject _chargeWaveObj = Instantiate(ChargeWaveObj, transform.position + Vector3.up * 0.2f, Quaternion.identity);
                _chargeWaveObj.GetComponent<DmgBuffOnTouch>().Init(roleController, roleController.roleAttack.AttackPower * ChargeWaveDmgMul);
            }
            //二级往上普攻会发送一道冲击波
            if(eventName == "NormalAttack")
            {
                GameObject _normalAttackObj = Instantiate(AttackWaveObj,AttackWavePos.position,AttackWavePos.rotation);
                _normalAttackObj.GetComponentInChildren<DmgBuffOnTouch>().Init(roleController,roleController.roleAttack.AttackPower * AttackWaveObjMul);
            }

          

        }
        if (eventName == "UltimateDmg")
        {
            if (roleColorLevel == 1)
            {
                GameObject _ultimateDmgObj = Instantiate(UltimateLevel1.gameObject, UltimateStartPos.position, UltimateStartPos.rotation);
                _ultimateDmgObj.GetComponent<DmgBuffOnTouch>().Init(roleController, roleController.roleAttack.AttackPower * UltimateMul);
            }
            if (roleColorLevel == 2)
            {
                GameObject _ultimateDmgObj = Instantiate(UltimateLevel2.gameObject, UltimateStartPos.position, UltimateStartPos.rotation);
                _ultimateDmgObj.GetComponent<DmgBuffOnTouch>().Init(roleController, roleController.roleAttack.AttackPower * UltimateMul);
            }
            //三级大招会发波
            if (roleColorLevel == 3)
            {
                GameObject _chargeWaveObj = Instantiate(ChargeWaveObj, transform.position + Vector3.up * 0.2f, Quaternion.identity);
                _chargeWaveObj.GetComponent<DmgBuffOnTouch>().Init(roleController, roleController.roleAttack.AttackPower * ChargeWaveDmgMul);
                GameObject _ultimateDmgObj = Instantiate(UltimateLevel3.gameObject, UltimateStartPos.position, UltimateStartPos.rotation);
                _ultimateDmgObj.GetComponent<DmgBuffOnTouch>().Init(roleController, roleController.roleAttack.AttackPower * UltimateMul);
            }
        }



    }
}
