using UnityEngine;

public class SurroundEffect : ItemEffect
{
    private Surround_Obj obj;
    private int AttackPower;
    private float attackPowerPercentage;
    public SurroundEffect(Surround_Obj obj,int attackPower,float attackPowerPercentage)
    {
        this.obj = obj;
        AttackPower = attackPower;
        this.attackPowerPercentage = attackPowerPercentage;
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        AttackPower += (int) (roleController.OriginalAttackPower * attackPowerPercentage);
    }

    private Surround_Obj surroundObj;

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        surroundObj = new Surround_Obj();
        surroundObj = GameObject.Instantiate(this.obj); 
        surroundObj.SetDmgValue(AttackPower);
        roleController.roleSurroundController.AddObj(surroundObj);
    }
 

    public override void Destroy(RoleItemController rpe)
    {
        if (surroundObj!=null)
        {
            roleController.roleSurroundController.RemoveObj(surroundObj);
        }
        base.Destroy(rpe);
    }
}
