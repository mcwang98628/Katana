using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmmitParticleOnDestroy : MonoBehaviour
{
    public int BreakDamage=50;
    public GameObject Particles;
    private void OnDestroy()
    {
        var role = BattleTool.FindNearestEnemy(transform);
        if(role!=null)
        {
            if(!role.IsDie)
            role.HpInjured(new DamageInfo(role.TemporaryId,BreakDamage,BattleManager.Inst.CurrentPlayer,transform.position));
        }
        Instantiate(Particles,transform.position,Quaternion.identity);
    }
}
