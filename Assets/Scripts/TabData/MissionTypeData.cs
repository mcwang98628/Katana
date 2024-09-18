

public struct MissionTabData
{
    public int Id;
    public string Name;
    public MissionType MissionType;
    public MissionTargetType TargetType;
    public int TargetValue;
    public int TargetNumber;
    public MissionRewardType RewardType;
    public int RewardNumber;


    public MissionTabData(
        int id,
        string name,
        MissionType missionType,
        MissionTargetType targetType,
        int targetValue,
        int targetNumber,
        MissionRewardType rewardType,
        int rewardNumber)
    {
        Id = id;
        Name = name;
        MissionType = missionType;
        TargetType = targetType;
        TargetValue = targetValue;
        TargetNumber = targetNumber;
        RewardType = rewardType;
        RewardNumber = rewardNumber;
    }
    
}