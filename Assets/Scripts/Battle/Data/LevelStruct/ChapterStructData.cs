using System;
using System.Collections.Generic;
using UnityEngine;

//不要出现自定义或Unity的引用类型
[Serializable]
public class ChapterStructData:IBattleLevelStructData
{
    public virtual LevelStructType LevelStructType => LevelStructType.Chapter;

    protected int allRoomCount;

    public int AllRoomCount
    {
        get
        {
            if (allRoomCount == 0)
            {
                foreach (var level in RoomList)
                {
                    allRoomCount += level.Count;
                }
            }

            return allRoomCount;
        }
    }
    
    
    //层List - 房间List - 房间位置可能的多少个房间
    public List<List<RoomData>> RoomList;
    
}