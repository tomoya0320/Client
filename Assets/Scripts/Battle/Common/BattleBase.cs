namespace GameCore {
  public abstract class BattleBase {
    public Battle Battle { get; private set; }

    protected BattleBase(Battle battle) => Battle = battle;
  }
}