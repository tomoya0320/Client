using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("�ڵ�/��Ϊ/��λ/��ⵥλ�Ƿ���")]
  public class CheckUnitAlive : ActionNode {
    [LabelText("��λ")]
    public NodeParamKey UnitKey;

    public override UniTask<NodeResult> Run(Behavior behavior, Context context) {
      var unit = behavior.GetUnit(UnitKey);
      return UniTask.FromResult(unit == null ? NodeResult.False : BoolToNodeResult(unit.IsAlive));
    }
  }
}