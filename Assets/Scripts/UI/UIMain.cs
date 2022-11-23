using UnityEngine;

namespace GameCore.UI {
  public class UIMain : UIBase {
    [SerializeField]
    private GameObject ContinueGameBtn;

    public override UIBase Init(params object[] args) {
      ContinueGameBtn.SetActiveEx(Game.Instance.User != null);
      return base.Init(args);
    }

    public void StartGame() {

    }

    public void ContinueGame() {

    }
  }
}