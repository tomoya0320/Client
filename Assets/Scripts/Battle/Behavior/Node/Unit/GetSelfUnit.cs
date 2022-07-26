using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/单位/获取我方当前单位")]
  public class GetSelfUnit : ActionNode {
    [LabelText("存值")]
    public NodeParamKey TargetKey;

    public override UniTask<NodeResult> Run(Behavior behavior, Context context) {
      var targetUnit = behavior.Battle.SelfPlayer.Master;
      if (targetUnit == null) {
        return UniTask.FromResult(NodeResult.False);
      }
      behavior.SetInt(TargetKey, targetUnit.RuntimeId);
      return UniTask.FromResult(NodeResult.True);
    }
  }
}