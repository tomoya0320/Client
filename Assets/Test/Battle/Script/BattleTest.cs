using Sirenix.OdinInspector;
using System.Threading;
using UnityEngine;

namespace GameCore.Test {
  public class BattleTest : MonoBehaviour {
    [LabelText("战斗数据")]
    public BattleData BattleData;
    private CancellationTokenSource cancellationTokenSource;

    private void Start() {
      Application.targetFrameRate = 120;
      cancellationTokenSource = new CancellationTokenSource();
      Battle.Enter(BattleData, cancellationTokenSource.Token);
    }

    private void OnApplicationQuit() {
      cancellationTokenSource.Cancel();
      cancellationTokenSource.Dispose();
      cancellationTokenSource = null;
    }
  }
}