using GameCore.UI;
using System.Threading;
using UnityEngine;

namespace GameCore {
  public class Game : MonoBehaviour {
    private CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
    public CancellationToken CancellationToken => CancellationTokenSource.Token;
    public User User { get; private set; }
    public static Game Instance { get; private set; }

    private void Awake() {
      Application.targetFrameRate = 120;
      User = User.LoadFromLocal();
      Instance = this;
    }

    private async void Start() {
      await UIManager.Instance.Open<UIMain>(UIType.NORMAL, "UIMain");
    }

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