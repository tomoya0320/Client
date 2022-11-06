using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore {
  [CreateAssetMenu(menuName = "模板/Buff")]
  public class BuffTemplate : ScriptableObject {
    [LabelText("效果Id")]
    public string MagicId;
    [LabelText("间隔触发效果Id")]
    public string IntervalMagicId;
    [LabelText("Buff类型")]
    public string BuffKind;
    [LabelText("免疫类型")]
    public List<string> ImmuneKinds;
    [LabelText("延迟回合数")]
    public int Delay;
    [LabelText("持续回合数")]
    public int Duration;
    [LabelText("更新回合数时机")]
    public TickTime UpdateTime;

    public int TotalDuration => Delay > 0 ? Delay + Duration : Duration;
  }
}