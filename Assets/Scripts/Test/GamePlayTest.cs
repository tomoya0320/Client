using Battle;
using UnityEngine;

public class GamePlayTest : MonoBehaviour {
  public BattleManager BattleManager = new BattleManager();

  private void FixedUpdate() {
    BattleManager.BeforeUpdate();
    BattleManager.Update();
    BattleManager.LateUpdate();
  }
}