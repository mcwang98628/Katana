using System;
using System.Collections;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public bool FollowWithOffset;
    Vector3 StartOffset;
    private void Start()
    {
        StartCoroutine(StartFollowIE());
    }
    public IEnumerator StartFollowIE()
    {
        yield return null;
        if (BattleManager.Inst == null || BattleManager.Inst.CurrentPlayer == null)
        {

        }
        else
        {
            StartOffset = transform.position - BattleManager.Inst.CurrentPlayer.transform.position;
        }

        while (true)
        {
            if (BattleManager.Inst == null || BattleManager.Inst.CurrentPlayer == null)
            {

            }
            else
            {
                var pos = BattleManager.Inst.CurrentPlayer.transform.position;
                if (FollowWithOffset)
                {
                    transform.position = StartOffset + new Vector3(pos.x, pos.y, pos.z);
                    transform.forward = BattleManager.Inst.CurrentPlayer.Animator.transform.forward;


                }
                else
                {
                    transform.position = new Vector3(pos.x,transform.position.y,pos.z);
                }
            }
            yield return null;
        }
    }
}
