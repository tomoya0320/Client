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
    private GameObject UIMask;
    [SerializeField]
    private GameObject LoadingMask;
    private int UIMaskCount;
    private int LoadingMaskCount;
    public Camera UICamera;
    private Dictionary<UIType, Stack<UIBase>> UIStackDict = new Dictionary<UIType, Stack<UIBase>>();
    public static UIManager Instance;

    private void Awake() {
      foreach (UIType type in Enum.GetValues(typeof(UIType))) {
        UIStackDict.Add(type, new Stack<UIBase>());
      }
      UIMask.SetActiveEx(false);
      LoadingMask.SetActiveEx(false);
      Instance = this;
    }

    public async UniTask<T> Open<T>(UIType type, string name, bool removeLastUI = false, params object[] args) where T : UIBase {
      SetUIMaskEnable(true);
      var handle = Addressables.LoadAssetAsync<GameObject>(name);
      while (!handle.IsDone) {
        await UniTask.Yield(Game.Instance.CancellationToken);
      }
      var ui = Instantiate(handle.Result, GetUIRoot(type)).GetComponent<T>();
      ui.gameObject.SetActiveEx(false);
      Addressables.Release(handle);
      ui.name = name;
      await ui.Init(type, args);

      if (UIStackDict[type].TryPeek(out var topUI)) {
        await topUI.OnClose();
        if (removeLastUI) {
          UIStackDict[type].Pop().OnRemove();
        }
      }

      UIStackDict[type].Push(ui);
      await ui.OnOpen();
      SetUIMaskEnable(false);
      return ui;
    }

    public async UniTask<bool> Close<T>(T ui) where T : UIBase {
      if (!UIStackDict[ui.UIType].TryPeek(out var topUI) || topUI != ui) {
        return false;
      }
      SetUIMaskEnable(true);
      await UIStackDict[ui.UIType].Pop().OnClose();
      ui.OnRemove(); // TODO:优化
      if (UIStackDict[ui.UIType].TryPeek(out topUI)) {
        await topUI.OnOpen();
      }
      SetUIMaskEnable(false);
      return true;
    }

    public async UniTask<T> OpenChild<T>(UIBase parentUI, string name, params object[] args) where T : UIBase {
      SetUIMaskEnable(true);
      var childUI = await parentUI.OpenChild<T>(name, args);
      SetUIMaskEnable(false);
      return childUI;
    }

    public async UniTask<bool> CloseChild<T>(UIBase parentUI, T ui) where T : UIBase {
      SetUIMaskEnable(true);
      bool closed = await parentUI.CloseChild(ui);
      SetUIMaskEnable(false);
      return closed;
    }

    public void SetUIMaskEnable(bool enable) {
      UIMaskCount += enable ? 1 : -1;
      UIMask.SetActiveEx(UIMaskCount > 0);
    }

    public void SetLoadingMaskEnable(bool enable) {
      LoadingMaskCount += enable ? 1 : -1;
      LoadingMask.SetActiveEx(LoadingMaskCount > 0);
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