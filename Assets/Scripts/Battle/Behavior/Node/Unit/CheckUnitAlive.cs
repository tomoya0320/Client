using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/单位/检测单位是否存活")]
  public class CheckUnitAlive : ActionNode {
    [LabelText("单位")]
    public NodeParamKey UnitKey;

    public override UniTask<NodeResult> Run(Behavior behavior, Context context) {
      var unit = behavior.GetUnit(UnitKey);
      if (unit == null) {
        return UniTask.FromResult(NodeResult.False);
      }
      return UniTask.FromResult(BoolToNodeResult(unit.IsAlive));
    }
  }
}