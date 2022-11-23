using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace GameCore.UI {
  public enum UIType {
    NORMAL,
    TOP,
  }
  // TODO:优化整个UI管理逻辑
  public class UIManager : MonoBehaviour {
    #region Canvas
    [SerializeField]
    private Transform NormalCanvas;
    [SerializeField]
    private Transform TopCanvas;
    #endregion

    [SerializeField]
    private Image Mask;
    public Camera UICamera;
    private Dictionary<UIType, Stack<UIBase>> UIStackDict = new Dictionary<UIType, Stack<UIBase>>();
    public static UIManager Instance;
    private const float MASK_TRANSITION_TIME = 0.3f;

    private void Awake() {
      foreach (UIType type in Enum.GetValues(typeof(UIType))) {
        UIStackDict.Add(type, new Stack<UIBase>());
      }
      Instance = this;
    }

    private static UniTask WaitMaskTransitionTime() => UniTask.Delay((int)(BattleConstant.THOUSAND * MASK_TRANSITION_TIME), cancellationToken: Game.Instance.CancellationToken);

    public async UniTask<T> Open<T>(UIType type, string name, bool fade = true, params object[] args) where T : UIBase {
      Mask.gameObject.SetActiveEx(true);
      Mask.color = Color.clear;
      if (UIStackDict[type].TryPeek(out var topUI)) {
        if (fade) {
          Mask.DOColor(Color.black, MASK_TRANSITION_TIME);
          await UniTask.WhenAll(topUI.OnClose(), WaitMaskTransitionTime());
        } else {
          await topUI.OnClose();
        }
        topUI.gameObject.SetActiveEx(false);
      } else if (type > UIType.NORMAL) {
        if (fade) {
          Mask.DOColor(Color.black, MASK_TRANSITION_TIME);
          await WaitMaskTransitionTime();
        }
      }
      var handle = Addressables.LoadAssetAsync<GameObject>(name);
      while (!handle.IsDone) {
        await UniTask.Yield(Game.Instance.CancellationToken);
      }
      var ui = Instantiate(handle.Result, GetUIRoot(type)).GetComponent<T>();
      Addressables.Release(handle);
      ui.name = name;
      UIStackDict[type].Push(ui.Init(type, args));
      if (fade) {
        Mask.color = Color.black;
        Mask.DOColor(Color.clear, MASK_TRANSITION_TIME);
        await UniTask.WhenAll(ui.OnOpen(), WaitMaskTransitionTime());
      } else {
        await ui.OnOpen();
      }
      Mask.gameObject.SetActiveEx(false);
      return ui;
    }

    public async UniTask<bool> Close<T>(T ui, bool fade = true) where T : UIBase {
      if (!UIStackDict[ui.UIType].TryPeek(out var topUI) || topUI != ui) {
        return false;
      }
      Mask.gameObject.SetActiveEx(true);
      Mask.color = Color.clear;
      if (fade) {
        Mask.DOColor(Color.black, MASK_TRANSITION_TIME);
        await UniTask.WhenAll(UIStackDict[ui.UIType].Pop().OnClose(), WaitMaskTransitionTime());
      } else {
        await UIStackDict[ui.UIType].Pop().OnClose();
      }
      Destroy(ui.gameObject); // TODO:优化
      if (UIStackDict[ui.UIType].TryPeek(out topUI)) {
        topUI.gameObject.SetActiveEx(true);
        if (fade) {
          Mask.color = Color.black;
          Mask.DOColor(Color.clear, MASK_TRANSITION_TIME);
          await UniTask.WhenAll(topUI.OnOpen(), WaitMaskTransitionTime());
        } else {
          await topUI.OnOpen();
        }
      } else if (ui.UIType > UIType.NORMAL) {
        if (fade) {
          Mask.color = Color.black;
          Mask.DOColor(Color.clear, MASK_TRANSITION_TIME);
          await WaitMaskTransitionTime();
        }
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
        case UIType.TOP:
          return TopCanvas;
        default:
          return null;
      }
    }
  }
}