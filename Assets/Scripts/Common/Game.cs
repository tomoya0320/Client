using System.Threading;
using UnityEngine;

namespace GameCore {
  public class Game : MonoBehaviour {
    private CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
    public CancellationToken CancellationToken => CancellationTokenSource.Token;
    public static Game Instance { get; private set; }

    private void Awake() => Instance = this;

    private void OnApplicationQuit() {
      if (Battle.Instance != null) {
        Battle.Instance.Cancel(true);
      }
      Cancel();
    }

    private void Cancel() {
      CancellationTokenSource.Cancel();
    }
  }
}