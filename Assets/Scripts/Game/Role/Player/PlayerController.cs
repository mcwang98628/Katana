using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerController : RoleController
{
    [LabelText("摇杆死区")] [GUIColor(1, 0, 0)]
    public float JoyDeadZone;

    [BoxGroup("Player 基础组件")] public PlayerInputAction InputAction;

    [HideInInspector] public bool AOECrit;
    // [HideInInspector]
    // public PlayerExperience playerExperience;

    //使用中的技能计数器，开始技能+1 结束技能需-1   可移动
    public int CanMoveSkillUsingCount;

    //使用中的技能计数器，开始技能+1 结束技能需-1   不可移动
    public int CantMoveSkillUsingCount;

    // //外围英雄等级
    // public int GlobalHeroLevel { get; protected set; }
    // public int LocalHeroLevel { 
    //     get { 
    //         return playerExperience.CurrentLevel;
    //     } 
    // }

    public int ColorLevel => ArchiveManager.Inst.ArchiveData.GlobalData.HeroUpgradeDatas[UniqueID].ColorLevel;

    public float AddGoldMagnification => 1f + GetAttributeBonusValue(AttributeType.AddGoldMagnification);

    public override bool IsCanMove => !IsDie && !IsDizziness && !IsRolling && !isHiting && (CantMoveSkillUsingCount == 0) && !isOnlyRoll && !IsAttacking;
    public override bool IsCanRoll => base.IsCanRoll && CanMoveSkillUsingCount == 0 && CantMoveSkillUsingCount == 0 && ((PlayerRoll) roleRoll).CoolPercent >= 1;
    public override bool IsCanAttack => base.IsCanAttack && CanMoveSkillUsingCount == 0 && CantMoveSkillUsingCount == 0;

    private bool isOnlyRoll;


    protected override void Awake()
    {
        base.Awake();
        if (InputAction == null)
        {
            InputAction = gameObject.AddComponent<PlayerInputAction>();
        }

        EventManager.Inst.AddEvent(EventName.OnRoleDead, OnEnemyDead);
        EventManager.Inst.AddEvent(EventName.OnRoleInjured, OnPlayerInjured);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.Inst.RemoveEvent(EventName.OnRoleDead, OnEnemyDead);
        EventManager.Inst.RemoveEvent(EventName.OnRoleInjured, OnPlayerInjured);
    }

    private void OnPlayerInjured(string arg1, object arg2)
    {
        RoleInjuredInfo data = (RoleInjuredInfo) arg2;
        if (data.RoleId != TemporaryId)
        {
            return;
        }
        //BattleManager.Inst.AddFortunesValue(-1);
    }

    private void OnEnemyDead(string arg1, object arg2)
    {
        RoleDeadEventData data = (RoleDeadEventData) arg2;
        if (data.DeadRole.TemporaryId == TemporaryId)
        {
            return;
        }
        //BattleManager.Inst.AddFortunesValue(1);
    }

    public override void Init()
    {
        if (isInit)
        {
            return;
        }

        base.Init();
        roleTeamType = RoleTeamType.Player;
        BattleManager.Inst.RoleRegistered(this);
    }


    protected override IEnumerator onDead()
    {
        Rigidbody.velocity = Vector3.zero;
        yield return null;
        StopFastMove();
    }

    protected override void OnSelfAnimEvent(string eventName)
    {
        base.OnSelfAnimEvent(eventName);
        switch (eventName)
        {
            case AnimatorEventName.StartGod:
                SetDodge(true);
                break;
            case AnimatorEventName.EndGod:
                SetDodge(false);
                break;
            case "TestHitStart":
                isHiting = true;
                break;
            case "TestHitOver":
                isHiting = false;
                break;
            case AnimatorEventName.CanMoveSkillStart:
                CanMoveSkillUsingCount++;
                break;
            case AnimatorEventName.CanMoveSkillEnd:
                CanMoveSkillUsingCount--;
                break;
            case AnimatorEventName.CantMoveSkillStart:
                CantMoveSkillUsingCount++;
                break;
            case AnimatorEventName.CantMoveSkillEnd:
                CantMoveSkillUsingCount--;
                break;
            case AnimatorEventName.OnlyRollStart:
                isOnlyRoll = true;
                break;
            case AnimatorEventName.OnlyRollEnd:
                isOnlyRoll = false;
                break;
        }
    }

    private void ResetPlayerPosition()
    {
        Vector3 targetPos = Vector3.zero;
        if (BattleManager.Inst.CurrentRoom is FightRoom fightRoom)
        {
            float minDis = 99999;
            for (int i = 0; i < fightRoom.Enemypoints.childCount; i++)
            {
                var enemyPos = fightRoom.Enemypoints.GetChild(i).position;
                float dis = (enemyPos - transform.position).magnitude;
                if (minDis > dis)
                {
                    minDis = dis;
                    targetPos = enemyPos;
                }
            }
        }
        else
        {
            if (BattleManager.Inst.CurrentRoom == null)
            {
                targetPos = GameObject.Find("Floors").transform.position;
            }
            else
            {
                targetPos = BattleManager.Inst.CurrentRoom.RoomEnter.position;
            }
        }

        BattleManager.Inst.CurrentCamera.StartCameraLerpMove();
        transform.position = targetPos;
    }

    protected override void Update()
    {
        base.Update();
        if (!BattleManager.Inst.GameIsRuning)
            return;

        if (_isExitFloor)
        {
            _exitFloorTimer += Time.deltaTime;
            if (_exitFloorTimer > 2f)
            {
                ResetPlayerPosition();
                _exitFloorTimer = 0;
            }
        }
        else
        {
            _exitFloorTimer = 0;
        }
    }
}