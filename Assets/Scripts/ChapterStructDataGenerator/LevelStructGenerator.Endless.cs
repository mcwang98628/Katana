

using System.Collections.Generic;
using UnityEngine;

public partial class LevelStructGenerator
{
    
    public static EndlessStructData GenerateEndlessChapterStruct(ChapterData cpData)
    {
        if (cpData.levelStructType != LevelStructType.OneRoomEndless)
        {
            Debug.LogError("章节类型错误！");
            return null;
        }
        EndlessStructData cpStructData = new EndlessStructData();

        cpStructData.RoomPerfabName = cpData.EndlessLevelData.Room.gameObject.name;


        for (int i = 0; i < 10; i++)
        {
            cpStructData.WaveDatas.Add(new EndlessStructWaveData()
            {
                RoomContentId = -1,
                EnemyList = new List<int>() { 15, 15, 15, 15, 15, 15 },
                WaveType = EndlessWaveType.Enemy
            });
        }
        cpStructData.WaveDatas.Insert(3, new EndlessStructWaveData()
        {
            RoomContentId = 101,
            EnemyList = null,
            WaveType = EndlessWaveType.Event
        });

        return cpStructData;
    }



}