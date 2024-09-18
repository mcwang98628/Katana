using System;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattlePanel_RollBtn : MonoBehaviour
{
    [SerializeField]
    private BattlePanel_ItemBtn_Joy joyBtn;
    [SerializeField]
    private Image mask;

    [SerializeField]
    private AnimationCurve _curve;
    [SerializeField]
    private AudioClip _audio;
    private PlayerRoll playerRoll;
    private void Start()
    {
        if (BattleManager.Inst.CurrentPlayer.roleRoll is PlayerRoll _playerRoll)
        {
            playerRoll = _playerRoll;
        }
        EventManager.Inst.AddEvent(EventName.RollCool, OnRollCool);
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.RollCool, OnRollCool);
    }

    private void OnRollCool(string arg1, object arg2)
    {
        transform.DOScale(Vector3.one * 1.3f, 0.2f).SetEase(_curve);
        mask.DOColor(new Color(1, 0, 0, 0.8f), 0.2f).SetEase(_curve);
        AudioManager.Inst.PlaySource(_audio);
    }

    private void Update()
    {
        UpdateBtnCoolDown();
    }

    private bool isCanUse;

    public void UpdateBtnCoolDown()
    {
        float value = playerRoll.CoolPercent;
        if (!isCanUse)
        {
            if (value == 1)
            {
                isCanUse = true;
                transform.DOScale(Vector3.one * 1.2f, 0.2f);
            }
        }
        if (value < 1)
        {
            isCanUse = false;
            mask.fillAmount = Mathf.Pow(1 - value, 2f);
        }
        else
            mask.fillAmount = 0;
    }

    public void OnBtnClick()
    {
        BattleManager.Inst.CurrentPlayer.InputRoll(Vector2.zero);
    }
}
