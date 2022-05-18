using Battle;
using Cysharp.Threading.Tasks;

namespace BehaviorTree.Battle {
  [CreateNodeMenu("�ڵ�/��Ϊ/����Int")]
  public class SetInt : ActionNode {
    public NodeParam Source;
    public NodeParamKey TargetKey;

    public override UniTask<bool> Run(Behavior behavior, Context context) {
      behavior.SetInt(TargetKey, behavior.GetInt(Source));
      return UniTask.FromResult(true);
    }
  }
}