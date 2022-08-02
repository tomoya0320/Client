using DG.Tweening;
using Sirenix.OdinInspector;

namespace GameCore.AVGFuncs {
  public abstract class FadeNode : EffectNode {
    [LabelText("是否阻塞")]
    public bool Block = true;
    [LabelText("设置为速度")]
    public bool SetSpeedBased;
    [LabelText("过渡时间")]
    public float FadeTime;
    [LabelText("过渡结束回调")]
    [Output]
    public NodePort OnFadeCompleted;

    public override void Run(AVG avg) {
      if (Block) {
        avg.Block++;
      }
      Tween tween = DoTween(avg);
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

    protected abstract Tween DoTween(AVG avg);
  }
}