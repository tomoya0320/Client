namespace Battle {
  public class Unit : BattleBase {
    public int RuntimeId { get; private set; }
    public Player Player { get; private set; }
    public UnitData UnitData { get; private set; }
    public Blackboard Blackboard { get; private set; }
    public string Name => UnitData?.Name;

    public Unit(BattleManager battleManager, int runtimeId, Player player, UnitData unitData) : base(battleManager) {
      RuntimeId = runtimeId;
      Player = player;
      UnitData = unitData;
      Blackboard = BattleManager.ObjectPool.Get<Blackboard>();
    }
  }
}