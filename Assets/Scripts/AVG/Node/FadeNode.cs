using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.AVG {
  public abstract class FadeNode : ActionNode {
    [LabelText("是否阻塞")]
    [SerializeField]
    protected bool Block = true;
    [LabelText("过渡时间")]
    [SerializeField]
    protected float FadeTime;
    [LabelText("过渡结束回调节点")]
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