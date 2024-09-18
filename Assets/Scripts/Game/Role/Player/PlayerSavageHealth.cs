using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerSavageHealth : RoleHealth
{
    [SerializeField]
    private PlayerSavageAttack _savageAttack;
    [SerializeField][LabelText("受伤增加能量")]
    private int injuredPower;
    public override void Injured(DamageInfo dmg)
    {
        if (_savageAttack.ArrestEnemy!=null && !_savageAttack.ArrestEnemy.IsDie)
        {
            var dir = dmg.AttackPoint - roleController.transform.position;
            float ang = Vector3.Angle(roleController.Animator.transform.forward, dir);
            if (ang < 20)
            {
                dmg.DmgValue /= 2;   
                _savageAttack.ArrestEnemy.HpInjured(dmg);
            }
        }

        if (dmg.DmgValue>0)
        {
            roleController.AddSkillPower(injuredPower);
        }
        base.Injured(dmg);
    }
}
