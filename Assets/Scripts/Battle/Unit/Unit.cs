namespace Battle {
  public class Unit : BattleBase {
    public int RuntimeId { get; private set; }
    public Player Player { get; private set; }
    public UnitTemplate UnitTemplate { get; private set; }
    public Blackboard Blackboard { get; private set; }
    public string Name => UnitTemplate?.Name;

    public Unit(BattleManager battleManager, int runtimeId, Player player, UnitTemplate unitTemplate) : base(battleManager) {
      RuntimeId = runtimeId;
      Player = player;
      UnitTemplate = unitTemplate;
      Blackboard = BattleManager.ObjectPool.Get<Blackboard>();
    }
  }
}