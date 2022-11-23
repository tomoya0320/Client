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
    public Func<bool> WaitAnimFunc { get; private set; }
    protected UIBase ParentUI;
    protected List<UIBase> ChildUIList = new List<UIBase>();
    public bool IsChildUI => ParentUI != null;

    public virtual UIBase Init(params object[] args) {
      WaitAnimFunc = () => Animation && Animation.isPlaying;
      return this;
    }

    public virtual async UniTask OnOpen() {
      await PlayAnim($"{GetType().Name}Open");
    }

    public virtual async UniTask OnClose() {
      await PlayAnim($"{GetType().Name}Close");
    }

    public virtual void OnDestroy() { } // TODO:优化 可能改成OnRecycle

    public virtual async UniTask<T> OpenChild<T>(string name, params object[] args) where T : UIBase {
      var handle = Addressables.LoadAssetAsync<GameObject>(name);
      while (!handle.IsDone) {
        await UniTask.Yield(Game.Instance.CancellationToken);
      }
      var ui = Instantiate(handle.Result, ChildNode).GetComponent<T>();
      ui.name = name;
      ui.ParentUI = this;
      ChildUIList.Add(ui.Init(args));
      await ui.OnOpen();
      return ui;
    }

    public async UniTask<bool> CloseChild<T>(T ui) where T : UIBase {
      if (!ChildUIList.Remove(ui)) {
        return false;
      }
      ui.ParentUI = null;
      await ui.OnClose();
      Destroy(ui.gameObject); // TODO:优化
      return true;
    }

    private async UniTask PlayAnim(string name) {
      if (Animation && Animation.GetClip(name) && Animation.Play(name)) {
        await UniTask.WaitWhile(WaitAnimFunc, cancellationToken: Game.Instance.CancellationToken);
      }
    }
  }
}