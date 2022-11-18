using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/其他/加Buff")]
  public class AddBuff : ActionNode {
    public AssetReferenceT<BuffTemplate> Buff;
    [LabelText("目标单位")]
    public NodeParamKey TargetUnit;

    public async override UniTask<NodeResult> Run(Behavior behavior, Context context) {
      Unit targetUnit = behavior.GetUnit(TargetUnit);
      if (targetUnit == null) {
        return NodeResult.False;
      }
      Buff buff = await behavior.Battle.BuffManager.AddBuff(Buff?.AssetGUID, behavior.Unit, targetUnit);
      return BoolToNodeResult(buff != null);
    }
  }
}