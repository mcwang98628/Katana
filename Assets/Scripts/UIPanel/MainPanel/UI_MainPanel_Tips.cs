using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainPanel_Tips : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private UIText _uiText;
    [SerializeField]
    private Image _assetIcon;

    [SerializeField]
    private Sprite _diamond;
    [SerializeField]
    private Sprite _soul;
    [SerializeField]
    private Sprite _fire;

    public void Show(UI_Asset.AssetType assetType)
    {
        DOTween.Kill(_canvasGroup, false);
        _canvasGroup.alpha = 0;
        switch (assetType)
        {
            case UI_Asset.AssetType.Diamond:
                _assetIcon.sprite = _diamond;
                _uiText.text = "UI_Tips_NoDiamond";
                break;
            case UI_Asset.AssetType.Soul:
                _assetIcon.sprite = _soul;
                _uiText.text = "UI_Tips_NoSoul";
                break;
            case UI_Asset.AssetType.Fire:
                _assetIcon.sprite = _fire;
                _uiText.text = "UI_Tips_NoFire";
                break; 
        }
        _assetIcon.gameObject.SetActive(true);
        _canvasGroup.DOFade(1f, 0.3f);
        _canvasGroup.DOFade(0, 0.3f).SetDelay(2f).Delay();
    }

    public void Show(string textKey)
    {
        DOTween.Kill(_canvasGroup, false);
        _uiText.text = textKey;
        _assetIcon.gameObject.SetActive(false);
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1f, 0.3f);
        _canvasGroup.DOFade(0, 0.3f).SetDelay(2f).Delay();
    }
}
