using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.AVGFuncs {
  public class SetDialogue : FadeNode {
    [LabelText("名字")]
    public string Name;
    [LabelText("是否将过渡时间设置为速度")]
    public bool SetSpeedBased;
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