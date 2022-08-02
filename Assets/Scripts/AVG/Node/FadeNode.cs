using DG.Tweening;
using Sirenix.OdinInspector;

namespace GameCore.AVGFuncs {
  public abstract class FadeNode : EffectNode {
    [LabelText("�Ƿ�����")]
    public bool Block = true;
    [LabelText("����Ϊ�ٶ�")]
    public bool SetSpeedBased;
    [LabelText("����ʱ��")]
    public float FadeTime;
    [LabelText("���ɽ����ص�")]
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