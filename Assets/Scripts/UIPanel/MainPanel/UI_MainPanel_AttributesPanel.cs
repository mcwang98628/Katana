using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainPanel_AttributesPanel : MonoBehaviour
{
    [SerializeField]
    private Text currentAttackPower;
    [SerializeField]
    private Text nextAttackPower;
    
    [SerializeField]
    private Text currentMaxHp;
    [SerializeField]
    private Text nextMaxHp;

    [SerializeField]
    private Text currentCrit;
    [SerializeField]
    private Text nextCrit;

    [SerializeField]
    private Text attackPowerBtnText;
    [SerializeField]
    private Text maxHpBtnText;
    [SerializeField]
    private Text critBtnText;

    private void Awake()
    {
        UpdateText();
    }

    void UpdateText()
    {
        var currentAttack = ArchiveManager.Inst.ArchiveData.attributesArchiveData.GetCurrentAttributeData(AttributeType.AttackPower);
        var nextAttack = ArchiveManager.Inst.ArchiveData.attributesArchiveData.GetNextLvAttributeData(AttributeType.AttackPower);
        var currentMaxHp = ArchiveManager.Inst.ArchiveData.attributesArchiveData.GetCurrentAttributeData(AttributeType.MaxHp);
        var nextMaxHp = ArchiveManager.Inst.ArchiveData.attributesArchiveData.GetNextLvAttributeData(AttributeType.MaxHp);
        var currentCrit = ArchiveManager.Inst.ArchiveData.attributesArchiveData.GetCurrentAttributeData(AttributeType.CriticalProbability);
        var nextCrit = ArchiveManager.Inst.ArchiveData.attributesArchiveData.GetNextLvAttributeData(AttributeType.CriticalProbability);

        var heroData = DataManager.Inst.GetHeroUpgradeData(1101, 1, 0).Value;
        
        currentAttackPower.text = $"{(int) currentAttack.Value + heroData.AttackPower}";
        nextAttackPower.text = $"{(int) nextAttack.Value + heroData.AttackPower}";
        this.currentMaxHp.text = $"{(int) currentMaxHp.Value + heroData.MaxHp}";
        this.nextMaxHp.text = $"{(int) nextMaxHp.Value + heroData.MaxHp}";
        this.currentCrit.text = currentCrit.Value.ToString("0.00");
        this.nextCrit.text = nextCrit.Value.ToString("0.00");
        
        attackPowerBtnText.text = nextAttack != null?$"{nextAttack.Price}G 升级":"已满级";
        maxHpBtnText.text = nextAttack != null?$"{nextMaxHp.Price}G 升级":"已满级";
        critBtnText.text = nextAttack != null?$"{nextCrit.Price}G 升级":"已满级";
    }

    public void OnAttackPowerBtnClick()
    {
        UIManager.Inst.Tips.ShowText("功能暂时关闭");
        return;
        var err = ArchiveManager.Inst.ArchiveData.attributesArchiveData.UpgradeAttribute(AttributeType.AttackPower);
        switch (err)
        {
            case ArchiveErrorType.NoError:
                break;
            case ArchiveErrorType.DiamondShortage:
                UIManager.Inst.Tips.ShowText("钱不够。。。");
                break;
            case ArchiveErrorType.MaxLevel:
                UIManager.Inst.Tips.ShowText("等级满。。。");
                break;
        }
        UpdateText();
    }

    public void OnMaxHpBtnClick()
    {
        UIManager.Inst.Tips.ShowText("功能暂时关闭");
        return;
        var err = ArchiveManager.Inst.ArchiveData.attributesArchiveData.UpgradeAttribute(AttributeType.MaxHp);
        
        switch (err)
        {
            case ArchiveErrorType.NoError:
                break;
            case ArchiveErrorType.DiamondShortage:
                UIManager.Inst.Tips.ShowText("钱不够。。。");
                break;
            case ArchiveErrorType.MaxLevel:
                UIManager.Inst.Tips.ShowText("等级满。。。");
                break;
        }
        UpdateText();
    }

    public void OnCritBtnClick()
    {
        UIManager.Inst.Tips.ShowText("功能暂时关闭");
        return;
        var err = ArchiveManager.Inst.ArchiveData.attributesArchiveData.UpgradeAttribute(AttributeType.CriticalProbability);
        
        switch (err)
        {
            case ArchiveErrorType.NoError:
                break;
            case ArchiveErrorType.DiamondShortage:
                UIManager.Inst.Tips.ShowText("钱不够。。。");
                break;
            case ArchiveErrorType.MaxLevel:
                UIManager.Inst.Tips.ShowText("等级满。。。");
                break;
        }
        UpdateText();
    }
    
    
    
    
}
