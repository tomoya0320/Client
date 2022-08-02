using GameCore;
using Sirenix.OdinInspector;
using UnityEngine;

public class BattleTest : SingletonMono<BattleTest> {
  [LabelText("战斗数据")]
  public BattleData BattleData;

  private void Start() {
    Battle.Enter(BattleData);
  }

  private void Update() {
    // Test
    if(Battle.Instance != null) {
      if (Input.GetKeyDown(KeyCode.Alpha1)) {
        var self = Battle.Instance.SelfPlayer.Master;
        self.PlayCard(self.CardHeapDict[CardHeapType.HAND][0], Battle.Instance.UnitManager.GetUnit(2));
      }
      if (Input.GetKeyDown(KeyCode.Alpha2)) {
        var self = Battle.Instance.SelfPlayer.Master;
        self.EndTurn();
      }
    }
  }
}