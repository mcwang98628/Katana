using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerSavagePower : RolePower
{
    [SerializeField][LabelText("脱离战斗多久 掉能量")] [BoxGroup("野蛮人")]
    private float LosePowerTime;
    [SerializeField][LabelText("掉能量 速度 value/s")] [BoxGroup("野蛮人")]
    private float LosePowerSpeed;
    [SerializeField][LabelText("满能量增加攻击力的比例 0-1f")] [BoxGroup("野蛮人")]
    private float MaxPowerAddAttackPower;

    private float lastAddSkillPowerTime;
    public override void AddSkillPower(float value)
    {
        base.AddSkillPower(value);
        if (value>0)
        {
            lastAddSkillPowerTime = Time.time;   
        }
    }

    AttributeBonus attributeBonus;
    public override void Init()
    {
        base.Init();
        attributeBonus = new AttributeBonus();
        attributeBonus.Type = AttributeType.AttackPower;
        attributeBonus.Value = 0;
        RoleController.AddAttributeBonus(attributeBonus);
    }

    
    protected override void OnDestroy()
    {
        base.OnDestroy();
        RoleController.RemoveAttributeBonus(attributeBonus);
        
    }

    protected override void Update()
    {
        base.Update();
        if (Time.time-lastAddSkillPowerTime > LosePowerTime)
        {
            base.AddSkillPower(-LosePowerSpeed*Time.deltaTime);
        }

        attributeBonus.Value = MaxPowerAddAttackPower * (CurrentSkillPower/MaxSkillPower) * RoleController.OriginalAttackPower;
    }
}
