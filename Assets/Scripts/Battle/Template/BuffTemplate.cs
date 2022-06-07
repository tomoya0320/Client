using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore {
  [CreateAssetMenu(menuName = "模板/Buff")]
  public class BuffTemplate : ScriptableObject {
    public string MagicId;
    [LabelText("立即移除")]
    public bool RemoveImmediately;
    [HideIf(nameof(RemoveImmediately))]
    [LabelText("持续回合数")]
    public int Duration;
    [HideIf(nameof(RemoveImmediately))]
    [LabelText("更新回合数时机")]
    public TrickTime UpdateTime;
  }
}