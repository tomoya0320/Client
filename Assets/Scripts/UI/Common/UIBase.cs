using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace GameCore.UI {
  public abstract class UIBase : MonoBehaviour {
    [SerializeField]
    private Animation Animation;
    [SerializeField]
    private CanvasGroup CanvasGroup;
    private Func<bool> WaitAnimFunc;

    public virtual UIBase Init(params object[] args) {
      WaitAnimFunc = () => Animation && Animation.isPlaying;
      return this;
    }

    public virtual async UniTask Open() {
      await PlayAnim($"{GetType().Name}Open");
    }

    public virtual async UniTask Close() {
      await PlayAnim($"{GetType().Name}Close");
    }

    private async UniTask PlayAnim(string name) {
      if (Animation && Animation.GetClip(name) && Animation.Play(name)) {
        await UniTask.WaitWhile(WaitAnimFunc, cancellationToken: Game.Instance.CancellationToken);
      }
    }
  }
}