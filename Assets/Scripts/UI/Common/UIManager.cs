using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using DG.Tweening;

namespace GameCore.UI {
  public enum UIType {
    NORMAL,
  }
  // TODO:优化整个UI管理逻辑
  public class UIManager : MonoBehaviour {
    #region Canvas
    [SerializeField]
    private Transform NormalCanvas;
    #endregion

    [SerializeField]
    private Image Mask;
    public Camera UICamera;
    private Stack<UIBase> UIStack = new Stack<UIBase>();
    public static UIManager Instance;
    private const float MASK_TRANSITION_TIME = 0.3f;

    private void Awake() => Instance = this;

    private static UniTask WaitMaskTransitionTime() => UniTask.Delay((int)(BattleConstant.THOUSAND * MASK_TRANSITION_TIME), cancellationToken: Game.Instance.CancellationToken);

    public async UniTask<T> Open<T>(UIType type, string name, params object[] args) where T : UIBase {
      Mask.gameObject.SetActiveEx(true);
      if (UIStack.TryPeek(out var topUI)) {
        Mask.color = Color.clear;
        Mask.DOColor(Color.black, MASK_TRANSITION_TIME);
        await UniTask.WhenAll(topUI.OnClose(), WaitMaskTransitionTime());
        topUI.gameObject.SetActiveEx(false);
      }
      Mask.color = Color.black;
      var handle = Addressables.LoadAssetAsync<GameObject>(name);
      while (!handle.IsDone) {
        await UniTask.Yield(Game.Instance.CancellationToken);
      }
      var ui = Instantiate(handle.Result, GetUIRoot(type)).GetComponent<T>();
      ui.name = name;
      UIStack.Push(ui.Init(args));
      Mask.DOColor(Color.clear, MASK_TRANSITION_TIME);
      await UniTask.WhenAll(ui.OnOpen(), WaitMaskTransitionTime());
      Mask.gameObject.SetActiveEx(false);
      return ui;
    }

    public async UniTask<bool> Close<T>(T ui) where T : UIBase {
      if (!UIStack.TryPeek(out var topUI) || topUI != ui) {
        return false;
      }
      Mask.gameObject.SetActiveEx(true);
      Mask.color = Color.clear;
      Mask.DOColor(Color.black, MASK_TRANSITION_TIME);
      await UniTask.WhenAll(UIStack.Pop().OnClose(), WaitMaskTransitionTime());
      Destroy(ui.gameObject); // TODO:优化
      if (UIStack.TryPeek(out topUI)) {
        Mask.color = Color.black;
        Mask.DOColor(Color.clear, MASK_TRANSITION_TIME);
        await UniTask.WhenAll(topUI.OnOpen(), WaitMaskTransitionTime());
        topUI.gameObject.SetActiveEx(true);
      }
      Mask.gameObject.SetActiveEx(false);
      return true;
    }

    public async UniTask<T> OpenChild<T>(UIBase parentUI, string name, params object[] args) where T : UIBase {
      Mask.gameObject.SetActiveEx(true);
      Mask.color = Color.clear;
      var childUI = await parentUI.OpenChild<T>(name, args);
      Mask.gameObject.SetActiveEx(false);
      return childUI;
    }

    public async UniTask<bool> CloseChild<T>(UIBase parentUI, T ui) where T : UIBase {
      Mask.gameObject.SetActiveEx(true);
      Mask.color = Color.clear;
      bool closed = await parentUI.CloseChild(ui);
      Mask.gameObject.SetActiveEx(false);
      return closed;
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