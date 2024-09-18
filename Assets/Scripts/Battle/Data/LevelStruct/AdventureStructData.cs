using System;
using System.Collections.Generic;

[Serializable]
public class AdventureStructData:ChapterStructData
{
    public override LevelStructType LevelStructType => LevelStructType.Adventure;

    public List<EnvironmentItemScript> EnvironmentDatas = new List<EnvironmentItemScript>();
    public List<int> ChapterIds = new List<int>();
    
    
    
}