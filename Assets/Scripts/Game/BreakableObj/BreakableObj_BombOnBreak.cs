using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class BreakableObj_BombOnBreak : BreakableObj
{
    public ThrowProjectile Bomb;
    public GameObject Explosion;
    public int DamageValue;

    public override void BreakObj()
    {
        base.BreakObj();
        IsBroken = true;
        SpawnObj();
        SwitchModel();
        PlayPartical();
        PlaySound();
        BreakableObjManager.Inst.PlayFeedBack();
        ThrowBomb();
    }
    void ThrowBomb()
    {
        ThrowProjectile instance = Instantiate(Bomb, transform.position, Quaternion.identity);
        instance.Init(transform.position, 1.2f, 5.0f, Explosion, 2, DamageValue, BattleManager.Inst.CurrentPlayer);
    }
}
