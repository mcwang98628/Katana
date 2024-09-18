using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpwanPosType
{
    RoleCenter = 0,
    EnemyTarget = 1,
    RandomEnemy = 2,
    BodyNode = 3,
}
public class SpawnEffect : ItemEffect
{
    public GameObject SpwanObj;
    public SpwanPosType SpwanPosType;
    public Vector3 Offset = Vector3.zero;
    public Vector3 Direction = Vector3.zero;
    public bool IsFollowRole = false;

    public bool enemyFollow=false;
    GameObject objInstance;
    //public int Damage;
    public SpawnEffect(GameObject spwanObj, SpwanPosType spwanPosType, Vector3 offset, Vector3 direction, bool isFollowRole,bool ef)
    {
        //Damage = damage;
        Direction = direction;
        SpwanObj = spwanObj;
        SpwanPosType = spwanPosType;
        Offset = offset;
        IsFollowRole = isFollowRole;
        enemyFollow = ef;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        if (value.HasValue)
        {
            if (!string.IsNullOrEmpty(value.Value.TargetId))
            {
                Trigger(value.Value.TargetId);
            }
            else if (value.Value.TargetPosition != Vector3.zero)
            {
                Trigger(value.Value.TargetPosition + Offset,Vector3.forward);
            } 
        }
        else
        {
            Trigger("");
        }
    }

    void Trigger(string targetId)
    {
        var fireFly = SpwanObj.GetComponent<ProjectileTrace_FireFly>();
        if (fireFly != null && 
            (BattleManager.Inst.BattleObject.ContainsKey(BattleObjectType.FireFly) && BattleManager.Inst.BattleObject[BattleObjectType.FireFly].Count >= fireFly.MaxCount))
        {
            return;
        } 
        Vector3 position = Vector3.zero;
        Vector3 forward = Vector3.forward;
        var followTarget = roleController;
        switch (SpwanPosType)
        {
            case SpwanPosType.RoleCenter:
                position = BattleManager.Inst.CurrentPlayer.Animator.transform.position;
                 if (IsFollowRole)//用于处理铠甲特效的位置偏移
                 {
                    position = roleController.GetComponent<RoleNode>().Body.transform.position;
                    position.y=0;
                 }
                position += BattleManager.Inst.CurrentPlayer.Animator.transform.right * Offset.x +
                                   BattleManager.Inst.CurrentPlayer.Animator.transform.up * Offset.y +
                                   BattleManager.Inst.CurrentPlayer.Animator.transform.forward * Offset.z;
                forward = BattleManager.Inst.CurrentPlayer.Animator.transform.right * Direction.x +
                   BattleManager.Inst.CurrentPlayer.Animator.transform.up * Direction.y +
                   BattleManager.Inst.CurrentPlayer.Animator.transform.forward * Direction.z;
                break;
            case SpwanPosType.EnemyTarget:
                RoleController enemy;
                if (BattleManager.Inst.EnemyTeam.ContainsKey(targetId))
                {
                    enemy = BattleManager.Inst.EnemyTeam[targetId];
                }
                else
                {
                    enemy = BattleTool.FindNearestEnemy(BattleManager.Inst.CurrentPlayer);
                }

                if (enemy != null)
                {
                    var enemyTransform = enemy.transform;
                    position = new Vector3(enemyTransform.position.x, 0, enemyTransform.position.z);
                    position += enemyTransform.right * Offset.x +
                                enemyTransform.up * Offset.y +
                                enemyTransform.forward * Offset.z;
                    forward = enemyTransform.forward;
                }

                followTarget = enemy;
                break;
            case SpwanPosType.RandomEnemy:
                RoleController randomEnemy=BattleTool.GetRandomEnemy(BattleManager.Inst.CurrentPlayer);              
                if (randomEnemy != null)
                {
                    var enemyTransform = randomEnemy.transform;
                    position = new Vector3(enemyTransform.position.x, 0, enemyTransform.position.z);
                    position += enemyTransform.right * Offset.x +
                                enemyTransform.up * Offset.y +
                                enemyTransform.forward * Offset.z;
                    forward = enemyTransform.forward;
                }

                followTarget = randomEnemy;
                break;
            default:
                break;
        }

        objInstance = GameObject.Instantiate(SpwanObj);
        if (SpwanPosType == SpwanPosType.BodyNode)
        {
            objInstance.transform.SetParent(roleController.roleNode.Body);
            objInstance.transform.localPosition = Vector3.zero;
        }
        else
        {
            objInstance.transform.position = position;
            objInstance.transform.forward = forward;
        }

        if (IsFollowRole)
        {
            objInstance.transform.SetParent(followTarget.GetComponent<RoleNode>().Body.transform);
            objInstance.transform.localPosition=new Vector3(objInstance.transform.localPosition.x,0,objInstance.transform.localPosition.z);
        }

        if (enemyFollow)
        {
            objInstance.transform.SetParent(followTarget.transform);
        }

        //如果是区域伤害物，需要初始化伤害来源
        if (objInstance.GetComponent<DmgBuffOnTouch>())
        {
            objInstance.GetComponent<DmgBuffOnTouch>().Init(roleController);
        }
        //TODO:对不同召唤物类型设置伤害


    }
    
    void Trigger(Vector3 position,Vector3 forward)
    {
        var fireFly = SpwanObj.GetComponent<ProjectileTrace_FireFly>();
        if (fireFly != null && 
            (BattleManager.Inst.BattleObject.ContainsKey(BattleObjectType.FireFly) && BattleManager.Inst.BattleObject[BattleObjectType.FireFly].Count >= fireFly.MaxCount))
        {
            return;
        } 

        objInstance = GameObject.Instantiate(SpwanObj);
        objInstance.transform.position = position;
        objInstance.transform.forward = forward;

        if (IsFollowRole)
        {
            objInstance.transform.SetParent(roleController.GetComponent<RoleNode>().Body.transform);
            objInstance.transform.localPosition=new Vector3(objInstance.transform.localPosition.x,0,objInstance.transform.localPosition.z);
        }

        


        //如果是区域伤害物，需要初始化伤害来源
        if (objInstance.GetComponent<DmgBuffOnTouch>())
        {
            objInstance.GetComponent<DmgBuffOnTouch>().Init(roleController);
        }
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    public override void Destroy(RoleItemController rpe)
    {
        base.Destroy(rpe);
        if (objInstance)
            GameObject.Destroy(objInstance);
    }
}
