using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.AVG {
  public abstract class FadeNode : ActionNode {
    [LabelText("�Ƿ�����")]
    [SerializeField]
    protected bool Block = true;
    [LabelText("����ʱ��")]
    [SerializeField]
    protected float FadeTime;
    [LabelText("���ɽ����ص��ڵ�")]
    public AVGNode[] OnFadeCompletedNode;

    public override void Run() {
      if (Block) {
        AVGGraph.Block++;
      }
      Tween tween = DoTween(FadeTime);
      tween.OnComplete(() => {
        AVGGraph.Tweens.Remove(tween);
        foreach (var node in OnFadeCompletedNode) {
          node?.Run();
        }
        if (Block) {
          AVGGraph.Block--;
        }
      });
      AVGGraph.Tweens.Add(tween);
    }

    protected abstract Tween DoTween(float fadeTime);
  }
}