using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class EnemySecondStage : MonoBehaviour
{
    public Animator SecondAnimator;
    public float SecondMoveSpeed;
    public int SecondAttack;
    public GameObject SpwanOnChange;
    public enum TriggerType
    {
        HealthBelow,
        ShieldBreak,
        PlayerInRange
    }
    public TriggerType Trigger;

    public float HpPercent;
    public float PlayerRange;
    EnemyShield shield;
    RoleHealth roleHealth;
    RoleController roleController;
    RoleMove roleMove;

    private void Start()
    {
        shield=GetComponent<EnemyShield>();
        roleHealth = GetComponent<RoleHealth>();
        roleController = GetComponent<RoleController>();
        roleMove = GetComponent<RoleMove>();
    }
    private void Update()
    {
        switch (Trigger)
        {
            case TriggerType.HealthBelow:
                if (roleHealth.CurrentHp / roleHealth.MaxHp < HpPercent)
                    ChangeStage();
                break;
            case TriggerType.ShieldBreak:
                if (shield==null ||shield.UseSheild==false)
                    ChangeStage();
                break;
            case TriggerType.PlayerInRange:
                if (!BattleManager.Inst.CurrentPlayer)
                    return;
                if (Vector3.Distance(transform.position,BattleManager.Inst.CurrentPlayer.transform.position) < PlayerRange)
                {
                    ChangeStage();
                }
                break;
            default:
                break;
        }
    }
    void ChangeStage()
    {
        Destroy(this);
    }
}
