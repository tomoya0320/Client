namespace Battle {
  public class Unit : BattleBase {
    public int RuntimeId { get; private set; }
    public Player Player { get; private set; }
    public Blackboard Blackboard { get; private set; }

    public Unit(BattleManager battleManager, int runtimeId, Player player) : base(battleManager) {
      RuntimeId = runtimeId;
      Player = player;
      Blackboard = BattleManager.ObjectPool.Get<Blackboard>();
    }
  }
}