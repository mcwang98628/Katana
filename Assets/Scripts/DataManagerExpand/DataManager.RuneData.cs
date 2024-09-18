using System.Collections.Generic;
using FlexFramework.Excel;
using UnityEngine;

public partial class DataManager : TSingleton<DataManager>
{
    private string RuneSlotData = "Assets/AssetsPackage/Table/CSV_RuneSlotData.csv";

    public Dictionary<int,RuneSlotData> RuneSlotDatas = new Dictionary<int, RuneSlotData>();
    
    public void InitRuneSlotData()
    {
        initNumber++;
        ResourcesManager.Inst.GetAsset<TextAsset>(RuneSlotData, delegate(TextAsset asset)
        {
            var doc = Document.Load(asset.text);
            for (int i = 1; i < doc.Count; i++)
            {
                RuneSlotData slotData = new RuneSlotData(
                    doc[i][0].Convert<int>(),
                    doc[i][1].Convert<int>()
                );
                RuneSlotDatas.Add(slotData.SlotId,slotData);
            }

            initNumber--;
        });
    }
    
}