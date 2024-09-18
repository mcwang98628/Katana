using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

[Serializable]
public class EndlessStructData:IBattleLevelStructData
{
    public LevelStructType LevelStructType => LevelStructType.OneRoomEndless;
    
    [FormerlySerializedAs("RoomPath")] public string RoomPerfabName;
    
    public List<EndlessStructWaveData> WaveDatas = new List<EndlessStructWaveData>();
    
}

[Serializable]
public class EndlessStructWaveData
{
    public EndlessWaveType WaveType;
    public int RoomContentId;
    public List<int> EnemyList;
}

public enum EndlessWaveType
{
    Event,
    
    Enemy,
}
