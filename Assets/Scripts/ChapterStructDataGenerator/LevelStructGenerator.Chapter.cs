

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class LevelStructGenerator
{

    public static ChapterStructData GenerateChapterStruct(ChapterData chapterData)
    {
        chapterData.Init();
        ChapterStructData cpStructData = new ChapterStructData();
        cpStructData.RoomList = new List<List<RoomData>>();
        int enemyCount = 0;
        for (int i = 0; i < chapterData.ChapterStruct.Count; i++)
        {
            List<RoomData> FloorStruct = new List<RoomData>();
            for (int j = 0; j < chapterData.ChapterStruct[i].Rooms.Count; j++)
            {
                RoomData room = GenerateRoomData(chapterData, chapterData.ChapterStruct[i].Rooms[j], i, j);
                FloorStruct.Add(room);
                if (room.RoomType == RoomType.FightRoom)
                {
                    int lv1EnemyCount = 0;
                    int lv2EnemyCount = 0;
                    int lv3EnemyCount = 0;
                    for (int k = 0; k < room.EnemyWaves[0].Count; k++)
                    {
                        var enemyInfo = DataManager.Inst.GetEnemyInfo(room.EnemyWaves[0][k]);
                        //Debug.LogError(enemyInfo.EnemyName + " = "+enemyInfo.EnemyWeight);
                        switch (enemyInfo.EnemyWeight)
                        {
                            case 1 :
                                lv1EnemyCount++;
                                break;
                            case 2 :
                                lv2EnemyCount++;
                                break;
                            case 3 :
                                lv3EnemyCount++;
                                break;
                        }

                        enemyCount++;

                    }
                    // Debug.LogError("第"+(i+1)+"层战斗房怪物："+lv1EnemyCount+" | "+lv2EnemyCount+" | "+lv3EnemyCount);
                }
            }
            cpStructData.RoomList.Add(FloorStruct);
        }
        // Debug.LogError("总怪物数："+enemyCount);
        return cpStructData;
    }

    
    static RoomData GenerateRoomData(ChapterData chapterData, RoomInfoConfig roomInfo, int floorIndex, int roomIndex)
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
            var room = chapterData.RoomData.GetRoom(roomInfo.RoomType, chapterData.IsTutorial);
            if (room == null)
            {
                Debug.LogError($"Null\n {roomInfo.RoomType} - {chapterData.IsTutorial}");
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

    static List<int> GetBossWave(ChapterData chapterData)
    {
        return new List<int>() { chapterData.EnemyData.GetEnemy(4).EnemyID };

    }


    static List<int> GetEnemyWave(ChapterData chapterData, int maxEnemyLv, int difficult)
    {
        return GetEnemyWave(chapterData.EnemyData,maxEnemyLv,difficult);
    }

    /*随怪规则
    *1.从怪物池里选2~3种怪，构建带权卡池
    *2.按照权重平衡概率
    *3.限制3级怪的数量
    */
    static List<int> GetEnemyWave(ChapterData.EnemyDataGroup enemyData, int maxEnemyLv, int difficult)
    {
        //从怪物池里选取2~3种怪
        List<EnemyInfoData> enemies = new List<EnemyInfoData>();
        enemies.Add(enemyData.GetEnemy(1));
        enemies.Add(enemyData.GetEnemy(maxEnemyLv>=2?2:1));
        if(maxEnemyLv>=2)
            enemies.Add(enemyData.GetEnemy(maxEnemyLv>=3?3: Random.Range(1,3)));

        if (enemies.Count <= 0)
        {
            Debug.LogError("没有可以使用的怪物或者怪物难度值小于0");
            return null;
        }
        
        //权重list
        List<int> weights = new List<int>();
        int weightSum = 0;
        for (int i = 0; i < enemies.Count; i++)
        {
            int weight=0;
            if (enemies[i].EnemyWeight == 1)
            {
                weight = 30;
            } 
            else if (enemies[i].EnemyWeight == 2)
            {
                weight = 20;
            }
            else if (enemies[i].EnemyWeight == 3)
            {
                weight = 5;
            }
            weights.Add(weight);
            weightSum += weight;
        }


        //用怪物池里的怪物随机填充怪物列表
        List<int> wave = new List<int>();
        float waveWeightTemp = 0;
        do
        {
            int rand = Random.Range(0, weightSum);
            int temp = 0;
            for (int i = 0; i < weights.Count; i++)
            {
                temp += weights[i];
                if (temp >= rand)
                { 
                    wave.Add(enemies[i].EnemyID);
                    waveWeightTemp += enemies[i].EnemyWeight;
                    break;
                }
            }
        } while (waveWeightTemp < difficult);

        return wave;
    }
}