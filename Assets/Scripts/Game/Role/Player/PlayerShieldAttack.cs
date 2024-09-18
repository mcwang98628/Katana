using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerShieldAttack : PlayerAttack
{
    [BoxGroup("盾冲")][SerializeField][LabelText("最小盾冲距离")][GUIColor(1,1,0)]
    private float minSprintDistance;
    [BoxGroup("盾冲")][SerializeField][LabelText("最大盾冲距离")][GUIColor(1,1,0)]
    private float maxSprintDistance;
    [BoxGroup("盾冲")][SerializeField][LabelText("最大盾冲蓄力时间")][GUIColor(1,1,0)]
    private float maxAccumulateTime;
    [BoxGroup("盾冲")][SerializeField][LabelText("蓄力提示颜色")][GUIColor(1,1,0)]
    private Color SprintRectColor;
    //盾冲 指示器
    private AttackIndeicate_Rect sprintRect;

    public float CurrentSprintDistance => Mathf.Lerp(minSprintDistance, maxSprintDistance, (Time.time - AccumulateingStartTime) / maxAccumulateTime);
    
    
    public AudioClip DashShieldSFX;
    
    RoleController ShieldAttackRoleController;
    public AudioClip ShieldAttackSFX;
    protected override void Awake()
    {
        base.Awake();
        // EventManager.Inst.AddEvent(EventName.JoyStatus,OnJoyStatus);
        sprintRect = IndicatorManager.Inst.ShowRectangleIndicator();
        sprintRect.SetColor(SprintRectColor);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        // EventManager.Inst.RemoveEvent(EventName.JoyStatus,OnJoyStatus);
        IndicatorManager.Inst.Recover(sprintRect);
    }

    protected override void OnInput(JoyStatusData joyStatusData)
    {
        if (roleController.IsDie)
        {
            SetAccumulateing(false);
            sprintRect.gameObject.SetActive(false);
            return;
        }
        if (joyStatusData.JoyStatus == UIJoyStatus.OnPressUp && !IsAccumulateing)
        {
            roleController.InputAttack();
            return;
        }
        
        if (joyStatusData.JoyStatus == UIJoyStatus.OnHoldStart)
        {
            SetAccumulateing(true);
            roleController.Animator.ResetTrigger("AccumulateAttack");
            
            SprintRectUpdate();
        }
        else if(joyStatusData.JoyStatus == UIJoyStatus.HoldDraging ||
                joyStatusData.JoyStatus == UIJoyStatus.Holding)
        {
            SetAccumulateing(true);
            if (joyStatusData.JoyValue != Vector2.zero)
            {
                Vector2 dir2 = InputManager.Inst.GetCameraDir(joyStatusData.JoyValue);
                dir2 = InputRotate(dir2);
                roleController.Animator.transform.forward = new Vector3(dir2.x,0,dir2.y);
            }
        
            SprintRectUpdate();
        }
        else if(joyStatusData.JoyStatus == UIJoyStatus.Idle)
        {
            SetAccumulateing(false);
            sprintRect.gameObject.SetActive(false);
        }
    }

    //
    // private void OnJoyStatus(string arg1, object arg2)
    // {
    //     if (roleController.IsDie)
    //     {
    //         SetAccumulateing(false);
    //         sprintRect.gameObject.SetActive(false);
    //         return;
    //     }
    //     JoyStatusData joyStatusData = (JoyStatusData) arg2;
    //     if (joyStatusData.JoyStatus == UIJoyStatus.OnHoldStart)
    //     {
    //         SetAccumulateing(true);
    //         roleController.Animator.ResetTrigger("AccumulateAttack");
    //         
    //         SprintRectUpdate();
    //     }
    //     else if(joyStatusData.JoyStatus == UIJoyStatus.HoldDraging ||
    //             joyStatusData.JoyStatus == UIJoyStatus.Holding)
    //     {
    //         SetAccumulateing(true);
    //         if (joyStatusData.JoyValue != Vector2.zero)
    //         {
    //             Vector2 dir2 = InputManager.Inst.GetCameraDir(joyStatusData.JoyValue);
    //             dir2 = InputRotate(dir2);
    //             roleController.Animator.transform.forward = new Vector3(dir2.x,0,dir2.y);
    //         }
    //
    //         SprintRectUpdate();
    //     }
    //     else if(joyStatusData.JoyStatus == UIJoyStatus.Idle)
    //     {
    //         SetAccumulateing(false);
    //         sprintRect.gameObject.SetActive(false);
    //     }
    // }

    [SerializeField]
    private float _turningAngSpeed = 180f;
    Vector2 InputRotate(Vector2 dirV2)
    {
        if (dirV2 != Vector2.zero)
        {
            var forward = roleController.Animator.transform.forward;
            float ang = Vector3.Angle(forward, new Vector3(dirV2.x, 0, dirV2.y));
            float value = 1;
            if (Mathf.Abs(ang)>0 && _turningAngSpeed>0)
            {
                value =(_turningAngSpeed*Time.deltaTime)/ang;
                if (value>1)
                {
                    value = 1;
                }
            }
            var v2Value = Vector3.SlerpUnclamped(forward, new Vector3(dirV2.x, 0, dirV2.y), value);

            return new Vector2(v2Value.x,v2Value.z);
        }
        return dirV2;
    }
    void SprintRectUpdate()
    {
        if (!IsAccumulateing)
        {
            sprintRect.gameObject.SetActive(false);
            return;
        }
        sprintRect.gameObject.SetActive(true);
        var rect = sprintRect.transform;
        rect.position = roleController.transform.position+new Vector3(0,0.2f,0);
        rect.forward = roleController.Animator.transform.forward;
        rect.localScale = new Vector3(1.6f,0,CurrentSprintDistance);
    }
    
    protected override void AnimEvent(GameObject go, string eventName)
    {
        base.AnimEvent(go,eventName);
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
        if(eventName== "DmgStart_ShieldDashFinishAttack")
        {
            DashShieldSFX.Play();
        }
        else if (eventName == "ShieldAttackStart")
        {
            ShieldAttackSFX.Play();
        }
    }
    
    
    public override void PlayAttackAudio()
    {
        //当前攻击不是持盾冲刺
        if (currentAttackIndex != 20)
        {
            base.PlayAttackAudio();
        }
    }
    
    public override void DoSomeThingBeforeDamage(AttackInfo ai)
    {
        //当前攻击如果不是持盾冲刺
        if (currentAttackIndex != 20)
        {
            base.DoSomeThingBeforeDamage(ai);
        }
    }
}
