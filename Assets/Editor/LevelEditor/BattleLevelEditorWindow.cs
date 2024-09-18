using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine.SceneManagement;
namespace LevelEditor
{
    public class BattleLevelEditorWindow : EditorWindow
    {
        static BattleLevelEditorWindow windowInstance;
        [MenuItem("Tools/Battle Level Editor Window &2", priority = 100)]
        public static void ShowLevelEditorWindow()
        {
            if (windowInstance != null)
            {
                windowInstance.Close();
            }
            else
            {
                windowInstance = EditorWindow.GetWindow<BattleLevelEditorWindow>();
                windowInstance.Show();
            }
            //弹出窗口
             //EditorWindow.GetWindow<BattleLevelEditorWindow>(true, "战斗房编辑器").Show();

        }
        [MenuItem("Tools/RotateBy90 _n", priority = 100)]
        public static void RotateBy90()
        {
            Undo.RecordObjects(Selection.transforms, "Rotate a group.");
            for (int i = 0; i < Selection.gameObjects.Count(); i++)
            {
                Selection.gameObjects[i].transform.Rotate(0, 90, 0);
            }
        }
        [MenuItem("Tools/RotateByRandomDegrees _m", priority = 100)]
        public static void RotateByRandomDegrees()
        {
            Undo.RecordObjects(Selection.transforms, "Rotate a group.");
            for (int i = 0; i < Selection.gameObjects.Count(); i++)
            {
                Selection.gameObjects[i].transform.Rotate(0, UnityEngine.Random.Range(0, 360), 0);
            }
        }
        [MenuItem("Tools/ScaleLarger _x", priority = 100)]
        public static void ScaleLarger()
        {
            Undo.RecordObjects(Selection.transforms, "ScaleLarger.");
            for (int i = 0; i < Selection.gameObjects.Count(); i++)
            {
                Selection.gameObjects[i].transform.localScale += 0.1f * Vector3.one;
            }
        }
        [MenuItem("Tools/ScaleSmaller _z", priority = 100)]
        public static void ScaleSmaller()
        {
            Undo.RecordObjects(Selection.transforms, "ScaleSmaller.");
            for (int i = 0; i < Selection.gameObjects.Count(); i++)
            {
                Selection.gameObjects[i].transform.localScale -= 0.1f * Vector3.one;
            }
        }



        private LevelEditorSceneHandler editorSceneHandler;
        private BattleLevelEditorLoader levelEditorLoader;
        private BattleLevelEditorCreater levelEditorCreater;

        private string loadContentPath;
        private string createPrefabPath;

        private int createPrefabChapter = -1;
        private int createPrefabLevel = -1;

        private int enemyPointCount = 0;

        private Vector2 panelScrollPos;
        private Dictionary<LevelContentType, Vector2> contentScrollPosDic;

        private void Awake()
        {
            levelEditorLoader = new BattleLevelEditorLoader();
            levelEditorCreater = new BattleLevelEditorCreater();

            editorSceneHandler = new LevelEditorSceneHandler();
            editorSceneHandler.Init(levelEditorCreater);

            loadContentPath = BattleLevelEditorLoader.loadContentDefaultPath;
            createPrefabPath = BattleLevelEditorCreater.createPrefabDefaultPath;

            contentScrollPosDic = new Dictionary<LevelContentType, Vector2>();
            foreach (LevelContentType contentType in System.Enum.GetValues(typeof(LevelContentType)))
            {
                contentScrollPosDic[contentType] = Vector2.zero;
            }
        }
        private void OnEnable()
        {
        }
        private void OnDisable()
        {
        }
        private void OnFocus()
        {
        }
        private void OnDestroy()
        {
            editorSceneHandler.Destroy();
            editorSceneHandler = null;
            levelEditorLoader = null;
            levelEditorCreater = null;
        }

        private void OnGUI()
        {
            panelScrollPos = GUILayout.BeginScrollView(panelScrollPos, false, true);

            GUILayout.Space(10);
            GUILayout.Label("Load Room Content");
            loadContentPath = EditorGUILayout.TextField("Path : ", loadContentPath);
            GUILayout.Space(5);
            if (GUILayout.Button("Load"))
            {
                //1:读取所有的模块文件
                levelEditorLoader.LoadContent(loadContentPath);
                //2:设置当前的放置路径
                levelEditorCreater.SetCurrentRoot(loadContentPath);
            }

            GUILayout.Space(20);
            //关上创建按钮
            //GUILayout.Label("Create New FightRoom Room");
            //createPrefabPath = EditorGUILayout.TextField("Path : ", createPrefabPath);
            //using(new EditorGUILayout.HorizontalScope())
            //{
            //    createPrefabChapter = EditorGUILayout.IntField("Chapter",createPrefabChapter,GUILayout.MaxWidth(200));
            //    createPrefabLevel = EditorGUILayout.IntField("Level",createPrefabLevel,GUILayout.MaxWidth(200));
            //}


            //createPrefabChapter = EditorGUILayout.IntField("Chapter", createPrefabChapter);
            //createPrefabLevel = EditorGUILayout.IntField("Level", createPrefabLevel);
            //GUILayout.Space(5);
            //if (GUILayout.Button("Create"))
            //    levelEditorCreater.SaveLevelPrefab(createPrefabPath, createPrefabChapter, createPrefabLevel);
            if (GUILayout.Button("清空场景"))
            {
                editorSceneHandler.DeleteAllInScene();
            }
                GUILayout.Space(20);
            GUILayout.Label("Set Grids Size");
            LevelEditorSceneHandler.gridSize = EditorGUILayout.Vector2Field("Size :", LevelEditorSceneHandler.gridSize);

            GUILayout.Space(10);
            if (levelEditorLoader.contentNameDic != null)
            {
                foreach (LevelContentType contentType in System.Enum.GetValues(typeof(LevelContentType)))
                {
                    GUILayout.Space(10);
                    GUILayout.Label(contentType + " Prefabs");
                    if (levelEditorLoader.contentNameDic[contentType] != null && levelEditorLoader.contentNameDic[contentType].Count > 0)
                    {
                        contentScrollPosDic[contentType] = GUILayout.BeginScrollView(contentScrollPosDic[contentType], GUILayout.MinHeight(150));
                        GUILayout.BeginHorizontal();
                        for (int i = 0; i < levelEditorLoader.contentNameDic[contentType].Count; i++)
                        {
                            GUILayout.BeginVertical();
                            string contentName = levelEditorLoader.contentNameDic[contentType][i];
                            if (GUILayout.Button(levelEditorLoader.GetContentTextureByName(contentName), GUILayout.Width(75), GUILayout.Height(75)))
                            {
                                //string contentPath = levelEditorLoader.GetContentPathByName(contentName);
                                //levelEditorCreater.CreateConnectGameObj(contentType,contentPath);

                                //点选了某个物件。

                                ResetCurrentRoot();

                                editorSceneHandler.SetSelectContent(contentType, levelEditorLoader.GetContentPathByName(contentName));
                            }
                            GUILayout.Box(string.Format("{0} : {1}", i, contentName), GUILayout.Width(75), GUILayout.Height(50));
                            GUILayout.EndVertical();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.EndScrollView();
                    }
                }

                GUILayout.Space(20);
                GUILayout.Label("Enemy Spawn Points");
                enemyPointCount = EditorGUILayout.IntField("Count : ", enemyPointCount);
                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Create"))
                    levelEditorCreater.CreateEnemySpawnPointsByRandom(enemyPointCount, true);
                if (GUILayout.Button("Add/Romove"))
                    levelEditorCreater.CreateEnemySpawnPointsByRandom(enemyPointCount, false);
                GUILayout.EndHorizontal();
            }
            
            GUILayout.Space(20);
            GUILayout.EndScrollView();
            
            GUILayout.Space(10);
            GUILayout.Label("使用方法：1.读取指定目录，需规范命名。2.鼠标点击选取，按下或者拖拽是释放。Esc：清除选中物品。 3.按C开关碰撞检测，按B拖拽碰撞检测位置。4.按Q拖拽刷怪点。");
        }
        //重新设置放置路径
        public void ResetCurrentRoot()
        {
            levelEditorCreater.SetCurrentRoot(loadContentPath);
        }

    }
}