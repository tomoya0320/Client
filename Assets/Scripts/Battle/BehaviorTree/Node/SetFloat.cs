using Battle;
using Cysharp.Threading.Tasks;

namespace BehaviorTree.Battle {
  [CreateNodeMenu("节点/行为/设置Float")]
  public class SetFloat : ActionNode {
    public BehaviorNodeParam<float> Source;
    public BehaviorNodeParamKey TargetValue;

    public override UniTask<bool> Run(BattleManager battleManager, Context context) {
      Behavior.SetFloat(TargetValue, Behavior.GetFloat(Source));
      return UniTask.FromResult(true);
    }
  }
}