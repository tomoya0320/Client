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
      Mask.DOColor(Color.clear, MASK_TRANSITION_TIME);
      var ui = Instantiate(await Addressables.LoadAssetAsync<GameObject>(name), GetUIRoot(type)).GetComponent<T>();
      UIStack.Push(ui.Init(args));
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
      Destroy(ui.gameObject); // TODO:”≈ªØ
      if (UIStack.TryPeek(out topUI)) {
        Mask.color = Color.black;
        Mask.DOColor(Color.clear, MASK_TRANSITION_TIME);
        await UniTask.WhenAll(topUI.OnOpen(), WaitMaskTransitionTime());
        topUI.gameObject.SetActiveEx(true);
      }
      Mask.gameObject.SetActiveEx(false);
      return true;
    }

    public async UniTask<T> OpenChild<T>(T parentUI, string name, params object[] args) where T : UIBase {
      if (!UIStack.TryPeek(out var topUI) || topUI != parentUI) {
        return null;
      }

      return await parentUI.OpenChild<T>(name, args);
    }

    public async UniTask<bool> CloseChild<T>(T ui) where T : UIBase {
      if (!UIStack.TryPeek(out var topUI)) {
        return false;
      }

      return await topUI.CloseChild(ui);
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