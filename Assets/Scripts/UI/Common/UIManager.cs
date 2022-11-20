using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore.UI {
  public enum UIType {
    NORMAL,
  }

  public class UIManager : MonoBehaviour {
    [SerializeField]
    private Transform NormalCanvas;
    private Stack<UIBase> UIStack = new Stack<UIBase>();
    public static UIManager Instance;

    private void Awake() => Instance = this;

    public async UniTask<T> Open<T>(UIType type, string name, params object[] args) where T : UIBase {
      var ui = Instantiate(await Addressables.LoadAssetAsync<GameObject>(name), GetUIRoot(type)).GetComponent<T>();
      UIStack.Push(ui.Init(args));
      await ui.Open();
      return ui;
    }

    public async UniTask Close<T>(T ui) where T : UIBase {
      if (!UIStack.TryPeek(out var topUi) || topUi != ui) {
        return;
      }
      UIStack.Pop();
      await ui.Close();
      Destroy(ui.gameObject);
    }

    private Transform GetUIRoot(UIType type) {
      switch (type) {
        case UIType.NORMAL:
          return NormalCanvas;
        default:
          return null;
      }
    }
  }
}