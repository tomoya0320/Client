using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/玩家/检查玩家是否是我方")]
  public class CheckPlayerIsSelf : ActionNode {
    [LabelText("玩家单位")]
    public NodeParamKey UnitKey;

    public override UniTask<NodeResult> Run(Behavior behavior, Context context) {
      var unit = behavior.GetUnit(UnitKey);
      if (unit == null) {
        return UniTask.FromResult(NodeResult.False);
      }
      return UniTask.FromResult(BoolToNodeResult(unit.Player.IsSelf));
    }
  }
}