using System;
using System.Collections.Generic;


[Serializable]
public class ChapterRulesRuntimeData : BattleRuntimeBaseData
{

    #region 关卡结构
    
    //章节模式的 关卡数据
    public override IBattleLevelStructData LevelStructData => _chapterStructData;
    protected ChapterStructData _chapterStructData;

    public override void SetLevelStructData(IBattleLevelStructData levelStructData)
    {
        if (levelStructData.LevelStructType == LevelStructType.Chapter)
        {
            _chapterStructData = (ChapterStructData) levelStructData;
        }
        else if (levelStructData.LevelStructType == LevelStructType.Adventure)
        {
            _chapterStructData = (AdventureStructData) levelStructData;
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    #endregion
    
    #region 章节信息

    /// <summary>
    /// 是否是教程章节
    /// </summary>
    public override bool IsTutorial => CurrentChapterId == 0;

    
    //房间index 从0开始
    public int CurrentRoomIndex => _currentRoomIndex;
    private int _currentRoomIndex;
    
    //层index 从0开始
    public int CurrentLevelIndex => _currentLevelIndex;
    private int _currentLevelIndex;
    
    //章节
    public override int CurrentChapterId => _currentChapterId;
    private int _currentChapterId;

    //进入到下一个房间。
    public void EnterNextRoom()
    {
        _currentRoomIndex++;
        if (CurrentRoomIndex >= _chapterStructData.RoomList[CurrentLevelIndex].Count)
        {
            _currentLevelIndex++;
            _currentRoomIndex = 0;
        }
    }

    public void SetChapterData(int chapteId,int levelIndex,int roomIndex)
    {
        _currentChapterId = chapteId;
        _currentLevelIndex = levelIndex;
        _currentRoomIndex = roomIndex;
    }

    public void SetCpId(int chapteId)
    {
        _currentChapterId = chapteId;
    }
    
    #endregion

    #region 进度相关

    /// <summary>
    /// 通过的房间
    /// </summary>
    public List<RoomType> PassingRooms = new List<RoomType>();
    
    /// <summary>
    /// 通过房间第几次
    /// </summary>
    public int GetPassingRoomTimes(RoomType roomType)
    {
        int times = 0;
        if (PassingRooms.Contains(roomType))
        {
            foreach (RoomType room in PassingRooms)
            {
                if (room == roomType)
                {
                    times++;
                }
            }
        }
        return times;
    }
    
    /// <summary>
    /// 清空的房间数量
    /// </summary>
    public int ClearRoomNumber
    {
        get
        {
            int currentRoomNumber = 0;
            for (int i = 0; i <= CurrentLevelIndex; i++)
            {
                if (i == CurrentLevelIndex)
                {
                    currentRoomNumber += CurrentRoomIndex;
                }
                else
                {
                    currentRoomNumber += _chapterStructData.RoomList[i].Count;
                }
            }

            currentRoomNumber--;
            return currentRoomNumber;
        }
    }
    /// <summary>
    /// 游戏进度 0-100
    /// </summary>
    public override int Progress
    {
        get
        {
            if (ClearRoomNumber <= 0)
            {
                return 0;
            } 
            float progress = (float)ClearRoomNumber / AllRoomCount;
            progress *= 100;
            return (int)progress;
        }
    }

    public int AllRoomCount => _chapterStructData.AllRoomCount; 
    #endregion

}
