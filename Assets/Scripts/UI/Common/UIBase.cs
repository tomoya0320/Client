using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace GameCore.UI {
  public abstract class UIBase : MonoBehaviour {
    [SerializeField]
    private Animation Animation;
    [SerializeField]
    private CanvasGroup CanvasGroup;
    [SerializeField]
    private RectTransform ChildNode;
    public Func<bool> WaitAnimFunc { get; private set; }
    private List<UIBase> ChildUIList = new List<UIBase>();

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

    public virtual async UniTask<T> OpenChild<T>(string name, params object[] args) where T : UIBase {
      var ui = Instantiate(await Addressables.LoadAssetAsync<GameObject>(name), ChildNode).GetComponent<T>();
      ChildUIList.Add(ui.Init(args));
      await ui.OnOpen();
      return ui;
    }

    public async UniTask<bool> CloseChild<T>(T ui) where T : UIBase {
      if (!ChildUIList.Remove(ui)) {
        return false;
      }
      await ui.OnClose();
      Destroy(ui.gameObject); // TODO:�Ż�
      return true;
    }

    private async UniTask PlayAnim(string name) {
      if (Animation && Animation.GetClip(name) && Animation.Play(name)) {
        await UniTask.WaitWhile(WaitAnimFunc, cancellationToken: Game.Instance.CancellationToken);
      }
    }
  }
}