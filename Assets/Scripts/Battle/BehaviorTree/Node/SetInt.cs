using Battle;
using Cysharp.Threading.Tasks;

namespace BehaviorTree.Battle {
  [CreateNodeMenu("节点/行为/设置Int")]
  public class SetInt : ActionNode {
    public BehaviorNodeParam<int> Source;
    public BehaviorNodeParamKey TargetKey;

    public override UniTask<bool> Run(BattleManager battleManager, Context context) {
      Behavior.SetInt(TargetKey, Behavior.GetInt(Source));
      return UniTask.FromResult(true);
    }
  }
}