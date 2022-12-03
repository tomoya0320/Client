using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/其他/延迟")]
  public class Delay : ActionNode {
    [LabelText("延迟时间")]
    public NodeFloatParam DelayTimeKey;

    public async override UniTask<NodeResult> Run(Behavior behavior, Context context) {
      float delayTime = behavior.GetFloat(DelayTimeKey);
      await UniTask.Delay((int)(GameConstant.THOUSAND * delayTime), cancellationToken: behavior.Battle.CancellationToken);
      return NodeResult.True;
    }
  }
}