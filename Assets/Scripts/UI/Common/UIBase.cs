using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameCore.UI {
  public abstract class UIBase : MonoBehaviour {
    [SerializeField]
    private Animation OpenAnim;
    [SerializeField]
    private Animation CloseAnim;
    [SerializeField]
    private CanvasGroup CanvasGroup;

    public abstract UIBase Init(params object[] args);

    public virtual async UniTask Open() {
      if (OpenAnim && OpenAnim.Play()) {
        await UniTask.WaitWhile(WaitOpenAnim);
      }
    }

    public virtual async UniTask Close() {
      if (CloseAnim && CloseAnim.Play()) {
        await UniTask.WaitWhile(WaitCloseAnim);
      }
    }

    private bool WaitOpenAnim() => OpenAnim.isPlaying;

    private bool WaitCloseAnim() => CloseAnim.isPlaying;
  }
}