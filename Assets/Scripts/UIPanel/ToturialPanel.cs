using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum ToturialGuideType
{
    Move,
    Attack,
    Roll,
    Click,
    Skill,
}

public class ToturialPanel : PanelBase
{
    
    private float time;
    private ToturialGuideType _currentToturialGuide;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Image mask;


    private bool isInit;
    private bool isPause;
    private Coroutine Coroutine;
    public void OnOpen(ToturialGuideType type)
    {
        isInit = true;
        _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        time = Time.unscaledTime;
        _currentToturialGuide = type;
        if (!isPause)
        {
            if (Coroutine!=null)
            {
                StopCoroutine(Coroutine);
            }
            Coroutine = StartCoroutine(waitUnPause());   
        }
        switch (type)
        {
            case ToturialGuideType.Move:
                _animator.SetTrigger("Move");
                break;
            case ToturialGuideType.Attack:
                _animator.SetTrigger("Attack");
                break;
            case ToturialGuideType.Roll:
                _animator.SetTrigger("Roll");
                break;
            case ToturialGuideType.Click:
                _animator.SetTrigger("Attack");
                break;
            case ToturialGuideType.Skill:
                _animator.SetTrigger("Skill");
                break;
        }

    }

    private void OnEnable()
    {
        if (!isInit)
        {
            return;
        }
        OnOpen(_currentToturialGuide);
    }

    IEnumerator waitUnPause()
    {
        isPause = true;
        // TimeManager.Inst.SetTimeScale(0.2f);
        yield return new WaitForSecondsRealtime(2f);
        // TimeManager.Inst.SetTimeScale(1f);
    }
    
    public override void Show()
    {
        gameObject.SetActive(true);
        IsShow = true;
        if (_canvasGroup == null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        // _canvasGroup.interactable = false;
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1, 0.15f);
        StartCoroutine(waitInteractable());
    }


    IEnumerator waitInteractable()
    {
        _canvasGroup.interactable = false;
        mask.raycastTarget = false;
        yield return new WaitForSecondsRealtime(2f);
        // _canvasGroup.interactable = false;
        mask.raycastTarget = false;
    }

    protected override void OnPause()
    {
        
    }

    protected override void OnUnPause()
    {
        
    }

    private void Awake()
    { 
        EventManager.Inst.DistributeEvent(EventName.JoyUp);
        EventManager.Inst.AddEvent(EventName.ExitRoom,OnNextRoom);
        EventManager.Inst.AddEvent(EventName.OnUseItem,OnUseItem);
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.ExitRoom,OnNextRoom);
        EventManager.Inst.RemoveEvent(EventName.OnUseItem,OnUseItem);
    }

    private void OnUseItem(string arg1, object arg2)
    {
        if (_currentToturialGuide == ToturialGuideType.Skill)
        {
            UIManager.Inst.Close("ToturialPanel");
        }
    }

    private void OnNextRoom(string arg1, object arg2)
    {
        UIManager.Inst.Close("ToturialPanel");
    }

    private bool isHide = false;
    // private float timer;
    // private float showTime = 3f;
    private void Update()
    {
        if (_currentToturialGuide == ToturialGuideType.Click && Input.GetMouseButtonDown(0) && !_canvasGroup.interactable)
        {
            UIManager.Inst.Close("ToturialPanel");
            return;
        }

        if (BattleManager.Inst.CurrentPlayer == null)
        {
            return;
        }
        
        if ((_currentToturialGuide == ToturialGuideType.Move && BattleManager.Inst.CurrentPlayer.IsMoving)
                || (_currentToturialGuide == ToturialGuideType.Attack && BattleManager.Inst.CurrentPlayer.IsAttacking)
                || (_currentToturialGuide == ToturialGuideType.Roll && BattleManager.Inst.CurrentPlayer.IsRolling))
        {
            // if(timer<1)//防止开局取消伤害
            // {
            //     timer += Time.deltaTime;
            //     return;
            // }    
            //
            // timer = 0;
            if (!isHide)
            {
                isHide = true;
                _canvasGroup.DOFade(0, 0.25f);
            }
        }
        else
        {
            // timer += Time.deltaTime;
            // if (timer>=showTime)
            {
                if (isHide)
                {
                    isHide = false;
                    _canvasGroup.DOFade(1, 0.25f);
                    OnOpen(_currentToturialGuide);
                }
            }
        }
    }


    // public void OnBtnClick()
    // {
    //     if (Time.unscaledTime - time < 1)
    //     {
    //         //防止玩家在界面打开一瞬间就关闭，至少等待1秒。
    //         return;
    //     }
    //     UIManager.Inst.Close();
    // }
}
