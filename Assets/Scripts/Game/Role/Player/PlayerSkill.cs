using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
//不能直接用
public class PlayerSkill : MonoBehaviour
{
    public RoleController roleController;
    public int RoleSkillLevel=>roleColorLevel;
    protected int roleColorLevel;//1,2,3
    [LabelText("游戏内调整等级")]
    public bool IsDebug;
    private void OnGUI()
    {
        ////手动改变等级。
        //if (GUI.Button(new Rect(300, 0, 100, 100), "Level1"))
        //{
        //    roleColorLevel = 1;
        //}
        //if (GUI.Button(new Rect(300, 100, 100, 100), "Level2"))
        //{
        //    roleColorLevel = 2;
        //}
        //if (GUI.Button(new Rect(300, 200, 100, 100), "Level3"))
        //{
        //    roleColorLevel = 3;
        //}
    }

    public virtual void Init()
    {
        roleController = this.gameObject.GetComponent<RoleController>();
        roleColorLevel = ArchiveManager.Inst.GetHeroUpgradeData(roleController.UniqueID).ColorLevel;
        EventManager.Inst.AddAnimatorEvent(AnimEvent);
    }
    protected virtual void OnEnable()
    {
        //EventManager.Inst.AddAnimatorEvent(AnimEvent);
    }
    virtual protected void OnDestroy()
    {
        EventManager.Inst.RemoveAnimatorEvent(AnimEvent);
    }

    protected virtual void AnimEvent(GameObject go, string eventName)
    {
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
    }
}