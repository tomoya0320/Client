using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/其他/加Buff")]
  public class AddBuff : ActionNode {
    public string BuffId;
    [LabelText("目标单位")]
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