using GameCore;
using Sirenix.OdinInspector;

public class Game : SingletonMono<Game> {
  [LabelText("ս������")]
  public BattleData BattleData;

  private void Start() {
    Battle.Enter(BattleData);
  }
}