using System;
using UnityEngine;

public class FullHpEffect:ItemEffect
{
    private float timer;
    private float cd;
    private GameObject SpwanObj;
    private GameObject insSwpanObj;
    
    private Surround_Obj surroundObj;
    private Surround_Obj insSurround;
    
    private Vector3 Offset = Vector3.zero;
    private Vector3 Direction = Vector3.zero;
    private bool IsFollowRole = false;

    public FullHpEffect(float cd, GameObject spwanObj, Surround_Obj surroundObj, Vector3 offset, Vector3 direction, bool isFollowRole)
    {
        this.cd = cd;
        SpwanObj = spwanObj;
        this.surroundObj = surroundObj;
        Direction = direction;
        Offset = offset;
        IsFollowRole = isFollowRole;
    }

    public override void Awake(RoleItemController rpe)
    {
        base.Awake(rpe);
        timer = 0;
    }

    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        if (!surroundObj) return;
        insSurround = GameObject.Instantiate(surroundObj);
        roleController.roleSurroundController.AddObj(insSurround);
        insSurround.gameObject.SetActive(roleController.CurrentHp == roleController.MaxHp);
    }

    public override void Update(RoleItemController rpe)
    {
        base.Update(rpe);
        bool fullHp = roleController.CurrentHp == roleController.MaxHp;
        if (fullHp)
        {
            timer += Time.deltaTime;
            if (timer>cd)
            {
                timer = 0;
                SpawnObj();
            }

            if (insSurround&&!insSurround.gameObject.activeSelf)
            {
                insSurround.gameObject.SetActive(true); //环绕物是否显
            }
        }
        else
        {
            if (insSurround&&insSurround.gameObject.activeSelf)
            {
                insSurround.gameObject.SetActive(false); //环绕物是否显
            }
        }
        
        
    }
    /// <summary>
    /// 产生爆炸物
    /// </summary>
    void SpawnObj()
    {
        if (SpwanObj==null)
        {
            return;
        }
        Vector3 position = Vector3.zero;
        Vector3 forward = Vector3.forward;
        position = BattleManager.Inst.CurrentPlayer.Animator.transform.position;

        var transform = BattleManager.Inst.CurrentPlayer.Animator.transform;
        position += transform.right * Offset.x +
                    transform.up * Offset.y +
                    transform.forward * Offset.z;
        forward = transform.right * Direction.x +
                  transform.up * Direction.y +
                  transform.forward * Direction.z;
        
        insSwpanObj = GameObject.Instantiate(SpwanObj);
        insSwpanObj.transform.position = position;
        insSwpanObj.transform.forward = forward;
        
        if (IsFollowRole)
        {
            insSwpanObj.transform.SetParent(roleController.GetComponent<RoleNode>().Body.transform);
            insSwpanObj.transform.localPosition=new Vector3(insSwpanObj.transform.localPosition.x,0,insSwpanObj.transform.localPosition.z);
        }
        
        //如果是区域伤害物，需要初始化伤害来源
        if (insSwpanObj.GetComponent<DmgBuffOnTouch>())
        {
            insSwpanObj.GetComponent<DmgBuffOnTouch>().Init(roleController);
        }
        
    }

    

    public override void Destroy(RoleItemController rpe)
    {
        if (insSurround)
        {
            roleController.roleSurroundController.RemoveObj(surroundObj);
        }
        
        if (insSwpanObj)
            GameObject.Destroy(insSwpanObj);
        
        base.Destroy(rpe);
    }
    
}