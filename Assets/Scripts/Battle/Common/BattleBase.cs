namespace Battle {
  public abstract class BattleBase {
    protected BattleManager BattleManager;

    protected BattleBase(BattleManager battleManager) => BattleManager = battleManager;
  }
}