using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorail_FlameThrower : MonoBehaviour
{
    public int DamageValue;
    RoleController player;
    Bounds bounds;
    Collider collider;
    float timer;
    void Start()
    {
        player = BattleManager.Inst.CurrentPlayer;
        collider = GetComponent<Collider>();
        timer = Time.time;
    }

    /*
    private void Update()
    {
        if (Time.time - timer > 0.5f)
        {
            Vector3 pos = player.transform.position;
            pos.y = transform.position.y;
            if (bounds.Contains(pos))
            {
                DamageInfo damage = new DamageInfo(DamageValue);
                player.HpInjured(damage);
                timer = Time.time;
            }
        }
    }
    */
    
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }
        if(timer+0.3f> Time.time)
            return;
        timer = Time.time;
        
        DamageInfo damage = new DamageInfo(player.TemporaryId,DamageValue,player,player.Animator.transform.position+player.Animator.transform.forward,DmgType.Physical,true,true,0.12f,20,false,null);
        damage.DmgType = DmgType.Physical;
        player.HpInjured(damage);
        
        if (player.IsRolling)
        {
            collider.isTrigger = true;
            StartCoroutine(ResetCollider());
        }

        // Vector3 v3 = player.transform.position - transform.position;
        // float angle = Vector3.Angle(v3, transform.right);
        // Vector3 moveDir = transform.right;
        // if (angle > 90)
        // {
        //     moveDir = -transform.right;
        // }
        // player.FastMove(0.1f, 20, moveDir, null);
    }

    private void OnCollisionStay(Collision other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }
        if(timer+0.3f> Time.time)
            return;
        timer = Time.time;

        DamageInfo damage = new DamageInfo(player.TemporaryId,DamageValue,player,player.Animator.transform.position+player.Animator.transform.forward,DmgType.Physical,true,true,0.1f,20,false,null);
        damage.DmgType = DmgType.Physical;
        player.HpInjured(damage);

        if (player.IsRolling)
        {
            collider.isTrigger = true;
            StartCoroutine(ResetCollider());
        }
    }

    IEnumerator ResetCollider()
    {
        yield return new WaitForSeconds(0.5f);
        collider.isTrigger = false;
    }
    
    


}
