using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//环绕物转换
public class SurroundObjectConversionEffect : ItemEffect
{
    public List<Surround_Obj> ConversionObj = new List<Surround_Obj>();
    private float attackPowerPercentage;
    private int AttackPower;
    public SurroundObjectConversionEffect(List<Surround_Obj> objs,int attackPower,float attackPowerPercentage)
    {
        ConversionObj = objs;
        this.attackPowerPercentage = attackPowerPercentage;
        AttackPower = attackPower;
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        
        AttackPower += (int) (roleController.OriginalAttackPower * attackPowerPercentage);
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        int objOldCount = roleController.roleSurroundController.SurroundObjs.Count;
        for (int i = 0; i < objOldCount; i++)
        {
            roleController.roleSurroundController.RemoveObj(roleController.roleSurroundController.SurroundObjs[0]);
        }

        for (int i = 0; i < objOldCount; i++)
        {
            Surround_Obj obj = ConversionObj[Random.Range(0, ConversionObj.Count)];
            Surround_Obj surroundObj = new Surround_Obj();
            surroundObj = GameObject.Instantiate(obj);
            surroundObj.SetDmgValue(AttackPower);
            roleController.roleSurroundController.AddObj(surroundObj);
        }
    }
}
