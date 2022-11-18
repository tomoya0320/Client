using GameCore;
using Sirenix.OdinInspector;
using System.Threading;

public class BattleTest : SingletonMono<BattleTest> {
  [LabelText("战斗数据")]
  public BattleData BattleData;
  private CancellationTokenSource cancellationTokenSource;

  private void Start() {
    cancellationTokenSource = new CancellationTokenSource();
    Battle.Enter(BattleData, cancellationTokenSource.Token);
  }

  private void OnApplicationQuit() {
    cancellationTokenSource.Cancel();
    cancellationTokenSource.Dispose();
    cancellationTokenSource = null;
  }
}