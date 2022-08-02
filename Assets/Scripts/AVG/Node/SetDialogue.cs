using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.AVGFuncs {
  public class SetDialogue : FadeNode {
    [LabelText("����")]
    public string Name;
    [LabelText("�Ƿ񽫹���ʱ������Ϊ�ٶ�")]
    public bool SetSpeedBased;
    [LabelText("�Ի�����")]
    [TextArea]
    public string Dialogue;

    protected override void Init() {
      base.Init();
      Block = false;
    }

    protected override Tween DoTween(AVG avg) => avg.UI.SetDialogue(Name, Dialogue, FadeTime, SetSpeedBased);
  }
}