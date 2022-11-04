using GameCore;
using Sirenix.OdinInspector;
using System.Threading;
using UnityEngine;

public class BattleTest : SingletonMono<BattleTest> {
  [LabelText("战斗数据")]
  public BattleData BattleData;
  private CancellationTokenSource cancellationTokenSource;

  private void Start() {
    cancellationTokenSource = new CancellationTokenSource();
    Battle.Enter(BattleData, cancellationTokenSource.Token);
  }

  private void Update() {
    // Test
    if(Battle.Instance != null) {
      if (Input.GetKeyDown(KeyCode.Alpha1)) {
        var self = Battle.Instance.SelfPlayer.Master;
        self.PlayCard(self.BattleCardControl[CardHeapType.HAND][0], Battle.Instance.UnitManager.GetUnit(2));
      }
      if (Input.GetKeyDown(KeyCode.Alpha2)) {
        var self = Battle.Instance.SelfPlayer.Master;
        self.EndTurn();
      }
    }
  }

  private void OnApplicationQuit() {
    cancellationTokenSource.Cancel();
    cancellationTokenSource.Dispose();
    cancellationTokenSource = null;
  }
}