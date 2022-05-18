using Battle;
using Cysharp.Threading.Tasks;

namespace BehaviorTree.Battle {
  [CreateNodeMenu("节点/行为/设置Float")]
  public class SetFloat : ActionNode {
    public NodeParam Source;
    public NodeParamKey TargetKey;

    public override UniTask<bool> Run(Behavior behavior, Context context) {
      behavior.SetFloat(TargetKey, behavior.GetFloat(Source));
      return UniTask.FromResult(true);
    }
  }
}