namespace GameCore {
  public class Player : BattleBase {
    public int RuntimeId { get; private set; }
    public Blackboard Blackboard { get; private set; }
    public Unit Master { get; private set; }
    public Unit[] Units { get; private set; }
    public int DeadUnitCount;
    public int TotalUnitCount => Units.Length;
    public bool Available => DeadUnitCount < TotalUnitCount;

    public Player(Battle battle) : base(battle) {
      Blackboard = Battle.ObjectPool.Get<Blackboard>();
    }

    public void Init(int runtimeId, PlayerData playerData) {
      RuntimeId = runtimeId;
      Units = new Unit[playerData.UnitData.Length];
      for (int i = 0; i < Units.Length; i++) {
        Units[i] = Battle.UnitManager.Create(this, playerData.UnitData[i]);
      }
      Master = Units[playerData.FirstIndex];
    }
  }
}