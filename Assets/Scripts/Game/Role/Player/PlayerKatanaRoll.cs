using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKatanaRoll : PlayerRoll
{
    // public bool IsCanDash
    // {
    //     get
    //     {
    //         float value2 = roleController.MaxMoveSpeed - roleController.MinMoveSpeed;
    //         float value = value2 - (roleController.MaxMoveSpeed - roleController.CurrentMoveSpeed);
    //         return (roleController.IsMoving && value / value2 >= 0.75f) || ((PlayerKatanaAttack)roleController.roleAttack).IsAccumulateComplete;
    //     }
    // }
    
    // public override void InputRoll(Vector2 v2)
    // {
    //     if (!roleController.IsCanRoll)
    //     {
    //         return;
    //     }
    //
    //     
    //     // if (IsCanDash)
    //     // {
    //     //      //自瞄距离
    //     //      float dis = 4f;
    //     //      //自瞄单边角度
    //     //      float ang = 30f;
    //     //      Vector3 targetDir = roleController.Animator.transform.forward;
    //     //      float minEnemyAng = 999;
    //     //      foreach (var controller in BattleManager.Inst.EnemyTeam)
    //     //      {
    //     //          var enemyDir = controller.Value.transform.position - roleController.transform.position;
    //     //          if (dis < enemyDir.magnitude)
    //     //          {
    //     //              continue;
    //     //          }
    //     //
    //     //          float enemyAng = Vector3.Angle(new Vector3(v2.x, 0, v2.y), enemyDir);
    //     //          if (enemyAng>ang)
    //     //          {
    //     //              continue;
    //     //          }
    //     //
    //     //          if (minEnemyAng>enemyAng)
    //     //          {
    //     //              minEnemyAng = enemyAng;
    //     //              targetDir = enemyDir;
    //     //          }
    //     //      }
    //     //      
    //     //      ((PlayerKatanaAttack)roleController.roleAttack).StartRunAccumulateAttack(targetDir);
    //     // }
    //     // else
    //     {
    //         base.InputRoll(v2);
    //     }
    // }

    // private bool isCanDistributeEvent = false;
    // protected override void Update()
    // {
    //     base.Update();
    //     if (IsCanDash && isCanDistributeEvent)
    //     {
    //         isCanDistributeEvent = false;
    //         EventManager.Inst.DistributeEvent(EventName.KatanaAccumulateEnd);
    //     }
    //     
    // }

    // protected override void Awake()
    // {
    //     base.Awake();
    //     // EventManager.Inst.AddEvent(EventName.JoyStatus,OnJoyStatus);
    //     // EventManager.Inst.AddEvent(EventName.KatanaAccumulateEnd,KatanaAccumulateEnd);
    //     // EventManager.Inst.AddEvent(EventName.KatanaAccumulateAttack,KatanaAccumulateAttack);
    //     // EventManager.Inst.AddEvent(EventName.OnRoleInjured,OnRoleInjured);
    // }
    //
    // protected override void OnDestroy()
    // {
    //     base.OnDestroy();
    //     // EventManager.Inst.RemoveEvent(EventName.JoyStatus,OnJoyStatus);
    //     // EventManager.Inst.RemoveEvent(EventName.KatanaAccumulateEnd,KatanaAccumulateEnd);
    //     // EventManager.Inst.RemoveEvent(EventName.KatanaAccumulateAttack,KatanaAccumulateAttack);
    //     // EventManager.Inst.RemoveEvent(EventName.OnRoleInjured,OnRoleInjured);
    // }
    //
    // private void OnRoleInjured(string arg1, object arg2)
    // {
    //     RoleInjuredInfo info = (RoleInjuredInfo) arg2;
    //     if (info.RoleId != BattleManager.Inst.CurrentPlayer.TemporaryId)
    //     {
    //         return;
    //     }
    //
    //     if (info.Dmg.Interruption)
    //     {
    //         // isCanDistributeEvent = true;
    //     }
    // }

    // private void KatanaAccumulateAttack(string arg1, object arg2)
    // {
    //     // isCanDistributeEvent = true;
    // }
    //
    // private void KatanaAccumulateEnd(string arg1, object arg2)
    // {
    //     // isCanDistributeEvent = false;
    // }

    // private void OnJoyStatus(string arg1, object arg2)
    // {
    //     // JoyStatusData data = (JoyStatusData) arg2;
    //     // if (data.JoyStatus == UIJoyStatus.OnDragEnd
    //     //     || data.JoyStatus == UIJoyStatus.OnHoldDragEnd)
    //     // {
    //     //     if (!IsCanDash)
    //     //     {
    //     //         return;
    //     //     }
    //     //
    //     //     Vector3 dir;
    //     //     if (roleController.EnemyTarget==null)
    //     //     {
    //     //         dir = roleController.Animator.transform.forward;
    //     //     }
    //     //     else
    //     //     {
    //     //         dir = roleController.EnemyTarget.transform.position - roleController.transform.position;
    //     //     }
    //     //     ((PlayerKatanaAttack)roleController.roleAttack).StartRunAccumulateAttack(dir);
    //     // }
    //     // else if (data.JoyStatus == UIJoyStatus.Idle)
    //     // {
    //     //     if (!roleController.IsRolling && !roleController.IsAttacking && !roleController.IsMoving)
    //     //     {
    //     //         isCanDistributeEvent = true;   
    //     //     }
    //     // }
    // }
}
