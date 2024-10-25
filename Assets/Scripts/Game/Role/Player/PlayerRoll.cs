using System.Collections;
using System.Collections.Generic;
using AnimatorTools;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerRoll : RoleRoll
{
    [SerializeField]
    protected float coolDown=1f;
    float timer;
    [SerializeField]
    [LabelText("TestRoll Type")]
    protected int currentRollType = 1;

    public float CoolPercent=1;
    public int CurrentRollType => currentRollType;

    private static readonly int RollType = Animator.StringToHash("RollType");

    
    private static readonly int RollEndString = Animator.StringToHash("RollEnd");

    protected override void Awake()
    {
        base.Awake();

        ((PlayerController)roleController).InputAction.OnInputEvent += OnInput;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        // ReSharper disable once DelegateSubtraction
        ((PlayerController)roleController).InputAction.OnInputEvent -= OnInput;
    }

    public void SetRollType(int rollType)
    {
        currentRollType = rollType;
    }
    
#if UNITY_ANDROID || UNITY_IOS
    protected virtual void OnInput(JoyStatusData statusData)
    {

        if (statusData.JoyStatus == UIJoyStatus.OnSlide || statusData.JoyStatus == UIJoyStatus.OnHoldSlide)
        {
            if (statusData.SlideDir == Vector2.zero)
            {
                return;
            }

            roleController.InputRoll(InputManager.Inst.GetCameraDir(statusData.SlideDir));
        }
        if (Input.GetMouseButtonUp(1))
        {
            roleController.InputRoll(roleController.transform.forward);
        }
    }
#else
    protected virtual void OnInput()
    {
        if (Input.GetMouseButtonUp(1))
        {
            roleController.InputRoll(roleController.transform.forward);
        }
    }
#endif

    public override void InputRoll(Vector2 v2)
    {
        //OnStartRole();
        roleController.Animator.SetInteger(RollType, currentRollType);
        if (roleController.Animator2 != null)
        {
            roleController.Animator2.SetInteger(RollType, currentRollType);
        }
        // roleController.SetDodge(true);

        base.InputRoll(v2);
        // rollSphere.SetActive(true);
        // GameModel.SetActive(false);
        // StartCoroutine(StopRollAfter(.31f));
        roleController.SetIsRoll(true);
        roleController.SetIsAttacking(false);
        roleController.gameObject.layer = LayerMask.NameToLayer("PlayerRoll");
        timer=0;
    }
    protected override void RollBack()
    {
        OnEndRoll();
        base.RollBack();
        roleController.SetIsRoll(false);
        // rollSphere.SetActive(false);
        // GameModel.SetActive(true);
        // roleController.SetIsAttacking(false);
        roleController.gameObject.layer = LayerMask.NameToLayer("Player");
    }
    List<BreakableObj> _breakableObjs = new List<BreakableObj>();
    protected virtual void Update()
    {
        if (roleController.IsRolling)
        {
            List<BreakableObj> breakableObjs = BreakableObjManager.Inst.BreakObjsInRange(roleController, 1.5f, 360);
            foreach (BreakableObj obj in breakableObjs)
            {
                if (_breakableObjs.Contains(obj))
                    continue;
                
                obj.BreakObj();
                _breakableObjs.Add(obj);
            }

            if (breakableObjs.Count>0)
            {
                StartCoroutine(DelayStopPlayer(0.18f));
            }

        }
        else
        {
            _breakableObjs.Clear();
        }
        timer+=Time.deltaTime;
        timer = timer>coolDown?coolDown:timer;
    } 
    IEnumerator DelayStopPlayer(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
         roleController.StopFastMove();
    }
    
    IEnumerator StopRollAfter(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        RollBack();
        roleController.SetDodge(false);

    }
}
