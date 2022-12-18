using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI {
  public abstract class UIBase : MonoBehaviour {
    [SerializeField]
    private Animation Animation;
    [SerializeField]
    private RectTransform ChildNode;
    [SerializeField]
    private Button CloseBtn;
    public UIType UIType { get; private set; }
    public Func<bool> WaitAnimFunc { get; private set; }
    public UIBase ParentUI { get; private set; }
    protected List<UIBase> ChildUIList = new List<UIBase>();
    public bool IsChildUI => ParentUI != null;

    public virtual UniTask Init(UIType type, params object[] args) {
      UIType = type;
      WaitAnimFunc = () => Animation && Animation.isPlaying;
      if (CloseBtn) {
        CloseBtn.onClick.AddListener(async () => await Close());
      }
      return UniTask.CompletedTask;
    }

    public async UniTask OnOpen() {
      gameObject.SetActiveEx(true);
      await PlayAnim($"{GetType().Name}Open");
    }

    public async UniTask OnClose() {
      await PlayAnim($"{GetType().Name}Close");
      gameObject.SetActiveEx(false);
    }

    public virtual void OnRemove() {
      foreach (var child in ChildUIList) {
        child.ParentUI = null;
        child.OnRemove();
      }
      ChildUIList.Clear();
      Destroy(gameObject);
    }

    public async UniTask<T> OpenChild<T>(string name, params object[] args) where T : UIBase {
      var ui = Instantiate(await ResourceManager.LoadAssetAsync<GameObject>(name), ChildNode).GetComponent<T>();
      ui.name = name;
      ui.ParentUI = this;
      await ui.Init(UIType, args);
      ChildUIList.Add(ui);
      await ui.OnOpen();
      return ui;
    }

    public async UniTask<bool> CloseChild<T>(T ui) where T : UIBase {
      if (!ChildUIList.Remove(ui)) {
        return false;
      }
      ui.ParentUI = null;
      await ui.OnClose();
      ui.OnRemove(); // TODO:”≈ªØ
      return true;
    }

    public async UniTask Close() {
      if (IsChildUI) {
        await UIManager.Instance.CloseChild(this);
      } else {
        await UIManager.Instance.Close(this);
      }
    }

    private async UniTask PlayAnim(string name) {
      if (Animation && Animation.GetClip(name) && Animation.Play(name)) {
        await UniTask.WaitWhile(WaitAnimFunc, cancellationToken: Game.Instance.CancellationToken);
      }
    }
  }
}