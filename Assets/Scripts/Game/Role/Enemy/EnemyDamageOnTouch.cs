using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageOnTouch : DmgBuffOnTouch
{
    public RoleController role;
    public int AttackPower=50;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayInit());
    }
    IEnumerator DelayInit()
    {
       yield return new WaitForSeconds(0.2f); 
       role = GetComponent<RoleController>();
       Init(role,AttackPower);
    }

    public void OnDead()
    {
        Destroy(this);
    }
}
