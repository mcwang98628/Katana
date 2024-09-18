using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LevelEditor
{
    public class NormalLevelEditorWindow : EditorWindow
    {
        [MenuItem("Tools/Normal Level Editor Window &3",priority = 101)]
        public static void ShowLevelEditorWindow()
        {
            //弹出窗口
            EditorWindow.GetWindow<NormalLevelEditorWindow>(true,"其他房间编辑器").Show();
        }

        private NormalLevelEditorCreater levelEditorCreater;

        private string loadSamplePathName;
        private string loadContentPath;

        private int createPrefabChapter = -1;

        private void Awake()
        {
            levelEditorCreater = new NormalLevelEditorCreater();

            loadSamplePathName = NormalLevelEditorCreater.loadSampleDefaultPathName;
            loadContentPath = NormalLevelEditorCreater.loadContentDefaultPath;

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
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            GUILayout.Label("Load Room Sample");
            loadSamplePathName = EditorGUILayout.TextField("Sample Path&Name : ",loadSamplePathName);
            loadContentPath = EditorGUILayout.TextField("Content Path : ",loadContentPath);
            GUILayout.Space(5);
            if(GUILayout.Button("Load"))
            {
                levelEditorCreater.LoadSampleAndContent(loadSamplePathName,loadContentPath);
            }

            GUILayout.Space(5);
            if(GUILayout.Button("Replace"))
            {
                levelEditorCreater.ReplaceContent();
            }

            GUILayout.Space(10);
            createPrefabChapter = EditorGUILayout.IntField("Chapter",createPrefabChapter);
            GUILayout.Space(5);
            if(GUILayout.Button("Save"))
            {
                levelEditorCreater.SaveNewSamplePrefab(createPrefabChapter);
            }
        }
    }
}