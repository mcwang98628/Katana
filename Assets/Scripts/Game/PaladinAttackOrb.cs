using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinAttackOrb : Projectile
{
    //当前等级
    public int CurrentLevel;
    public GameObject DestroyExpSmall;
    public GameObject DestroyExpLarge;
    public float SmallExpMul=1f;
    public float LargeExpMul=1.5f;
    //bool EnemyIsNear;
    public MoveToRandomEnemy _move;
    bool IsExped;
    //public GameObject DestroyExplosion;
    public override void OtherEffect(RoleController Target)
    {
        Exp();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        if(!IsExped)
        {
            Exp();
        }
    }

    public void Exp()
    {
        //获取当前的爆炸物,三级大爆炸
        GameObject CurrentExp = CurrentLevel <= 2 ? DestroyExpSmall : DestroyExpLarge;
        float ExpMul = CurrentLevel <=2 ? SmallExpMul : LargeExpMul;
        //释放爆炸
        GameObject Exp = Instantiate(CurrentExp, transform.position, Quaternion.identity);
        Exp.GetComponent<DmgBuffOnTouch>().Init(BattleManager.Inst.CurrentPlayer, BattleManager.Inst.CurrentPlayer.roleAttack.AttackPower * ExpMul);
        IsExped = true;
        Destroy(gameObject);
    }
    protected override void FixedUpdate()
    {
        if (CurrentLevel == 3)
        {
            if (BattleTool.GetEnemysInDistance(BattleManager.Inst.CurrentPlayer, 999).Count == 0)
            {
                base.FixedUpdate();
            }
            else
            {
                _move.enabled = true;
            }
        }
        else
        {
            base.FixedUpdate();
        }
    }
}
