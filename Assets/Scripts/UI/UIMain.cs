using System;
using UnityEngine;

namespace GameCore.UI {
  public class UIMain : UIBase {
    [SerializeField]
    private GameObject ContinueGameBtn;
    public static event Action OnStart; // TEST

    public override UIBase Init(UIType type, params object[] args) {
      ContinueGameBtn.SetActiveEx(Game.Instance.User != null);
      return base.Init(type, args);
    }

    public void StartGame() {
      // TEST
      OnStart?.Invoke();
    }

    public void ContinueGame() {

    }
  }
}