namespace Battle {
  public enum BehaviorTime {
    ON_BEFORE_TURN,
    ON_LATE_TURN,
  }

  public class BehaviorManager : BattleBase {
    public BehaviorManager(BattleManager battleManager) : base(battleManager) {

    }
  }
}