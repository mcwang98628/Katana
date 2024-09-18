using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO 废弃脚本 待删除
public class MainScenePlayerMove : MonoBehaviour
{
    public Rigidbody Rigidbody;
    public Animator Animator;

    private void Awake()
    {
        // EventManager.Inst.AddEvent(EventName.JoyClick,OnJoyClick);
    }

    private void OnDestroy()
    {
        // EventManager.Inst.RemoveEvent(EventName.JoyClick,OnJoyClick);
    }

    private void OnJoyClick(string arg1, object arg2)
    {
        if (MainSceneInteractObjManager.Inst.CurrentSelectObj != null)
        {
            MainSceneInteractObjManager.Inst.CurrentSelectObj.InteractEvent.Invoke();
        }
    }

    // void Update()
    // {
    //     var target = new Vector3(InputManager.Inst.JoyValue.x, 0, InputManager.Inst.JoyValue.y);
    //     Rigidbody.velocity = new Vector3(InputManager.Inst.JoyValue.x,0,InputManager.Inst.JoyValue.y)*5f;
    //     if (InputManager.Inst.JoyValue != Vector2.zero)
    //     {
    //         Animator.transform.forward = target;
    //         Animator.SetFloat("MoveSpeed",2f);
    //     }
    //     else
    //     {
    //         Animator.SetFloat("MoveSpeed",0f);
    //     }
    // }
}
