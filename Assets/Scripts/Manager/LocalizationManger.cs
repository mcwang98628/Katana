using System;
using System.Collections;
using System.Collections.Generic;
using FlexFramework.Excel;
using UnityEngine;

public class LocalizationManger : TSingleton<LocalizationManger>
{
    private SystemLanguage currentLanguageType;
    public SystemLanguage CurrentLanguageType => currentLanguageType;
    
    //本地化语言
    public Dictionary<string,string> Language = new Dictionary<string, string>();
    
    private string LanguageCSVPath = "Assets/AssetsPackage/Table/CSV_Language.csv";
   
    
    // public LocalizationManger()
    // {
    //     Init();
    // }

    public bool IsInit => isInit;
    private bool isInit;
    public void Init()
    {
        if (isInit)
        {
            return;
        }
        
        LoadLanguage();
    }


    void LoadLanguage()
    {
        //如果没有主动设置过语言 ，则按照手机系统语言来设置。如果游戏不包含目标语言 则按照英语显示。
        if (!PlayerPrefs.HasKey("IsSetLanguage") || !PlayerPrefs.HasKey("GameLanguage"))
        {
            PlayerPrefs.SetInt("GameLanguage",(int)Application.systemLanguage);
        }
        currentLanguageType = (SystemLanguage)PlayerPrefs.GetInt("GameLanguage");
        
        int column = 0;
        switch (currentLanguageType)
        {
            case SystemLanguage.Chinese:
                column = 1;
                break;
            case SystemLanguage.ChineseSimplified:
                column = 1;
                break;
            case SystemLanguage.ChineseTraditional: 
                column = 6;
                break;
            case SystemLanguage.English:
                column = 2;
                break;
            case SystemLanguage.Portuguese:
                column = 4;
                break;
            case SystemLanguage.Russian:
                column = 5;
                break;
            case SystemLanguage.Indonesian:
                column = 3;
                break;
            default:
                column = 2;
                break;
        }
        

        Language.Clear();
        
        isInit = false;
        ResourcesManager.Inst.GetAsset<TextAsset>(LanguageCSVPath, delegate(TextAsset languageCSV)
        {
            Document languageInfo = Document.Load(languageCSV.text);
            //跳过第一行
            for (int i = 1; i < languageInfo.Count; i++)
            {
                if (languageInfo[i].Count <= 1)
                {
                    continue;
                }
                // Debug.LogError(i);
                try
                {
                    Language.Add(languageInfo[i][0].String,languageInfo[i][column].String);
                }
                catch (Exception e)
                {
                    Debug.LogError(currentLanguageType);
                    Debug.LogError(languageInfo[i][0].String);
                    Debug.LogError(e);
                    throw;
                }
            }
            
            
            isInit = true;
        });
        // TextAsset languageCSV = ResourcesManager.Inst.GetAsset<TextAsset>(LanguageCSVPath);
        // Document languageInfo = Document.Load(languageCSV.text);
        // //跳过第一行
        // for (int i = 1; i < languageInfo.Count; i++)
        // {
        //     if (languageInfo[i].Count <= 1)
        //     {
        //         continue;
        //     }
        //     // Debug.LogError(i);
        //     try
        //     {
        //         Language.Add(languageInfo[i][0].String,languageInfo[i][column].String);
        //     }
        //     catch (Exception e)
        //     {
        //         Debug.LogError(languageInfo[i][0].String);
        //         Debug.LogError(e);
        //         throw;
        //     }
        // }
    }

    public void SetLanguage(SystemLanguage targetLanguage)
    {
        if (!PlayerPrefs.HasKey("IsSetLanguage"))
            PlayerPrefs.SetInt("IsSetLanguage", 1);
        PlayerPrefs.SetInt("GameLanguage",(int)targetLanguage);
        LoadLanguage();
        EventManager.Inst.DistributeEvent(EventName.OnUpdateLanguage);
    }
    
    
    public string GetText(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return "";
        }

        key = key.Replace(" ", "");
        key = key.Replace("\n", "");
        
        
        if (Language.ContainsKey(key))
        {
            return Language[key].Replace("\\n","\n");
        }
        else
        {
            return "*"+key+"*";
        }
    }
}
