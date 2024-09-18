using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Adventure Chapter Data Asset")]
public class ChapterData_Adventure : ScriptableObject
{

    public void Init()
    {
        foreach (var adventureLevelData in AdventureFloorData)
        {
            adventureLevelData.RoomData.Init();
            adventureLevelData.EnemyData.Init();
        }
    }

    [Header("关卡环境")]
    [LabelText("起点环境")] 
    public EnvironmentItemScript StartEnvironment;
    [LabelText("层环境池")] 
    public List<EnvironmentItemScript> EnvironmentData;


    [Header("层数据")] 
    [LabelText("起点房间")] 
    public RoomController StartRoom;
    [FormerlySerializedAs("AdventureLevelData")] [LabelText("各层数据")] 
    public List<AdventureFloorDataGroup> AdventureFloorData;
    [System.Serializable]
    public class AdventureFloorDataGroup
    {
        [LabelText("章节ID")][GUIColor(0.4f, 1f, 0.5f)]
        public int ChapterID;
        [LabelText("房间数据")] [GUIColor(0.4f, 1f, 0.5f)]
        public ChapterData.RoomDataGroup RoomData;
        
        [LabelText("敌人数据")] [GUIColor(1f, 0.7f, 0.7f)]
        public ChapterData.EnemyDataGroup EnemyData;
    }

    [Header("物品数据")] 
    [LabelText("物品数据")] [GUIColor(0.4f, 1f, 0.5f)] public ChapterData.ItemDataGroup ItemData;
}