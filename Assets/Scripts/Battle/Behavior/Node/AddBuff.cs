using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("�ڵ�/��Ϊ/��Buff")]
  public class AddBuff : ActionNode {
    public string BuffId;
    [LabelText("Ŀ�굥λ")]
    public NodeParamKey TargetUnit;

    public async override UniTask<bool> Run(Behavior behavior, Context context) {
      Unit targetUnit = behavior.GetUnit(TargetUnit);
      if (targetUnit == null) {
        return false;
      }
      await behavior.Battle.BuffManager.AddBuff(BuffId, behavior.Unit, targetUnit);
      return true;
    }
  }
}