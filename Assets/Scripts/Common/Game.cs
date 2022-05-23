using Battle;

public class Game : SingletonMono<Game> {
  public BattleData BattleData;

  private void Start() {
    BattleManager.Enter(BattleData);
  }
}