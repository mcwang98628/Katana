using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerBar : MonoBehaviour
{
    [SerializeField] private AnimationCurve scaleAnimationCurve;
    [Header("PlayerHp")]
    [SerializeField] private RectTransform hpBarGroup;
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider hpLerpBar;
    [SerializeField] private Text hpText;
    [SerializeField] private Image hpBarFill;
    [SerializeField] private float hpLerpSpeed;

    [Header("Fever")]
    [SerializeField]
    private Slider feverPowerBar;
    [SerializeField]
    private Image feverFill;
    [SerializeField]
    private Color feverColor1;
    [SerializeField]
    private Color feverColor2;

    private Player_Fever _playerFever;
    
    private void OnEnable()
    {
        CheckFeverPower();
        EventManager.Inst.AddEvent(EventName.OnRoleInjured, OnPlayerInjured);
        if (_playerFever != null)
            _playerFever.FeverChangeEvent += OnFeverChange;
    }

    private void OnDisable()
    {
        EventManager.Inst.RemoveEvent(EventName.OnRoleInjured, OnPlayerInjured);
        if (_playerFever != null)
            _playerFever.FeverChangeEvent -= OnFeverChange;
    }


    private void OnFeverChange(bool isFever)
    {
        if (isFever)
        {
            feverFill.color = feverColor1;
        }
        else
        {
            feverFill.color = feverColor2;
        }
    }



    void CheckFeverPower()
    {
        _playerFever = BattleManager.Inst.CurrentPlayer.GetComponent<Player_Fever>();
        feverPowerBar.gameObject.SetActive(_playerFever!=null);
        if (_playerFever != null)
        {
            OnFeverChange(_playerFever.Fevering);
        }
        else
        {
            OnFeverChange(false);
        }
    }


    private Tweener scaleTweener;
    private Tweener colorTweener;
    private void OnPlayerInjured(string arg1, object id)
    {
        if (BattleManager.Inst.CurrentPlayer == null)
        {
            return;
        }
        var roleid = ((RoleInjuredInfo)id).RoleId;
        var dmgValue = ((RoleInjuredInfo)id).Dmg.DmgValue;
        if (roleid != BattleManager.Inst.CurrentPlayer.TemporaryId)
        {
            return;
        }
        float ScalePara = (dmgValue / BattleManager.Inst.CurrentPlayer.MaxHp)<0.05f?1.1f:1.5f;

        hpBar.transform.localScale = Vector3.one;
        if (scaleTweener != null)
        {
            scaleTweener.Kill(true);
        }
        scaleTweener = hpBar.transform.DOScale(Vector3.one * ScalePara, 0.3f).SetEase(scaleAnimationCurve);

        hpBarFill.color = new Color(0.36f, 0.72f, 0.2f);
        if (colorTweener != null)
        {
            colorTweener.Kill(true);
        }
        colorTweener = hpBarFill.DOColor(new Color(1f, 0, 0), 0.3f).SetEase(scaleAnimationCurve);

    }

    void Update()
    {
        if (BattleManager.Inst.CurrentPlayer != null)
        {
            hpBar.value = BattleManager.Inst.CurrentPlayer.CurrentHp / BattleManager.Inst.CurrentPlayer.MaxHp;
            hpLerpBar.value = Mathf.Lerp(hpLerpBar.value, hpBar.value, hpLerpSpeed);
            var value = Mathf.CeilToInt(BattleManager.Inst.CurrentPlayer.CurrentHp);
            if (value > BattleManager.Inst.CurrentPlayer.MaxHp)
                value = (int) BattleManager.Inst.CurrentPlayer.MaxHp;
            hpText.text = value + " / " + ((int)BattleManager.Inst.CurrentPlayer.MaxHp);

        }
        
        if (_playerFever != null)
        {
            feverPowerBar.value = _playerFever.CurrentPower / _playerFever.MaxPower;
        }
    }
}
