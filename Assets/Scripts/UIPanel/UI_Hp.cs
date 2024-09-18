using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UI_Hp : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Vector2 offset;
    [SerializeField]
    private Slider LerpSlider;
    [SerializeField]
    private Text hpText;
    public float LerpMul = 0.03f;
    float StartLerpDelay = 0.1f;
    bool isDelaying = false;
    float tiemr = -1;
    RectTransform rect;
    private RoleController roleController;
    private float maxHp;

    CanvasGroup canvasGroup;


    [SerializeField]
    private Image topHpBarImg;
    [SerializeField]
    private Image downHpBarImg;

    public void SetTarget(RoleController roleController_)
    {
        roleController = roleController_;
        maxHp = roleController.MaxHp;
        rect = GetComponent<RectTransform>();
        /*if (roleController.tag == "Player")
        {
            Destroy(gameObject);
        }*/
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;

        SetSize(roleController);
        // SetColor(roleController);

        EventManager.Inst.AddEvent(EventName.OnRoleInjured, OnRoleInjured);
        EventManager.Inst.AddEvent(EventName.OnRoleTreatment, OnRoleTreatment);

        UpdateHpBar();

        gameObject.SetActive(true);
        //canvasGroup.alpha = 0;
        
        if (roleController.roleTeamType == RoleTeamType.Player)
        {
            // LerpSlider.gameObject.SetActive(false);
        }
        else
        {
            canvasGroup.alpha = 0;
        }
        
    }

    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnRoleInjured,OnRoleInjured);
        EventManager.Inst.RemoveEvent(EventName.OnRoleTreatment, OnRoleTreatment);
    }

    private void OnRoleTreatment(string arg1, object arg2)
    {
        TreatmentData data = (TreatmentData) arg2;
        if (data.RoleId != roleController.TemporaryId)
            return;
        if (data.RoleId != BattleManager.Inst.CurrentPlayer.TemporaryId)
            ShowHpBar(); 
        
        UpdateHpBar(1);
    }

    protected virtual void OnRoleInjured(string arg1, object arg2)
    {
        var data = (RoleInjuredInfo)arg2;
        if (data.RoleId != roleController.TemporaryId)
            return;
        if (data.RoleId != BattleManager.Inst.CurrentPlayer.TemporaryId)
            ShowHpBar();   
        
        UpdateHpBar(-1);
    }
    // void SetColor(RoleController roleController)
    // {
    //     if (roleController.roleTeamType == RoleTeamType.Enemy ||
    //    roleController.roleTeamType == RoleTeamType.Enemy_Boss ||
    //    roleController.roleTeamType == RoleTeamType.EliteEnemy)
    //     {
    //         slider.fillRect.GetComponent<Image>().color = Color.red;
    //     }
    //     else if (roleController.roleTeamType == RoleTeamType.Player)
    //     {
    //         // canvasGroup.alpha = 0; 
    //         slider.fillRect.GetComponent<Image>().color = new Color(0.3f, 0.7f, 0.2f);
    //     }
    //
    // }
    public void SetSize(RoleController roleController)
    {
        int width = 100;
        int height = 36;
        if (roleController.roleTeamType == RoleTeamType.Enemy ||
        roleController.roleTeamType == RoleTeamType.Enemy_Boss||
        roleController.roleTeamType == RoleTeamType.EliteEnemy)
        {
            if (maxHp < 100)
            {
                width = 36;
            }
            else if (maxHp < 900)
            {
                width = 120;
            }
            else if (maxHp < 2000)
            {
                width = 160;
            }
            else
            {
                height = 36;
                width = 240;
            }
        }
        else if (roleController.roleTeamType == RoleTeamType.Player)
        {
            height = 36;
            width = (int)(roleController.MaxHp/5);
            width = Mathf.Clamp(width,100,160);
        }
        rect.sizeDelta = new Vector2(width, height);
    }

    public void ShowHpBar()
    {
        if (canvasGroup == null)
            return;
        
        canvasGroup.alpha = 1; 
        
        // if (DelayHideHpBarCoroutine != null)
        // {
        //     StopCoroutine(DelayHideHpBarCoroutine);
        // }
        // DelayHideHpBarCoroutine = StartCoroutine(DelayHideHpBar(2.5f));
    }

    Coroutine DelayHideHpBarCoroutine;
    IEnumerator DelayHideHpBar(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canvasGroup.DOFade(0,0.1f);
        DelayHideHpBarCoroutine = null;
    }

    public void FadeOff(float fadeTime)
    {
        transform.DOScale(transform.localScale * 3, fadeTime);
        canvasGroup.DOFade(0, fadeTime);
    }
    void Update()
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
        else
        {
            Destroy(gameObject);
        }
    }


    private int lastHpBarCount = -1;//最后一次血条数量
    private Coroutine doHpBarCoroutine;
    void UpdateHpBar(int dir = -1)
    {
        int hpBarCount = roleController.roleHealth.HpBarCount;
        float hpBarValue = roleController.MaxHp / hpBarCount;
        int currentHpBarCount = Mathf.CeilToInt(roleController.CurrentHp / hpBarValue);
        if (currentHpBarCount > hpBarCount)
            currentHpBarCount = hpBarCount;
        float hpSliderValue = (roleController.CurrentHp % hpBarValue) / hpBarValue;
        if (roleController.CurrentHp == roleController.MaxHp) 
            hpSliderValue = 1;
        
        
        if (doHpBarCoroutine != null)
        {
            GameManager.Inst.StopCoroutine(doHpBarCoroutine);
            slider.DOKill(true);
            LerpSlider.DOKill(true);
        }
        doHpBarCoroutine = GameManager.Inst.StartCoroutine(DoHpBar(lastHpBarCount,currentHpBarCount,hpSliderValue,dir));
    }

    IEnumerator DoHpBar(int oldBarCount,int newBarCount,float targetValue,int dir=-1)
    {
        if (oldBarCount < 1 || oldBarCount == newBarCount)
        {
            DoHpBar(newBarCount,targetValue,dir);
            lastHpBarCount = newBarCount;
            yield break;
        }
        
        if (oldBarCount < newBarCount)
        {
            while (oldBarCount <= newBarCount)
            {
                if (oldBarCount == newBarCount)
                    DoHpBar(oldBarCount,targetValue,dir);
                else
                    DoHpBar(oldBarCount,1,dir);
                lastHpBarCount = oldBarCount;
                yield return new WaitForSecondsRealtime(0.2f);
                oldBarCount++;
            }
        }
        else if (oldBarCount > newBarCount)
        {
            while (oldBarCount >= newBarCount)
            {
                if (oldBarCount == newBarCount)
                    DoHpBar(oldBarCount,targetValue,dir);
                else
                    DoHpBar(oldBarCount,0,dir);
                oldBarCount--;
                lastHpBarCount = oldBarCount;
                yield return new WaitForSecondsRealtime(0.2f);
            }
        }
        
    }
    void DoHpBar(int hpBarCount,float value,int dir)
    {
        slider.DOKill(true);
        LerpSlider.DOKill(true);
        if (roleController.IsPlayer)
        {
            slider.DOValue(value, 0.2f);
            LerpSlider.DOValue(value, 0.2f);
        }
        else
        {
            if (hpBarCount > 1)
            {
                topHpBarImg.color = GetHpBarColor(hpBarCount);
                downHpBarImg.color = GetHpBarColor(hpBarCount - 1);
                if (dir < 0)
                {
                    if (value > slider.value)
                        slider.value = 1;
                }
                else
                {
                
                }
                slider.DOValue(value, 0.2f);
                LerpSlider.value = 1f;
            }
            else
            {
                downHpBarImg.color = GetHpBarColor(hpBarCount);
                slider.DOValue(0, 0.2f);
                if (dir < 0)
                {
                    if (value > LerpSlider.value)
                        LerpSlider.value = 1;
                }
                else
                {
                
                }
                LerpSlider.DOValue(value, 0.2f);
            }
        }

        if (BattleManager.Inst.RuntimeData.ShowEnemyHpText)
        {
            hpText.text = ((int)roleController.CurrentHp).ToString();
        }
        else
        {
            hpText.text = "";
        }
    }
    
    Color GetHpBarColor(int count)
    {
        if (roleController.IsPlayer)
        {
            return Color.green;
        }
        switch (count)
        {
            case 1:
                return Color.red;
                break;
            case 2:
                return Color.yellow;
                break;
            case 3:
                return Color.blue;
                break;
            case 4:
                return Color.cyan;
                break;
            case 5:
                return Color.green;
                break;
        }

        return Color.white;
    }
    
}
