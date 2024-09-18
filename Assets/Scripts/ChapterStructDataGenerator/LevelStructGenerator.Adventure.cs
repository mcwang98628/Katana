

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class LevelStructGenerator
{

    public static AdventureStructData GenerateAdventureStruct(ChapterData_Adventure chapterData)
    {
        chapterData.Init();
        
        //生成的配置关卡空结构
        List<ChapterData.LevelStructConfig_Floor> chapterStructConfig = GetChapterStruct(chapterData.AdventureFloorData.Count);
        
        //最终输出的关卡结构
        AdventureStructData cpStructData = new AdventureStructData();
        cpStructData.RoomList = new List<List<RoomData>>();
        List<int> floorDataIdList = new List<int>();
        for (int i = 0; i < chapterData.AdventureFloorData.Count; i++)
        {
            floorDataIdList.Add(i);
        }

        List<int> indexs = new List<int>();
        for (int i = 0; i < chapterData.EnvironmentData.Count; i++)
            indexs.Add(i);
        while (indexs.Count > 0)
        {
            int index = indexs[Random.Range(0, indexs.Count)];
            cpStructData.EnvironmentDatas.Add(chapterData.EnvironmentData[index]);
            indexs.Remove(index);
        }
        
        
        for (int i = 0; i < chapterStructConfig.Count; i++)
        {
            List<RoomData> floorStruct = new List<RoomData>();
            int rand = Random.Range(0, floorDataIdList.Count);
            int floorDataId = floorDataIdList[rand];
            for (int j = 0; j < chapterStructConfig[i].Rooms.Count; j++)
            {
                floorStruct.Add(GenerateRoomData(chapterData.AdventureFloorData[floorDataId], chapterStructConfig[i].Rooms[j], i, j));
                
            }
            
            cpStructData.ChapterIds.Add(chapterData.AdventureFloorData[floorDataId].ChapterID);
            floorDataIdList.RemoveAt(rand);
            cpStructData.RoomList.Add(floorStruct);
        }

        return cpStructData;
    }


    static List<ChapterData.LevelStructConfig_Floor> GetChapterStruct(int maxFloorCount)
    {
        List<ChapterData.LevelStructConfig_Floor> result = new List<ChapterData.LevelStructConfig_Floor>();
        for (int i = 0; i < maxFloorCount; i++)
        {
            //初始化随机参数
            int fightRoomCount = Random.Range(1,6);
            int fightRoomWaveCount = fightRoomCount <3?3:1;
            int fightRoomDifficult = 9 + i * 2;
            int fightRoomMaxEnemyLv = i > 2 ? 3 : 2;
            bool hasTreasureRoom = (Random.value < 0.5f);
            bool hasEventRoom = (Random.value < 0.3f);
            bool isBossLevel=(i%3==2);
            
            ChapterData.LevelStructConfig_Floor floor = new ChapterData.LevelStructConfig_Floor();
            floor.Rooms = new List<RoomInfoConfig>();
            
            floor.Rooms.Add(new RoomInfoConfig(RoomType.StartRoom));
            for (int j = 0; j < fightRoomCount; j++)
            {
                floor.Rooms.Add(new RoomInfoConfig(RoomType.FightRoom, fightRoomDifficult+Random.Range(-2,3),fightRoomMaxEnemyLv , fightRoomWaveCount));
            }
            if (hasTreasureRoom)
            {
                floor.Rooms.Insert(Random.Range(1,floor.Rooms.Count),new RoomInfoConfig(RoomType.TreasureRoom));
            }
            if (hasEventRoom)
            {
                floor.Rooms.Insert(Random.Range(1,floor.Rooms.Count),new RoomInfoConfig(RoomType.EventRoom));
            }
            if(isBossLevel)
            {
                floor.Rooms.Add(new RoomInfoConfig(RoomType.BossFightRoom));
            }
            else
            {
                floor.Rooms.Add(new RoomInfoConfig(RoomType.ShopRoom));
            }
            
            result.Add(floor);
        }
        return result;
    }

    static RoomData GenerateRoomData(ChapterData_Adventure.AdventureFloorDataGroup chapterData, RoomInfoConfig roomInfo, int floorIndex, int roomIndex)
    {
        List<List<int>> enemyWaves = null;
        if (roomInfo.RoomType == RoomType.FightRoom)
        {
            enemyWaves = new List<List<int>>();
            for (int i = 0; i < roomInfo.EnemyWaveCount; i++)
            {
                enemyWaves.Add(GetEnemyWave(chapterData, roomInfo.MaxEnemyLv, roomInfo.EnemyWaveDifficult));
            }
        }
        else if (roomInfo.RoomType == RoomType.BossFightRoom)
        {
            enemyWaves = new List<List<int>>();
            enemyWaves.Add(GetBossWave(chapterData));
        }

        string str = "";
        try
        {
            var room = chapterData.RoomData.GetRoom(roomInfo.RoomType,false);
            if (room == null)
            {
                Debug.LogError($"Null\n {roomInfo.RoomType}");
            }
            str = room.name;
        }
        catch (Exception e)
        {
            Debug.LogError(chapterData == null);
            Debug.LogError(chapterData.RoomData == null);
            throw;
        }
        return new RoomData(
            floorIndex,
            roomIndex,
            roomInfo.RoomType,
            str,
            enemyWaves,
            roomInfo);
    }
    
    
    static List<int> GetBossWave(ChapterData_Adventure.AdventureFloorDataGroup chapterData)
    { var enemyData = chapterData.EnemyData.GetEnemy(4);
        List<int> result = new List<int>();
        if (enemyData != null)
        {
            result.Add(enemyData.EnemyID);
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                result.Add( chapterData.EnemyData.GetEnemy(3).EnemyID);
            }
        }
        return result;
    }
    static List<int> GetEnemyWave(ChapterData_Adventure.AdventureFloorDataGroup chapterData, int maxEnemyLv, int difficult)
    {
        return GetEnemyWave(chapterData.EnemyData,maxEnemyLv,difficult);
    }

}