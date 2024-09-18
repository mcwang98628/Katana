using System;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyController : RoleController
{
    public override bool IsAcceptInput { get => true; }
    public override bool IsCanMove => !IsFastMoving && !IsDmging && !IsDie && !IsDizziness;//&& !IsAttacking;

    [LabelText("死亡之后需要停止的粒子")]
    public List<ParticleSystem> ToBeDestroidPar;
    [FormerlySerializedAs("TimeType")] public RoleTeamType TeamType = RoleTeamType.Enemy;
    //public ParticleSystem[] ProtectedParticles;


    public override void Init()
    {
        
        if (isInit)
        {
            return;
        }
        base.Init();
        if (UniqueID <= 0)
        {
            UniqueID = 1;
        }
        var enemyInfo = DataManager.Inst.GetEnemyInfo(UniqueID);
        switch (enemyInfo.EnemyType)
        {
            case EnemyType.Lv1:
            case EnemyType.Lv2:
            case EnemyType.Lv3:
                TeamType = RoleTeamType.Enemy;
                break;
            case EnemyType.Elite:
                TeamType = RoleTeamType.EliteEnemy;
                break;
            case EnemyType.Boss:
                TeamType = RoleTeamType.Enemy_Boss;
                break;
        }
        this.roleTeamType = TeamType;
        roleHealth.Init(roleHealth.IniHp);
        BattleManager.Inst.RoleRegistered(this);
    }

    public override void OnDeadEvent()
    {
        base.OnDeadEvent();
        
        if (ToBeDestroidPar != null)
        for (int i=0;i<ToBeDestroidPar.Count;i++)
        {
            if(ToBeDestroidPar[i]!=null)
                ToBeDestroidPar[i].Stop();
        }

        StartCoroutine(waitUnRegistered());

        DropEquipment();
    }

    void DropEquipment()
    {
        if (TeamType == RoleTeamType.EliteEnemy || 
            TeamType == RoleTeamType.Enemy_Boss)
        {
            ResourcesManager.Inst.GetAsset<GameObject>("Assets/BundleAssets/Prefabs/EquipmentObject.prefab",
                delegate(GameObject prefab)
                {
                    int equipCount = Random.Range(2, 4);
                    if (BattleManager.Inst.RuntimeData.CurrentChapterId < 2)
                    {
                        //第一章只掉落攻击力和血量
                        var equipment = EquipmentTool.RandomScoreEquipmentById(1);
                        var equipGo = GameObject.Instantiate(prefab).GetComponent<EquipmentObject>();
                        equipGo.SetEquipment(equipment);
                        equipGo.transform.position = transform.position +
                                                     new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
                        
                        equipment = EquipmentTool.RandomScoreEquipmentById(2);
                        equipGo = GameObject.Instantiate(prefab).GetComponent<EquipmentObject>();
                        equipGo.SetEquipment(equipment);
                        equipGo.transform.position = transform.position +
                                                     new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));

                        equipment = EquipmentTool.RandomScoreEquipmentById(2);
                        equipGo = GameObject.Instantiate(prefab).GetComponent<EquipmentObject>();
                        equipGo.SetEquipment(equipment);
                        equipGo.transform.position = transform.position +
                                                     new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
                    }
                    else
                    {
                        for (int i = 0; i < equipCount; i++)
                        {
                            var equipment = EquipmentTool.RandomScoreEquipment();
                            var equipGo = GameObject.Instantiate(prefab).GetComponent<EquipmentObject>();
                            equipGo.SetEquipment(equipment);
                            equipGo.transform.position = transform.position +
                                                         new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
                        }
                    }
                });
            
        }
    }
    
    IEnumerator waitUnRegistered()
    {
        yield return null;
        // BattleManager.Inst.RoleUnRegistered(this);
    }
    public float GetRotateSpeed()
    {
        if ((roleMove as EnemyMove) == null)
            return -1;
        else
            return (roleMove as EnemyMove).RotateSpeed;
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
                DamageInfo dmg = new DamageInfo(
                    this.TemporaryId,
                    999999,
                    this,
                    new Vector3(transform.position.x,0,transform.position.z),
                    DmgType.Physical,
                    false,
                    false,
                    0,
                    0,
                    false,
                    this,
                    null,
                    null,
                    false
                );
                HpInjured(dmg);
                _exitFloorTimer = 0;
            }
        }
        else
        {
            _exitFloorTimer = 0;
        }
    }
    
     
}
