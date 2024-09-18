using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UI_UnLockHeroPanel : PanelBase
{
    protected override void OnPause()
    {
        
    }

    protected override void OnUnPause()
    {
        
    }

    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private UIText _heroName;

    private Transform _uiHeroPrefab;
    private Vector3 _oldHeroPosition;
    private void Awake()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1, 0.3f).SetDelay(2f).Delay();
        var cameraObj = GameObject.Find("UIHeroCamera");
        if (cameraObj != null)
            _uiHeroPrefab = cameraObj.transform.GetChild(0);
        if (_uiHeroPrefab != null)
        {
            _oldHeroPosition = _uiHeroPrefab.localPosition;
            _uiHeroPrefab.localPosition = new Vector3(0,-0.8f,_oldHeroPosition.z);
        }
    }

    private void OnDestroy()
    {
        if (_uiHeroPrefab != null)
        {
            _uiHeroPrefab.localPosition = _oldHeroPosition;
        }
    }

    public void OnOpen(int heroId)
    {
        HeroData heroData = DataManager.Inst.HeroDatas[heroId];
        _heroName.text = heroData.HeroName;
    }

    public void OnCloseBtnClick()
    {
        UIManager.Inst.Close("UnLockHeroPanel");
    }
}
