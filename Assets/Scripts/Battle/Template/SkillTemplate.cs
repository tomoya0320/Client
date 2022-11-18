using GameCore.MagicFuncs;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  public struct SKillEvent {
    [DrawWithUnity]
    [LabelText("效果")]
    public AssetReferenceT<MagicFuncBase> Magic;
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