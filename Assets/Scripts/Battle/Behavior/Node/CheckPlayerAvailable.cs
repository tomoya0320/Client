using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/检查玩家是否可用")]
  public class CheckPlayerAvailable : ActionNode {
    [LabelText("玩家单位")]
    public NodeParamKey UnitKey;

    public override UniTask<bool> Run(Behavior behavior, Context context) {
      var unit = behavior.GetUnit(UnitKey);
      return UniTask.FromResult(unit.Player.Available);
    }
  }
}