using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/执行效果")]
  public class DoMagic : ActionNode {
    [LabelText("效果Id")]
    public string MagicId;
    [LabelText("目标单位")]
    public NodeParamKey TargetUnit;

    public override UniTask<bool> Run(Behavior behavior, Context context) {
      Unit targetUnit = behavior.GetUnit(TargetUnit);
      bool result = behavior.BattleManager.MagicManager.DoMagic(MagicId, behavior.Unit, targetUnit, context);
      return UniTask.FromResult(result);
    }
  }
}