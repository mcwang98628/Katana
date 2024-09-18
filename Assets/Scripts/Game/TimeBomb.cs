using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBomb : MonoBehaviour
{
    public float WaitTime;
    public RoleController Target;
    public DmgBuffOnTouch Explosion;
    //攻击倍数
    public float DmgMul;
    float StartTime;
    private void Start()
    {
        StartTime = Time.time;
        StartCoroutine(ExpIE());
    }

    public IEnumerator ExpIE()
    {
        while(Time.time< StartTime + WaitTime )
        {
            if(Target.IsDie)
            {
                break; 
            }
            yield return null;
        }
        Exp();
    }
    public void Exp()
    {
        DmgBuffOnTouch exp = Instantiate(Explosion.gameObject,transform.position,Quaternion.identity).GetComponent<DmgBuffOnTouch>();
        exp.Init(BattleManager.Inst.CurrentPlayer, BattleManager.Inst.CurrentPlayer.roleAttack.AttackPower * DmgMul);

        List<RoleController> TargetList = BattleManager.Inst.CurrentPlayer.GetComponent<PlayerBoxerSkill>().EnemiesWithBomb;
        for (int i = 0; i < TargetList.Count; i++)
        {
            if(TargetList[i]==Target)
            {
                BattleManager.Inst.CurrentPlayer.GetComponent<PlayerBoxerSkill>().EnemiesWithBomb.RemoveAt(i);
            }
        }
        Destroy(gameObject);
    }


}
