using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UI_MainPanel_ShopPanel : MonoBehaviour
{
    [SerializeField]
    private Text Score;
    [SerializeField]
    private UI_MainPanel_Tips tips;

    int minScore = 30;
    int maxScore = 100;
    int chapterId = 0;
    
    
    void OnEnable()
    {
        var chapterClearanceDatas = ArchiveManager.Inst.ArchiveData.StatisticsData.ChapterClearanceDatas;
        for (int i = 10; i > 0; i--)
        {
            if (chapterClearanceDatas.ContainsKey(i) && chapterClearanceDatas[i].Count > 0)
            {
                maxScore = i * 100;
                minScore = maxScore - 100;
                if (minScore < 30)
                    minScore = 30;
                chapterId = i;
                break;
            }
        }

        //Score.text = $"Lv.{minScore/10} - Lv.{maxScore/10} {LocalizationManger.Inst.GetText("Score")}";

        Score.text = $"Lv.{minScore / 10} - Lv.{maxScore / 10} ";
        
        
        EventManager.Inst.DistributeEvent(TGANames.MainPanelShop);
    }

    //购买装备
    public void BuyEquipment()
    {
        if (ArchiveManager.Inst.ArchiveData.GlobalData.Diamond < 100)
        {
            tips.Show(UI_Asset.AssetType.Diamond);
            // UIManager.Inst.Tips.ShowText("NoDiamonds");
            return;
        }
        
        ArchiveManager.Inst.ChangeDiamond(-100);
        var qualityArr = Enum.GetNames(typeof(EquipmentQuality));
        EquipmentQuality equipmentQuality = (EquipmentQuality) Enum.Parse(typeof(EquipmentQuality), qualityArr[Random.Range(0, qualityArr.Length)]);
        Equipment equipment = EquipmentTool.RandomEquipment(Random.Range(minScore,maxScore), equipmentQuality);
        ArchiveManager.Inst.ArchiveData.equipmentArchiveData150.AddEquip(equipment);
        ArchiveManager.Inst.SaveArchive();
        
        
        UIManager.Inst.Open("InfoPanel",false,equipment);
        
        EventManager.Inst.DistributeEvent(TGANames.BuyEquipment,equipment);
    }
    
    
}
