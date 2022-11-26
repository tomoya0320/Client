using GameCore.UI;
using System;
using System.Threading;
using UnityEngine;

namespace GameCore {
  public class Game : MonoBehaviour {
    private CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
    public CancellationToken CancellationToken => CancellationTokenSource?.Token ?? CancellationToken.None;
    public UnitDatabase UnitDatabase; // TODO:hot update
    public CardDatabase CardDatabase; // TODO:hot update
    public MapNodeDatabase MapNodeDatabase; // TODO:hot update
    public User User { get; private set; }
    public event Action OnUpdate;
    public static Game Instance { get; private set; }

    private void Awake() {
      Application.targetFrameRate = 120;
      User = User.LoadFromLocal();
      Instance = this;
    }

    private async void Start() {
#if TEST
      var battleTest = GetComponent<Test.BattleTest>();
      if (battleTest) {
        battleTest.Test();
        return;
      }
#endif
      await UIManager.Instance.Open<UIMain>(UIType.NORMAL, "UIMain");
    }

    private void Update() {
      OnUpdate?.Invoke();
    }

    private void OnApplicationQuit() {
      Battle.Instance?.Cancel();
      Cancel();
    }

    private void Cancel() {
      CancellationTokenSource.Cancel();
      CancellationTokenSource.Dispose();
      CancellationTokenSource = null;
    }

    public void CreateNewUser() => User = User.CreateNew();
  }
}
