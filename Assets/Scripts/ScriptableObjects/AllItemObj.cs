using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FlexFramework.Excel;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "GameItem/AllItemObj")]
public class AllItemObj : ScriptableObject
{
    public List<ItemScriptableObject> objs = new List<ItemScriptableObject>();

    
#if UNITY_EDITOR

    [Button("刷新并生成Item表")]
    public void OnBtnClick()
    {
        GetAllItemScriptableObject();
        var allItemData = AutoGenerateItemsCSVTab();
        AutoGenerateUnlockItemCSVTab(allItemData);
        AssetDatabase.Refresh();
        Debug.Log("刷新");
    }
    
    public void GetAllItemScriptableObject()
    {
        string[] itemGuids = UnityEditor.AssetDatabase.FindAssets("t:ItemScriptableObject");
        objs.Clear();
        for (int i = 0; i < itemGuids.Length; i++)
        {
            var path = UnityEditor.AssetDatabase.GUIDToAssetPath(itemGuids[i]);
            var item = UnityEditor.AssetDatabase.LoadAssetAtPath<ItemScriptableObject>(path);
            
            objs.Add(item);
        }
    }
    private string itemCSVPath => Application.dataPath + "/AssetsPackage/Table/CSV_Items.csv";
    private string itemCSVUnityPath => "Assets/AssetsPackage/Table/CSV_Items.csv";
    public Dictionary<string,int> AutoGenerateItemsCSVTab()
    {
        Dictionary<string,int>itemTabDatas = new Dictionary<string, int>();
        TextAsset infoText = AssetDatabase.LoadAssetAtPath<TextAsset>(itemCSVUnityPath);
        if (infoText != null)
        {
            var dataCSV = Document.Load(infoText.text);
            for (int i = 1; i < dataCSV.Count; i++)
            {
                if (string.IsNullOrEmpty(dataCSV[i][0]))
                {
                    break;
                }
                int id = dataCSV[i][0].Convert<int>();
                string faileName = dataCSV[i][1];
                itemTabDatas.Add(faileName,id);
            }
        }

        //找出最大id
        int maxId = -1;
        //已经被删除了的item
        List<string> destroyitem = new List<string>();
        foreach (KeyValuePair<string,int> itemData in itemTabDatas)
        {
            if (!checkItemName(itemData.Key))
            {//如果表里有数据，但实际上已经删除了。
                destroyitem.Add(itemData.Key);
            }

            if (maxId<itemData.Value)
            {
                maxId = itemData.Value;
            }
        }
        //删除已经不存在的item数据
        foreach (string s in destroyitem)
        {
            Debug.LogError($"已被删除的:{s}");
            itemTabDatas.Remove(s);
        }
        
        //找出没有被记录过的item
        foreach (ItemScriptableObject itemSobj in objs)
        {
            if (!itemTabDatas.ContainsKey(itemSobj.name))
            {
                ++maxId;
                itemTabDatas.Add(itemSobj.name,maxId);
            }
        }
        
        //生成新表
        StringBuilder sbText = new StringBuilder();
        sbText.Append("Id,ItemFileName");
        foreach (KeyValuePair<string,int> itemData in itemTabDatas)
        {
            sbText.Append("\n"+ itemData.Value + "," + itemData.Key);
        }

        WriterTab(itemCSVPath,sbText.ToString());
        

        return itemTabDatas;
    } 
    
    //检查item名字 是否还存在
    private bool checkItemName(string itemName)
    {
        foreach (ItemScriptableObject itemSobj in objs)
        {
            if (itemName == itemSobj.name)
            {
                return true;
            } 
        }
        return false;
    }


    private string chapterItemUnLockPath => Application.dataPath + "/AssetsPackage/Table/CSV_ChapterItemUnLock.csv";
    private string defaultUnLockItemPath => Application.dataPath + "/AssetsPackage/Table/CSV_UnLockItem.csv";
    public void AutoGenerateUnlockItemCSVTab(Dictionary<string,int> itemIdData)
    {
        List<ItemScriptableObject> defaultUnlockList = new List<ItemScriptableObject>();
        List<ItemScriptableObject> chapterUnlockList = new List<ItemScriptableObject>();
        for (int i = 0; i < objs.Count; i++)
        {
            if (objs[i].DefaultUnLock)
            {
                defaultUnlockList.Add(objs[i]);
            }
            else
            {
                chapterUnlockList.Add(objs[i]);
            }
        }
        StringBuilder defaultUnlockSb = new StringBuilder();
        for (int i = 0; i < defaultUnlockList.Count; i++)
        {
            defaultUnlockSb.Append($"{itemIdData[defaultUnlockList[i].name]},{defaultUnlockList[i].name}\n");
        }
        StringBuilder chapterUnlockSb = new StringBuilder();
        chapterUnlockSb.Append($"ChapterId,LevelId,RoomId,UnLockItemId\n");
        for (int i = 0; i < chapterUnlockList.Count; i++)
        {
            chapterUnlockSb.Append($"{chapterUnlockList[i].ChapterId},{chapterUnlockList[i].LevelId},{chapterUnlockList[i].RoomId},{itemIdData[chapterUnlockList[i].name]}\n");
        }

        WriterTab(chapterItemUnLockPath,chapterUnlockSb.ToString());
        WriterTab(defaultUnLockItemPath,defaultUnlockSb.ToString());
    }

    public static void WriterTab(string path,string scvStr)
    {
        try
        {
            File.Delete(path);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        using (StreamWriter sw = new StreamWriter(path,false,Encoding.UTF8))
        {
            sw.WriteLine(scvStr);
            sw.Close();
        }
    }


    [Button("ItemNameIdTable")]
    public void OnItemNameIdTableBtnClick()
    {
        GenerateItemNameIdTable();

    }
    public void GenerateItemNameIdTable()
    {
        string testTablePath = "Assets/道具名字Id对照表.csv";
        
        //生成新表
        StringBuilder sbText = new StringBuilder();
        sbText.Append("Id,ItemName");
        
        foreach (var itemData in DataManager.Inst.AllItemObj)
            sbText.Append("\n"+ itemData.Key + "," + LocalizationManger.Inst.GetText(itemData.Value.Name));

        WriterTab(testTablePath,sbText.ToString());
    }

    
#endif
}
