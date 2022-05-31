using GameCore;
using Sirenix.OdinInspector;

public class Game : SingletonMono<Game> {
  [LabelText("战斗数据")]
  public BattleData BattleData;

  private void Start() {
    Battle.Enter(BattleData);
  }
}