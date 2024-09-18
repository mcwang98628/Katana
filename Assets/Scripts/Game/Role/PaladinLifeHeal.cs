using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinLifeHeal : MonoBehaviour
{

    public RoleHealth _health;
    public RoleController _controller;
    public float Interval = 1f;
    public float HealCount = 50f;
    public float Duration = 8f;
    public float Round = 5f;
    public void Start()
    {
        StartCoroutine(Heal());
        Destroy(gameObject,Duration);
    }
    public IEnumerator Heal()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, BattleManager.Inst.CurrentPlayer.transform.position) < Round)
            {
                _health.Treatment(new TreatmentData(HealCount, _controller.TemporaryId));
            }
            yield return new WaitForSeconds(Interval);
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
