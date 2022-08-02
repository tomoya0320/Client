using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.AVGFuncs {
  [CreateNodeMenu("节点/效果/设置对话")]
  public class SetDialogue : FadeNode {
    [LabelText("名字")]
    public string Name;
    [LabelText("对话内容")]
    [TextArea]
    public string Dialogue;

    protected override void Init() {
      base.Init();
      Block = false;
    }

    protected override Tween DoTween(AVG avg) => avg.UI.SetDialogue(Name, Dialogue, FadeTime, SetSpeedBased);
  }
}