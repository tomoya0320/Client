using Cysharp.Threading.Tasks;
using GameCore.MagicFuncs;
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/其他/执行效果")]
  public class DoMagic : ActionNode {
    public AssetReferenceT<MagicFuncBase> Magic;
    [LabelText("目标单位")]
    public NodeParamKey TargetUnit;

    public async override UniTask<NodeResult> Run(Behavior behavior, Context context) {
      Unit targetUnit = behavior.GetUnit(TargetUnit);
      if (targetUnit == null) {
        return NodeResult.False;
      }
      await behavior.Battle.MagicManager.DoMagic(Magic?.Asset as MagicFuncBase, behavior.Unit, targetUnit, context);
      return NodeResult.True;
    }
  }
}