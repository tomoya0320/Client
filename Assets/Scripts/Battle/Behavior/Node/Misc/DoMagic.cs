using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/执行效果")]
  public class DoMagic : ActionNode {
    public string MagicId;
    [LabelText("目标单位")]
    public NodeParamKey TargetUnit;

    public async override UniTask<bool> Run(Behavior behavior, Context context) {
      Unit targetUnit = behavior.GetUnit(TargetUnit);
      if (targetUnit == null) {
        return false;
      }
      await behavior.Battle.MagicManager.DoMagic(MagicId, behavior.Unit, targetUnit, context);
      return true;
    }
  }
}