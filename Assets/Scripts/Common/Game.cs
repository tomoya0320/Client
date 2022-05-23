using GameCore;
using Sirenix.OdinInspector;

public class Game : SingletonMono<Game> {
  [LabelText("Õ½¶·Êý¾Ý")]
  public BattleData BattleData;

  private void Start() {
    Battle.Enter(BattleData);
  }
}