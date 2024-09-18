using System;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattlePanel_ItemBtn : MonoBehaviour
{
    [SerializeField]
    private BattlePanel_ItemBtn_Joy joyBtn;
    [SerializeField]
    private Image btnIcon;
    [SerializeField]
    private Image mask;
    [SerializeField]
    private Image bg;
    [SerializeField]
    private Text number;

    [SerializeField]
    private AnimationCurve _curve;
    [SerializeField]
    private CanvasGroup _canvasGroup;
    public Item ItemData => _item;
    private Item _item;
    public void Init(Item item)
    {
        _item = item;
        gameObject.SetActive(true);
        btnIcon.gameObject.SetActive(item != null);
        number.gameObject.SetActive(item != null);
        if (item == null)
        {
            return;
        }
        joyBtn.ItemUseType = item.ItemUseType;
        
        btnIcon.sprite = item.Icon;
        // mask.sprite = item.Icon;
        
        if (bg != null)
        {
            bg.color = DataManager.Inst.HeroDatas[BattleManager.Inst.CurrentPlayer.UniqueID].Color;
            bg.color= new Color(bg.color.r,bg.color.g,bg.color.b,0.15f);
        }
        
        number.text = _item.RemainingTimes.ToString();
        number.gameObject.SetActive(_item.RemainingTimes != 999);

        if (item.ItemType == ItemType.ButtonSkill && BattleManager.Inst.RuntimeData is ChapterRulesRuntimeData runtimeData && runtimeData.IsTutorial)
        {
            this.gameObject.SetActive(false);
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
        }
        
    }

    private void Awake()
    {
        EventManager.Inst.AddEvent(EventName.OnItemDataUpdate, OnItemDataUpdate);
        EventManager.Inst.AddEvent(EventName.ShowSkillBtn,OnShowSkillBtn);
    }


    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnItemDataUpdate, OnItemDataUpdate);
        EventManager.Inst.RemoveEvent(EventName.ShowSkillBtn,OnShowSkillBtn);
    }

    private void OnShowSkillBtn(string arg1, object arg2)
    {
        this.gameObject.SetActive(true);
        _canvasGroup.DOFade(1, 0.3f);
        _canvasGroup.interactable = true;
    }

    private void OnItemDataUpdate(string arg1, object arg2)
    {
        if (_item == null)
        {
            return;
        }
        Item item = (Item)arg2;
        if (item.TemporaryId != _item.TemporaryId)
        {
            return;
        }
        number.text = _item.RemainingTimes.ToString();
    }


    private void Update()
    {
        if (_item == null)
        {
            return;
        }
        UpdateBtnCool(1 - _item.CoolingPercentage);
    }

    private bool isCanUse;
    //更新btn冷却
    public void UpdateBtnCool(float value)
    {
        if (value<0)
        {
            value = 0;
        }
        if (value <= 0 && !isCanUse)
        {
            isCanUse = true;
            btnIcon.transform.localScale = Vector3.one;
            btnIcon.transform.DOScale(Vector3.one * 1.3f, 0.3f).SetEase(_curve).OnComplete(() =>
            {
                btnIcon.transform.localScale = Vector3.one;
                btnIcon.transform.localPosition = Vector3.zero;
                btnIcon.transform.localRotation = new Quaternion(0,0,0,0);
            });
        }
        else if(value>0)
        {
            isCanUse = false;
        }
        if(isCanUse)
            _canvasGroup.alpha=1;
        else
            _canvasGroup.alpha=0.15f;

        mask.fillAmount = value;
    }

    public void OnBtnClick()
    {
        if (_item == null)
        {
            return;
        }
        _item.UseActive();
    }

    public void OnTouch(bool isDown)
    {
        if (_item == null)
        {
            return;
        }
        _item.OnBtnTouch(isDown);
    }

    public void OnDrag(bool isDown,Vector2 value)
    {
        if (_item == null)
        {
            return;
        }
        _item.OnDrag(isDown,value);
    }
}
