using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore.UI {
  public class UIMain : UIBase {
    [SerializeField]
    private GameObject ContinueGameBtn;

    public override UIBase Init(UIType type, params object[] args) {
      ContinueGameBtn.SetActiveEx(Game.Instance.User != null);
      return base.Init(type, args);
    }

    public async void StartGame() {
      Game.Instance.CreateNewUser();
      UIManager.Instance.SetLoadingMaskEnable(true);
      var avgGraph = await Addressables.LoadAssetAsync<AVGGraph>("StartAVG");
      UIManager.Instance.SetLoadingMaskEnable(false);
      var ui = await AVG.Enter(avgGraph);
      ui.AVG.OnExit = async () => { await UIManager.Instance.Open<UIMap>(UIType.NORMAL, "UIMap", true); };
    }

    public async void ContinueGame() {
      await UIManager.Instance.Open<UIMap>(UIType.NORMAL, "UIMap");
    }
  }
}