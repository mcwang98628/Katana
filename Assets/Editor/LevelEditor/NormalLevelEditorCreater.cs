using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace LevelEditor
{
    public class NormalLevelEditorCreater
    {
        //房间模板默认路径
        public const string loadSampleDefaultPathName = "Assets/Arts/RoomPrefabs/Rooms/chapter 2/Chapter_2_Connect";
        //房间摆放物品路径
        public const string loadContentDefaultPath = "Assets/Arts/Models/Chapter4";
        //所有章节摆放物品名字集合<场景中物品名字,预设体名字>
        public Dictionary<string,string> contentNameDic =new Dictionary<string,string>()
        {
            {"BrokenPillar",    "BrokenPillar"},
            {"Coffin",          "Coffin"},
            {"Dandelion",       "Dandelion"},
            {"Dandelion2",      "Dandelion2"},
            {"Grave",           "Grave"},
            {"Lantern",         "Lantern"},
            {"Stone1",          "Stone1" },
            {"StoneShard",      "StoneShard"},
            {"Tree",            "Tree"},
            {"Tree2",           "Tree2"},
            {"Vase",            "Vase"},
            {"Door",            "Door"},
            {"Entrance_Z",      "Entrance"},
            {"Exit_Z",          "Exit"},
            {"Floor",           "Floor"},
            {"Pillar",          "Pillar"},
            {"Railing",         "Railing"},
            {"RailingPillar",   "RailingPillar"},
            {"UnderPillar",     "UnderPillar"},
            {"UnderWall",       "UnderWall"},
            {"Wall",            "Wall"}
            /*"RoomFrame",*/
        };

        private GameObject sampleGameObj;
        private string sampleLoadPathName;
        private Dictionary<string,GameObject> contentPrefabDic = new Dictionary<string,GameObject>();

        public void LoadSampleAndContent(string loadSamplePathName,string loadContentPath)
        {
            //隐藏场景已有物体
            GameObject[] sceneRootObjs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            for(int i = 0; i < sceneRootObjs.Length; i++)
            {
                if(sceneRootObjs[i].activeSelf)
                    sceneRootObjs[i].SetActive(false);
            }

            LoadSample(loadSamplePathName);
            if(sampleGameObj == null)
            {
                EditorUtility.DisplayDialog("关卡编辑器错误","模板房间加载失败","确定");
                Debug.LogError("模板房间加载失败");
                return;
            }

            LoadContent(loadContentPath);
            if(contentPrefabDic == null || contentPrefabDic.Count == 0)
            {
                EditorUtility.DisplayDialog("关卡编辑器错误","房间摆放预设加载失败","确定");
                Debug.LogError("房间摆放预设加载失败");
                return;
            }

            Debug.Log("查找到Connect Prefab Count: " + contentPrefabDic.Count);
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            foreach(var item in contentPrefabDic)
            {
                stringBuilder.AppendLine(item.Key);
            }
            Debug.Log(stringBuilder.ToString());
        }

        public void ReplaceContent()
        {
            if(sampleGameObj == null)
            {
                EditorUtility.DisplayDialog("关卡编辑器错误","模板房间未加载","确定");
                Debug.LogError("模板房间未加载");
                return;
            }

            Dictionary<GameObject,string> contentDic = new Dictionary<GameObject,string>();
            GetContentByName(sampleGameObj,contentDic);

            Debug.Log("查找到需替换物品数量 : " + contentDic.Count);

            if(contentDic.Count == 0)
                return;

            int replaceCount = 0;
            foreach(var item in contentDic)
            {
                if(ReplaceContentByName(item.Key,item.Value))
                    replaceCount++;
            }

            Debug.Log("成功替换物品数量 : " + replaceCount);

            foreach(var item in contentDic)
            {
                if(item.Key != null)
                    GameObject.DestroyImmediate(item.Key);
            }
            contentDic.Clear();
        }

        public void SaveNewSamplePrefab(int createPrefabChapter)
        {
            if(sampleGameObj == null)
            {
                EditorUtility.DisplayDialog("关卡编辑器错误","模板房间未加载","确定");
                Debug.LogError("模板房间未加载");
                return;
            }

            if(createPrefabChapter < 0)
            {
                EditorUtility.DisplayDialog("关卡编辑器错误","请输入正确的章节序号","确定");
                Debug.LogError("请输入正确的章节序号");
                return;
            }

            //Assets/Arts/RoomPrefabs/Rooms/chapter 2/Chapter_2_Connect
            //Assets/Arts/Models/Chapter2/Models/RoomFrame

            //Window路径下包含'\', Mac仅包含'/'
            sampleLoadPathName = sampleLoadPathName.Replace('\\','/');
            string saveName = sampleLoadPathName.Split('/').LastOrDefault();
            string savePath = sampleLoadPathName.Replace(saveName,"");

            if(saveName.Contains('_'))
            {
                string[] nameArr = saveName.Split('_');
                if(nameArr.Length >= 2)
                {
                    nameArr[1] = createPrefabChapter.ToString();
                    saveName = string.Join("_",nameArr);
                }
            }

            string[] pathArr = savePath.Split('/');
            for(int i = 0; i < pathArr.Length; i++)
            {
                if(pathArr[i].Contains("chapter "))
                    pathArr[i] = "chapter " + createPrefabChapter;
                if(pathArr[i].Contains("Chapter"))
                    pathArr[i] = "Chapter" + createPrefabChapter;
            }
            savePath = string.Join("/",pathArr);

            if(!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            string fullPath = string.Format("{0}/{1}",savePath,saveName);

            if(File.Exists(fullPath))
            {
                EditorUtility.DisplayDialog("关卡编辑器错误","同名文件已存在","确定");
                Debug.LogError("同名文件已存在，请手动删除 : " + fullPath);
                return;
            }

            bool savePrefabResult;
            PrefabUtility.SaveAsPrefabAsset(sampleGameObj,fullPath,out savePrefabResult);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            if(savePrefabResult)
                Debug.Log("保存成功 : " + fullPath);
            else
            {
                EditorUtility.DisplayDialog("关卡编辑器错误","保存失败","确定");
                Debug.LogError("保存失败 : " + fullPath);
            }
        }

        private void LoadSample(string loadSamplePathName)
        {
            sampleGameObj = null;
            loadSamplePathName = loadSamplePathName.Replace('\\','/');
            loadSamplePathName += ".prefab";
            sampleLoadPathName = loadSamplePathName;
            GameObject roomFrameSource = AssetDatabase.LoadAssetAtPath<GameObject>(loadSamplePathName);
            if(roomFrameSource == null)
            {
                EditorUtility.DisplayDialog("关卡编辑器错误","加载路径错误","确定");
                Debug.LogError("加载路径错误 : " + loadSamplePathName);
                return;
            }
            sampleGameObj = PrefabUtility.InstantiatePrefab(roomFrameSource) as GameObject;
            sampleGameObj.transform.position = Vector3.zero;
            sampleGameObj.transform.rotation = Quaternion.identity;
            sampleGameObj.transform.localScale = Vector3.one;

            sampleGameObj.transform.SetAsLastSibling();

            if(PrefabUtility.IsAnyPrefabInstanceRoot(sampleGameObj))
                PrefabUtility.UnpackPrefabInstance(sampleGameObj,PrefabUnpackMode.OutermostRoot,InteractionMode.AutomatedAction);
            if(PrefabUtility.IsAnyPrefabInstanceRoot(sampleGameObj))
                PrefabUtility.UnpackPrefabInstance(sampleGameObj,PrefabUnpackMode.OutermostRoot,InteractionMode.AutomatedAction);
        }

        private void LoadContent(string loadContentPath)
        {
            loadContentPath = loadContentPath.Replace('\\','/');
            contentPrefabDic = new Dictionary<string,GameObject>();
            if(Directory.Exists(loadContentPath))
            {
                string[] contentFullPaths = Directory.GetFiles(loadContentPath,"*.prefab",SearchOption.AllDirectories);
                for(int i = 0; i < contentFullPaths.Length; i++)
                {
                    string contentName = contentFullPaths[i];
                    contentName = contentName.Replace('\\','/');
                    contentName = contentName.Split('/').LastOrDefault().Split('.').FirstOrDefault();

                    GameObject contentSource = AssetDatabase.LoadAssetAtPath<GameObject>(contentFullPaths[i]);
                    if(contentSource != null)
                    {
                        if(contentPrefabDic.ContainsKey(contentName))
                            Debug.LogError("Content Name Repetitive : " + contentName);

                        contentPrefabDic[contentName] = contentSource;
                    }
                }
            }
            else
            {
                EditorUtility.DisplayDialog("关卡编辑器错误","Content Prefab路径不存在","确定");
                Debug.LogError("Content Prefab路径不存在 : " + loadContentPath);
            }
        }

        private void GetContentByName(GameObject root,Dictionary<GameObject,string> contentDic)
        {
            if(root == null || root.transform.childCount == 0)
                return;

            for(int i = 0; i < root.transform.childCount; i++)
            {
                GameObject child = root.transform.GetChild(i).gameObject;
                string name = child.name;
                name = name.Split(' ').FirstOrDefault();

                if(contentNameDic.ContainsKey(name))
                    contentDic.Add(child,name);

                if(root.transform.childCount > 0)
                    GetContentByName(child,contentDic);
            }
        }

        private bool ReplaceContentByName(GameObject contentOld, string contentName)
        {
            if(contentOld == null || !contentNameDic.ContainsKey(contentName))
            {
                Debug.LogError("替换物品信息错误 : " + contentOld + "/" + contentName);
                return false;
            }

            if(!contentPrefabDic.ContainsKey(contentNameDic[contentName]))
            {
                Debug.Log("物品Prefab不存在 : " + contentName + "/" + contentNameDic[contentName]);
                return false;
            }

            GameObject contentNew = PrefabUtility.InstantiatePrefab(contentPrefabDic[contentNameDic[contentName]]) as GameObject;
            contentNew.transform.SetParent(contentOld.transform.parent);
            contentNew.transform.position = contentOld.transform.position;
            contentNew.transform.rotation = contentOld.transform.rotation;
            contentNew.transform.localScale = contentOld.transform.localScale;
            if(contentNew.name != contentName)
                contentNew.name = contentName;
            if(contentNew.activeSelf != contentOld.activeSelf)
                contentNew.SetActive(contentOld.activeSelf);

            return true;
        }

    }
}