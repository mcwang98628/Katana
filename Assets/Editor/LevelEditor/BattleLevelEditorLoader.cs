using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Linq;

namespace LevelEditor
{
    public class BattleLevelEditorLoader
    {
        //摆放物品prefab默认显示路径
        public const string loadContentDefaultPath = "Assets/Arts/Models/Chapter1";

        //每类摆放物品prefab对应文件夹名字
        private Dictionary<LevelContentType,string> contentDirectoryNameDic = new Dictionary<LevelContentType,string>()
        {
            { LevelContentType.Collision, "Models/CollisionDecoration"},
            { LevelContentType.NoCollision, "Models/NoCollisionDecoration"},
            { LevelContentType.Breakable, "Models/BreakableObj"},
            { LevelContentType.OtherModule, "Modules" }
        };

        public Dictionary<LevelContentType,List<string>> contentNameDic;
        private Dictionary<string,string> contentPathDic;
        private Dictionary<string,Texture2D> contentTextureDic;

        public string GetContentPathByName(string contentName)
        {
            if(contentPathDic != null && contentPathDic.ContainsKey(contentName))
                return contentPathDic[contentName];
            return null;
        }
        public Texture2D GetContentTextureByName(string contentName)
        {
            if(contentTextureDic != null && contentTextureDic.ContainsKey(contentName))
                return contentTextureDic[contentName];
            return null;
        }

        public void LoadContent(string loadContentPath)
        {
            contentPathDic = new Dictionary<string,string>();
            contentTextureDic = new Dictionary<string,Texture2D>();
            contentNameDic = new Dictionary<LevelContentType,List<string>>();

            loadContentPath = loadContentPath.Replace('\\','/');

            foreach(LevelContentType contentType in System.Enum.GetValues(typeof(LevelContentType)))
            {
                contentNameDic[contentType] = new List<string>();

                string contentDirectoryName = loadContentPath + "/" + contentDirectoryNameDic[contentType];
                if(Directory.Exists(contentDirectoryName))
                {
                    string[] contentFullPaths = Directory.GetFiles(contentDirectoryName,"*.prefab",SearchOption.AllDirectories);
                    for(int i = 0; i < contentFullPaths.Length; i++)
                    {
                        //GameObject prefab = PrefabUtility.LoadPrefabContents(contentFullPaths[i]);
                        if(!string.IsNullOrEmpty(contentFullPaths[i]))
                        {
                            string contentName = contentFullPaths[i].Split('/').LastOrDefault().Split('.').FirstOrDefault();

                            if(contentPathDic.ContainsKey(contentName))
                                Debug.LogError("Content Prefab命名重复 : " + contentName);

                            contentNameDic[contentType].Add(contentName);
                            contentPathDic[contentName] = contentFullPaths[i];
                            contentTextureDic[contentName] = GetPreviewTexture(contentFullPaths[i]);
                        }
                    }
                }
                else
                    Debug.LogError("Content Prefab路径不存在 : " + contentType);
            }
        }

        private Texture2D GetPreviewTexture(string contentFullPath)
        {
            contentFullPath = contentFullPath.Replace('\\','/');

            GameObject contentAssetObj = AssetDatabase.LoadAssetAtPath<GameObject>(contentFullPath);
            if(contentAssetObj != null)
            {
                Texture2D texture = AssetPreview.GetAssetPreview(contentAssetObj);
                return texture;

                ////GameObject contentInstanceObj = GameObject.Instantiate(contentAssetObj);
                //GameObject contentInstanceObj = PrefabUtility.InstantiatePrefab(contentAssetObj) as GameObject;
                //contentInstanceObj.transform.position = Vector3.zero;
                //contentInstanceObj.transform.rotation = Quaternion.identity;
                //contentInstanceObj.transform.localScale = Vector3.one;

                //Texture2D texture = null;
                //int index = 0;
                //while(texture == null && index < 10000000)
                //{
                //    //index++;
                //    texture = AssetPreview.GetAssetPreview(contentInstanceObj);
                //}
                //return texture;

                ////if(texture != null)
                ////{
                ////    Texture2D newTexture = new Texture2D(texture.width,texture.height);
                ////    newTexture.SetPixels(texture.GetPixels());
                ////    newTexture.Apply();
                ////    return newTexture;
                ////}
            }
            return AssetPreview.GetMiniTypeThumbnail(typeof(GameObject));
        }
    }
}