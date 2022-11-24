using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore.UI {
  public abstract class UIBase : MonoBehaviour {
    [SerializeField]
    private Animation Animation;
    [SerializeField]
    private RectTransform ChildNode;
    public UIType UIType { get; private set; }
    public Func<bool> WaitAnimFunc { get; private set; }
    protected UIBase ParentUI;
    protected List<UIBase> ChildUIList = new List<UIBase>();
    public bool IsChildUI => ParentUI != null;

    public virtual UIBase Init(UIType type, params object[] args) {
      UIType = type;
      WaitAnimFunc = () => Animation && Animation.isPlaying;
      return this;
    }

    public virtual async UniTask OnOpen() {
      gameObject.SetActiveEx(true);
      await PlayAnim($"{GetType().Name}Open");
    }

    public virtual async UniTask OnClose() {
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

    public virtual async UniTask<T> OpenChild<T>(string name, params object[] args) where T : UIBase {
      var handle = Addressables.LoadAssetAsync<GameObject>(name);
      while (!handle.IsDone) {
        await UniTask.Yield(Game.Instance.CancellationToken);
      }
      var ui = Instantiate(handle.Result, ChildNode).GetComponent<T>();
      Addressables.Release(handle);
      ui.name = name;
      ui.ParentUI = this;
      ChildUIList.Add(ui.Init(UIType, args));
      await ui.OnOpen();
      return ui;
    }

    public async UniTask<bool> CloseChild<T>(T ui) where T : UIBase {
      if (!ChildUIList.Remove(ui)) {
        return false;
      }
      ui.ParentUI = null;
      await ui.OnClose();
      ui.OnRemove(); // TODO:�Ż�
      return true;
    }

    private async UniTask PlayAnim(string name) {
      if (Animation && Animation.GetClip(name) && Animation.Play(name)) {
        await UniTask.WaitWhile(WaitAnimFunc, cancellationToken: Game.Instance.CancellationToken);
      }
    }
  }
}