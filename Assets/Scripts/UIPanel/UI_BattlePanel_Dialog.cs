using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattlePanel_Dialog : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private Text text;
    [SerializeField]
    private Vector2 offset;
    [SerializeField]
    private GameObject _dialogArror;
    [SerializeField]
    private GameObject _dialogClickBtn;
    [SerializeField]
    private AudioClip _finishDialog; 
    [SerializeField]
    private Image _bgImage1;
    [SerializeField]
    private Image _bgImage2;

    public void ShowDialog(string textKey,float showTime,Color textColor,bool force)
    {
        DOTween.Kill(this.gameObject, true);
        DOTween.Kill(this._canvasGroup, true);
        DOTween.Kill(this.text, true);

        if (force)
        {
            _bgImage1.color = new Color(0, 0, 0, 1f);
            _bgImage2.color = new Color(0, 0, 0, 1f);
        }
        else
        {
            _bgImage1.color = new Color(0, 0, 0, 0.8f);
            _bgImage2.color = new Color(0, 0, 0, 0.8f);
        }

        gameObject.SetActive(true);
        _dialogArror.SetActive(false);
        _dialogClickBtn.SetActive(false);
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1, 0.3f);
        text.text = "";
        string str = LocalizationManger.Inst.GetText(textKey);
        text.color = textColor;
        text.DOText(str, 0.5f).OnComplete(() =>
        {
            if (force)
            {
            _dialogArror.SetActive(force);
            _dialogClickBtn.SetActive(force);
            AudioManager.Inst.PlaySource(_finishDialog);
            }
        }).SetDelay(0.2f).Delay();
        if (!force)
        {
            HideDialog(showTime + 1f);
        }
        
    }

    public void HideDialog(float delayTime)
    {
        _canvasGroup.DOKill(true);
        
        _canvasGroup.DOFade(0, 0.2f).OnComplete(() =>
        {
            text.text = "";
            gameObject.SetActive(false);
        }).SetDelay(delayTime).Delay();
    }

    public void OverTextTween()
    {
        DOTween.Kill(this.text, true);
    }

    private RoleController roleController;

    private void Awake()
    {
        roleController = BattleManager.Inst.CurrentPlayer;
    }

    private void Update()
    {
        if (roleController != null)
        {
            if (Camera.main != null)
            {
                Vector3 targetPos;
                if (roleController.roleNode.Head != null)
                {
                    targetPos = roleController.roleNode.Head.position;
                }
                else
                {
                    targetPos = roleController.transform.position;
                }
                transform.position = Camera.main.WorldToScreenPoint(targetPos) + new Vector3(offset.x, offset.y, 0);
            } 
        }
    }
}
