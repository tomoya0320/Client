using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.AVGFuncs {
  public abstract class FadeNode : EffectNode {
    [LabelText("是否阻塞")]
    [SerializeField]
    protected bool Block = true;
    [LabelText("过渡时间")]
    [SerializeField]
    protected float FadeTime;
    [LabelText("过渡结束回调")]
    [Output]
    public NodePort OnFadeCompleted;

    public override void Run(AVG avg) {
      if (Block) {
        avg.Block++;
      }
      Tween tween = DoTween(FadeTime);
      tween.OnComplete(() => {
        avg.Tweens.Remove(tween);
        var connections = GetOutputPort(nameof(OnFadeCompleted)).GetConnections();
        foreach (var connection in connections) {
          (connection.node as AVGNode)?.Run(avg);
        }
        if (Block) {
          avg.Block--;
        }
      });
      avg.Tweens.Add(tween);
    }

    protected abstract Tween DoTween(float fadeTime);
  }
}