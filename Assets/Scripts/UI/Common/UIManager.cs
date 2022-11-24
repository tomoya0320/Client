using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
    private GameObject MaskGo;
    private int MaskEnableCount;
    public Camera UICamera;
    private Dictionary<UIType, Stack<UIBase>> UIStackDict = new Dictionary<UIType, Stack<UIBase>>();
    public static UIManager Instance;

    private void Awake() {
      foreach (UIType type in Enum.GetValues(typeof(UIType))) {
        UIStackDict.Add(type, new Stack<UIBase>());
      }
      Instance = this;
    }

    public async UniTask<T> Open<T>(UIType type, string name, params object[] args) where T : UIBase {
      SetMaskEnable(true);
      var handle = Addressables.LoadAssetAsync<GameObject>(name);
      while (!handle.IsDone) {
        await UniTask.Yield(Game.Instance.CancellationToken);
      }
      if (UIStackDict[type].TryPeek(out var topUI)) {
        await topUI.OnClose();
      }
      var ui = Instantiate(handle.Result, GetUIRoot(type)).GetComponent<T>();
      Addressables.Release(handle);
      ui.name = name;
      UIStackDict[type].Push(ui.Init(type, args));
      await ui.OnOpen();
      SetMaskEnable(false);
      return ui;
    }

    public async UniTask<bool> Close<T>(T ui) where T : UIBase {
      if (!UIStackDict[ui.UIType].TryPeek(out var topUI) || topUI != ui) {
        return false;
      }
      SetMaskEnable(true);
      await UIStackDict[ui.UIType].Pop().OnClose();
      ui.OnRemove(); // TODO:优化
      if (UIStackDict[ui.UIType].TryPeek(out topUI)) {
        await topUI.OnOpen();
      }
      SetMaskEnable(false);
      return true;
    }

    public async UniTask<T> OpenChild<T>(UIBase parentUI, string name, params object[] args) where T : UIBase {
      SetMaskEnable(true);
      var childUI = await parentUI.OpenChild<T>(name, args);
      SetMaskEnable(false);
      return childUI;
    }

    public async UniTask<bool> CloseChild<T>(UIBase parentUI, T ui) where T : UIBase {
      SetMaskEnable(true);
      bool closed = await parentUI.CloseChild(ui);
      SetMaskEnable(false);
      return closed;
    }

    public void SetMaskEnable(bool enable) {
      MaskEnableCount += enable ? 1 : -1;
      MaskGo.SetActiveEx(MaskEnableCount > 0);
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