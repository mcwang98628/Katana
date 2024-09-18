using System.Collections.Generic;
using UnityEngine;

// public partial class LevelStructGenerator
// {
    
//     public static ChapterStructData GenerateTutorialChapterStruct(ChapterData cpData)
//     {
//         if (cpData.levelStructType != LevelStructType.Chapter || !cpData.IsTutorial)
//         {
//             Debug.LogError("章节类型错误！");
//             return null;
//         }
//         ChapterStructData cpStructData = new ChapterStructData();

//         cpStructData.RoomList = new List<List<RoomData>>();
//         for (int i = 0; i < cpData.LevelDataListTutorial.Count; i++)
//         {
//             cpStructData.RoomList.Add(new List<RoomData>());
//             for (int j = 0; j < cpData.LevelDataListTutorial[i].Rooms.Count; j++)
//             {
//                 TutorialRoomData tutorialRoomData = cpData.LevelDataListTutorial[i].Rooms[j];

//                 cpStructData.RoomList[i].Add(GetTutorialRoom(i, j, tutorialRoomData));
//             }
//             for (int j = 0; j < cpStructData.RoomList[i].Count; j++)
//             {
//                 cpStructData.RoomList[i][j].SetRoomIndex(j);
//             }
//         }
//         return cpStructData;
//     }

//     static RoomData GetTutorialRoom(int floorIndex, int roomIndex, TutorialRoomData tutorialRoomData)
//     {
//         var enemyList = new List<List<int>>();
//         enemyList.Add(tutorialRoomData.EnemyIds);
//         return new RoomData(
//             floorIndex,
//             roomIndex,
//             tutorialRoomData.RoomType,
//             tutorialRoomData.Room.name,
//             enemyList);
//     }

// }