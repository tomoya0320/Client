using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/上下文/获取伤害上下文")]
  public class GetDamageContext : ActionNode {
    [LabelText("来源单位")]
    public NodeParamKey SourceKey;
    [LabelText("目标单位")]
    public NodeParamKey TargetKey;
    [LabelText("伤害值")]
    public NodeParamKey DamageValueKey;

    public override UniTask<NodeResult> Run(Behavior behavior, Context context) {
      if (context is not DamageContext damageContext) return UniTask.FromResult(NodeResult.False);
      behavior.SetInt(SourceKey, damageContext.Source.RuntimeId);
      behavior.SetInt(TargetKey, damageContext.Target.RuntimeId);
      behavior.SetInt(DamageValueKey, damageContext.DamageValue);
      return UniTask.FromResult(NodeResult.True);
    }
  }
}