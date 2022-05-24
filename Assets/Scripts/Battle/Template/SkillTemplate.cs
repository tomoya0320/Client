using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace GameCore {
  [Serializable]
  public struct SKillEvent {
    [LabelText("效果Id")]
    public string MagicTemplateId;
    [LabelText("等待时间")]
    public float WaitTime; // 配合动画表现使用
    [LabelText("目标选择器")]
    public TargetSelector TargetSelector;
  }

  [CreateAssetMenu(menuName = "模板/技能")]
  public class SkillTemplate : SerializedScriptableObject {
    [LabelText("技能事件列表")]
    public SKillEvent[] SKillEvents;
  }
}