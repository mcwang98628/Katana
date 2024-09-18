using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ALG;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ResourcesManager :TSingleton<ResourcesManager>
{
    public bool EditorMode = true;
    
    private Dictionary<string,AssetBundle> _assetBundles = new Dictionary<string, AssetBundle>();
    private AssetBundleConfig assetBundleConfig = null;


#region 初始化

// public ResourcesManager()
// {
//     Init();
// }

private bool isInit;
    public void Init()
    {
        if (isInit)
        {
            return;
        }

        isInit = true;
        if (!EditorMode)
        {
            assetBundleConfig = LoadAssetBundleConfig();
        }
    }
    
    /// <summary>
    /// 读取AssetBundle配置表
    /// 配置表打包时写入在 assetbundleconfig 包里 名字要固定。
    /// </summary>
    /// <returns>AssetBundle配置表</returns>
    public AssetBundleConfig LoadAssetBundleConfig()
    {
        string configPath = Application.streamingAssetsPath + "/assetbundleconfig";
        AssetBundle bundleConfig = AssetBundle.LoadFromFile(configPath);
        
        TextAsset textAsset = bundleConfig.LoadAsset<TextAsset>("AssetBundleConfigByte");
        MemoryStream stream = new MemoryStream(textAsset.bytes);
        BinaryFormatter bf = new BinaryFormatter();
        AssetBundleConfig assetBundleConfig = (AssetBundleConfig) bf.Deserialize(stream);
        stream.Close();
        bundleConfig.Unload(true);
        return assetBundleConfig;
    }

#endregion

    public void GetAsset<T>(string path,Action<T> callback) where T  : UnityEngine.Object
    {
#if UNITY_EDITOR
        if (EditorMode)
        {
            callback?.Invoke(EditorLoadAsset<T>(path));
        }
        else
#endif
        {
            AssetBundleLoadAsset<T>(path,callback);
        }
    }

    T EditorLoadAsset<T>(string path) where T  : Object
    {
#if UNITY_EDITOR
        return AssetDatabase.LoadAssetAtPath<T>(path);
#endif
        return null;
    }
    

#region AssetBundle Load

    void AssetBundleLoadAsset<T>(string path,Action<T> callBack) where T  : Object
    {
        string assetName = path.Remove(0, path.LastIndexOf("/")+1);
        ABBase abBase = FindAbBase(path);
        AsyncLoadAssetBundle(abBase, delegate(AssetBundle bundle)
        {
            if (bundle == null)
            {
                callBack?.Invoke(null);
                return;
            }
            
            var asset = bundle.LoadAsset<T>(assetName);
            callBack?.Invoke(asset);
        });
    }
    ABBase FindAbBase(string path)
    {
        uint crc = CRC32.GetCRC32(path);
        foreach (ABBase abBase in assetBundleConfig.ABList)
        {
            if (abBase.Crc == crc)
            {
                return abBase;
            }
        }
        return null;
    }
    
    // AssetBundle LoadAssetBundle(ABBase abBase)
    // {
    //     if (abBase == null)
    //         return null;
    //     
    //     //加载目标包所依赖的其他包。
    //     for (int i = 0; i < abBase.ABDependce.Count; i++)
    //     {
    //         if (_assetBundles.ContainsKey(abBase.ABDependce[i]))
    //         {
    //             continue;
    //         }
    //         string bundlePath = Application.streamingAssetsPath + "/" + abBase.ABDependce[i];
    //         AssetBundle ab = AssetBundle.LoadFromFile(bundlePath);
    //         _assetBundles.Add(abBase.ABDependce[i],ab);
    //     }
    //     
    //     if (_assetBundles.ContainsKey(abBase.ABName))
    //     {
    //         return _assetBundles[abBase.ABName];
    //     }
    //     else
    //     {
    //         AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + abBase.ABName);
    //         _assetBundles.Add(abBase.ABName,assetBundle);
    //         
    //         return assetBundle;
    //     }
    // }

    
    
    Dictionary<string,AssetBundleCreateRequest> _asyncLoadTask = new Dictionary<string, AssetBundleCreateRequest>();

    
    void AsyncLoadAssetBundle(ABBase abBase,Action<AssetBundle> callBack)
    {
        if (abBase == null)
        {
            callBack?.Invoke(null);
            return;
        }

        if (abBase.ABDependce.Count > 0)
        {
            bool isTriggerCallBack = false;
            //加载目标包所依赖的其他包。
            for (int i = 0; i < abBase.ABDependce.Count; i++)
            {
                string bundlePath = Application.streamingAssetsPath + "/" + abBase.ABDependce[i];
                AssetBundleAsyncLoad(bundlePath, delegate(AssetBundle bundle)
                {

                    //================================================================================================================
                    bool allLoadOver = true;
                    foreach (string abName in abBase.ABDependce)
                    {
                        string path = Application.streamingAssetsPath + "/" + abName;
                        if (!GetAssetBundleLoadIsDone(path))
                        {
                            allLoadOver = false;
                            break;
                        }
                    }

                    if (allLoadOver && !isTriggerCallBack)
                    {
                        isTriggerCallBack = true;
                        string path = Application.streamingAssetsPath + "/" + abBase.ABName;
                        AssetBundleAsyncLoad(path,callBack);
                    }
                    //================================================================================================================
                });
            }
        }
        else
        {
            //================================================================================================================
            string path = Application.streamingAssetsPath + "/" + abBase.ABName;
            AssetBundleAsyncLoad(path,callBack);
            //================================================================================================================
        }
    }
    
    void AssetBundleAsyncLoad(string bundlePath,Action<AssetBundle> callBack)
    {
        if (_asyncLoadTask.ContainsKey(bundlePath))
        {
            var bundleRequest = _asyncLoadTask[bundlePath];
            if (bundleRequest.isDone)
                callBack?.Invoke(bundleRequest.assetBundle);
            else
                bundleRequest.completed += operation =>
                {
                    callBack?.Invoke(bundleRequest.assetBundle);
                };
            
            return;
        }
        
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(bundlePath);
        _asyncLoadTask.Add(bundlePath,request);

        request.completed += operation =>
        {
            callBack?.Invoke(request.assetBundle);
        };
    }

    AssetBundle GetAssetBundle(string bundlePath)
    {
        if (GetAssetBundleLoadIsDone(bundlePath) && _asyncLoadTask[bundlePath].isDone)
        {
            return _asyncLoadTask[bundlePath].assetBundle;
        }
        return null;
    }

    bool GetAssetBundleLoadIsDone(string bundlePath)
    {
        if (!_asyncLoadTask.ContainsKey(bundlePath))
        {
            // Debug.LogError($"没加载过这个包 {bundlePath}");
            return false;
        }
        return _asyncLoadTask[bundlePath].isDone;
    }
    

#endregion


}
