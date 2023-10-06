using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
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
    private GameObject BlockMask;
    private int BlockCount;
    public Camera UICamera;
    private Dictionary<UIType, Stack<UIBase>> UIStackDict = new Dictionary<UIType, Stack<UIBase>>();
    public static UIManager Instance;

    private void Awake() {
      foreach (UIType type in Enum.GetValues(typeof(UIType))) {
        UIStackDict.Add(type, new Stack<UIBase>());
      }
      Instance = this;
    }

    public async UniTask<T> Open<T>(UIType type, string name, bool removeLastUI = false, params object[] args) where T : UIBase {
      SetBlock(true);
      var ui = Instantiate(await ResourceManager.LoadAssetAsync<GameObject>(name), GetUIRoot(type)).GetComponent<T>();
      ui.gameObject.SetActiveEx(false);
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
      SetBlock(false);
      return ui;
    }

    public async UniTask<bool> Close<T>(T ui) where T : UIBase {
      if (!UIStackDict[ui.UIType].TryPeek(out var topUI) || topUI != ui) {
        return false;
      }
      SetBlock(true);
      await UIStackDict[ui.UIType].Pop().OnClose();
      ui.OnRemove(); // TODO:优化
      if (UIStackDict[ui.UIType].TryPeek(out topUI)) {
        await topUI.OnOpen();
      }
      SetBlock(false);
      return true;
    }

    public async UniTask<T> OpenChild<T>(UIBase parentUI, string name, params object[] args) where T : UIBase {
      SetBlock(true);
      var childUI = await parentUI.OpenChild<T>(name, args);
      SetBlock(false);
      return childUI;
    }

    public async UniTask<bool> CloseChild<T>(T ui) where T : UIBase {
      SetBlock(true);
      bool closed = await ui.ParentUI.CloseChild(ui);
      SetBlock(false);
      return closed;
    }

    public void SetBlock(bool enable) {
      BlockCount += enable ? 1 : -1;
      BlockMask.SetActiveEx(BlockCount > 0);
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