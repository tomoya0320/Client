using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameCore.UI {
  public class UIMain : UIBase {
    [SerializeField]
    private GameObject ContinueGameBtn;

    public override UniTask Init(UIType type, params object[] args) {
      ContinueGameBtn.SetActiveEx(Game.Instance.User != null);
      return base.Init(type, args);
    }

    public async void StartGame() {
      Game.Instance.CreateNewUser();
      var ui = await AVG.Enter("StartAVG");
      ui.AVG.OnExit = async () => await UIManager.Instance.Open<UIMap>(UIType.NORMAL, "UIMap", true);
    }

    public async void ContinueGame() {
      await UIManager.Instance.Open<UIMap>(UIType.NORMAL, "UIMap");
    }
  }
}