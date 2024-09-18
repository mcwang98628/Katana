using System;
using System.Collections.Generic;
using FlexFramework.Excel;
using UnityEngine;



public partial class DataManager : TSingleton<DataManager>
{
    private string AttributesDataCSVPath = "Assets/AssetsPackage/Table/CSV_Attributes.csv";

    private Dictionary<AttributeType, Dictionary<int, TableAttributesData>> TableAttributesDatas =
        new Dictionary<AttributeType, Dictionary<int, TableAttributesData>>();
    
    void LoadAttributesData()
    {
        initNumber++;
        ResourcesManager.Inst.GetAsset<TextAsset>(AttributesDataCSVPath, delegate(TextAsset infoText)
        {
            var cpDataCSV = Document.Load(infoText.text);
            for (int i = 1; i < cpDataCSV.Count; i++)
            {
                if (cpDataCSV[i].Count < 2)
                    continue;

                var attType = (AttributeType) Enum.Parse(typeof(AttributeType), cpDataCSV[i][0].Convert<string>());
                int lv = cpDataCSV[i][1].Convert<int>();
                float value = cpDataCSV[i][2].Convert<float>();
                int price = cpDataCSV[i][3].Convert<int>();
                TableAttributesData attributesData = new TableAttributesData()
                {
                    AttributeType = attType,
                    Lv = lv,
                    Value = value,
                    Price = price,
                };

                if (!TableAttributesDatas.ContainsKey(attType))
                    TableAttributesDatas.Add(attType,new Dictionary<int, TableAttributesData>());
                
                if (!TableAttributesDatas[attType].ContainsKey(lv))
                    TableAttributesDatas[attType].Add(lv,attributesData);
                else
                    Debug.LogError("错误？？？？？？");
            }
            initNumber--;
        });
    }

    public TableAttributesData GetTableAttributesData(AttributeType attributeType,int level)
    {
        if (!TableAttributesDatas.ContainsKey(attributeType))
            return null;
        if (!TableAttributesDatas[attributeType].ContainsKey(level))
            return null;

        return TableAttributesDatas[attributeType][level];
    }
    
}

public class TableAttributesData
{
    public AttributeType AttributeType;
    public int Lv;
    public float Value;
    public int Price;
}


