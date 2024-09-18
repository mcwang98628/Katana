

public interface IBattleRules
{
    bool IsLoadOver { get; }
    LevelStructType BattleRulesType { get; }
    bool IsArchiveBattle { get; }
    RoomController CurrentRoom { get; }
    void StartGame();
    void LoadBattle(IBattleRulesData rulesData);
    void LoadBattle(IArchiveBattleData archiveBattleData);
    void EndBattle();
    void Update();

    void UpdateHeroData();
}
