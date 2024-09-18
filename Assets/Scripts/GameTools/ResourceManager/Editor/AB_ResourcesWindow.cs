using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;


public class AB_ResourcesWindow : OdinEditorWindow
{
    [MenuItem("GameTools/AB_Resources")]
    private static void OpenWindow()
    {
        GetWindow<AB_ResourcesWindow>().Show();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        initAbConfig();
    }

    #region Config

    [ReadOnly]
    [Required]
    [SerializeField]
    [TabGroup("Tabs","Config")]
    [LabelText("AB包 路径表")]
    [GUIColor(0,1,1,1f)]
    private ABConfig AbConfig = null;
    
    [LabelText("文件夹列表")]
    [TabGroup("Tabs/Config/Group","文件夹")]
    public List<ABConfig.FileDirABName> AllFileDirAB ;
    [LabelText("GameObject列表")]
    [TabGroup("Tabs/Config/Group","单独Prefab")]
    public List<string> AllPrefabPath ;
    
    void initAbConfig()
    {
        AbConfig = AssetDatabase.LoadAssetAtPath<ABConfig>("Assets/AssetsPackage/ABConfig.asset");
        AllPrefabPath = AbConfig.m_AllPrefabPath;
        AllFileDirAB = AbConfig.m_AllFileDirAB;
    }

    [LabelText("Build-AssetBundle")]
    [TabGroup("Tabs","Config")]
    [Button(ButtonSizes.Large)][GUIColor(1,1,0,1)]
    public void BuildAssetBundle()
    {
        BundleEditor.Build();
    }
    [LabelText("SaveConfig")]
    [TabGroup("Tabs","Config")]
    [Button(ButtonSizes.Large)][GUIColor(1,1,0,1)]
    public void SaveConfig()
    {
        EditorUtility.SetDirty(AbConfig);
    }
    
    // [LabelText("Clear-AssetBundleName")]
    // [TabGroup("Tabs","Config")]
    // [Button(ButtonSizes.Large)][GUIColor(1,1,0,1)]
    // public void ClearABName()
    // {
    //     BundleEditor.GetABData();
    //     BundleEditor.ClearABName();
    // }
    //

    #endregion


    #region 引用关系


    [LabelText("刷新")]
    [InfoBox("需要打好AssetBundle包之后才可以刷新!")]
    [TabGroup("Tabs","引用关系")]
    [Button(ButtonSizes.Large)][GUIColor(0,1,1,1)]
    public void RefreshQuoteConfig()
    {
        initQuote();
        assetsDependceData = LoadAssetsDependceDataXML();
        assetsCitedData = GetAssetsCitedDatas();
    }
    
    [TabGroup("Tabs","引用关系")]
    [LabelText("资源的引用情况")]
    [ShowInInspector] [ReadOnly]
    BundleEditor.AssetsDependceDataXML assetsDependceData = new BundleEditor.AssetsDependceDataXML();
    
    [TabGroup("Tabs","引用关系")]
    [LabelText("资源被引用情况")]
    [ShowInInspector] [ReadOnly]
    List<AssetsCitedData> assetsCitedData = new List<AssetsCitedData>();

    
    [TabGroup("Tabs","引用关系")]
    [LabelText("AB包 体积")]
    [ShowInInspector] [ReadOnly]
    [HideLabel]    
    List<AbSizeData> abSize = new List<AbSizeData>();
    
    Dictionary<string,AbSizeData> abSizeDic = new Dictionary<string, AbSizeData>();

    [Serializable] 
    private class AbSizeData
    {
        [LabelText("AB包名")][GUIColor(0.8f,1,0.2f)]
        public string Name;
        [LabelText("AB包体积")][GUIColor(0.1f,1,0.2f)]
        public string SizeStr;
        [LabelText("引用的文件")][GUIColor(0.1f,1,0.6f)]
        public List<string> AbFiles = new List<string>();
        [LabelText("AB包依赖")][GUIColor(0.1f,0.8f,0.5f)]
        public List<AbSizeData> AbDependce = new List<AbSizeData>();
        
        [HideInInspector]
        public long Size;

        public AbSizeData(string name,long size,string sizeStr)
        {
            Name = name;
            Size = size;
            SizeStr = sizeStr;
        }
    }

    private class FileSizeData
    {
        public string Name;
        
        public long Size;

        public string ABName;
    }
    void initQuote()
    {
        AssetBundleConfig quoteConfig = ResourcesManager.Inst.LoadAssetBundleConfig();
        abSize.Clear();
        abSizeDic.Clear();
        string unityProjectPath = Application.dataPath;
        unityProjectPath = unityProjectPath.Substring(0, unityProjectPath.Length - 6);
        System.IO.FileInfo f;

        foreach (ABBase abBase in quoteConfig.ABList)
        {
            //AB size
            if (!abSizeDic.ContainsKey(abBase.ABName))
            {
                try
                {
                    f = new FileInfo(Application.streamingAssetsPath + "/" + abBase.ABName);
                    var data = new AbSizeData(abBase.ABName, f.Length,getFileSize(f.Length));
                    abSizeDic.Add(abBase.ABName, data);
                    abSize.Add(data);
                }
                catch (Exception e)
                {
                    Debug.LogError("ERROR!!!: "+abBase.ABName);
                    continue;
                }
            }
            //AB size
        }

        abSize.Sort((lData, fData) =>
        {
            if (lData.Size>fData.Size)
            {
                return -1;
            }
            else if (lData.Size==fData.Size)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        });

        List<FileSizeData> fileSizeDatas = new List<FileSizeData>();
        foreach (ABBase abBase in quoteConfig.ABList)
        {
            //AB File
            f = new FileInfo(unityProjectPath + abBase.Path);
            fileSizeDatas.Add(new FileSizeData(){Name = abBase.AssetName,Size = f.Length,ABName = abBase.ABName});
            //AB File
        }
        fileSizeDatas.Sort((L, R) =>
        {
            if (L.Size>R.Size)
            {
                return -1;
            }
            else if (L.Size==R.Size)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        });
        for (int i = 0; i < fileSizeDatas.Count; i++)
        {
            abSizeDic[fileSizeDatas[i].ABName].AbFiles.Add(fileSizeDatas[i].Name + "      " + getFileSize(fileSizeDatas[i].Size));
        }
        
        
        foreach (ABBase abBase in quoteConfig.ABList)
        {
            //AB Dependce
            if (abBase.ABDependce.Count>0)
            {
                
                foreach (string dep in abBase.ABDependce)
                {
                    if (!abSizeDic[abBase.ABName].AbDependce.Contains(abSizeDic[dep]))
                    {
                        abSizeDic[abBase.ABName].AbDependce.Add(abSizeDic[dep]);
                    }
                }
            }
            //AB Dependce
        }
    }

    string getFileSize(long size)
    {
        float kb = size / 1024f;
        if (kb>1024f)
        {
            return (kb / 1024f).ToString("0.00") + "MB";
        }
        else
        {
            return (kb).ToString("0.00")  + "KB";
        }
    }
    private BundleEditor.AssetsDependceDataXML LoadAssetsDependceDataXML()
    {
        string assetsDependceXmlPath = "Assets/AssetsPackage/AssetsDependceXML.xml";
        TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(assetsDependceXmlPath);
        MemoryStream stream = new MemoryStream(textAsset.bytes);
        // BinaryFormatter bf = new BinaryFormatter();
        XmlSerializer assetsDependceXml = new XmlSerializer(typeof(BundleEditor.AssetsDependceDataXML));
        BundleEditor.AssetsDependceDataXML assetsDependceDataXML = (BundleEditor.AssetsDependceDataXML)assetsDependceXml.Deserialize(stream);
         // = (BundleEditor.AssetsDependceDataXML) bf.Deserialize(stream);
        stream.Close();
        return assetsDependceDataXML;
    }

    //获取资源被引用数据
    private List<AssetsCitedData> GetAssetsCitedDatas()
    {
        Dictionary<string,AssetsCitedData> assetsCited = new Dictionary<string,AssetsCitedData>();
        foreach (BundleEditor.AssetsDependceData dependceData in assetsDependceData.List)
        {
            foreach (string dependceAsset in dependceData.DependceAssets)
            {
                if (!assetsCited.ContainsKey(dependceAsset))
                {
                    
                    assetsCited.Add(dependceAsset,new AssetsCitedData(){AssetName = dependceAsset,InABName = dependceData.ABName});
                }

                if (!assetsCited[dependceAsset].ABName.Contains(dependceData.ABName))
                {
                    assetsCited[dependceAsset].ABName.Add(dependceData.ABName);
                }
                if (!assetsCited[dependceAsset].AssetsName.Contains(dependceData.AssetName))
                {
                    assetsCited[dependceAsset].AssetsName.Add(dependceData.AssetName);
                }
            }
        }

        return assetsCited.Values.ToList();
    }

    [LabelText("资源被引用数据")]
    public class AssetsCitedData
    {
        public string AssetName;
        public string InABName;
        public List<string> ABName = new List<string>();
        public List<string> AssetsName = new List<string>();
    }

    #endregion
    
    
    
    
    
}
