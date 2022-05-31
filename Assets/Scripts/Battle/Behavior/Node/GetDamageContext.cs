using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("�ڵ�/��Ϊ/��ȡ�˺�������")]
  public class GetDamageContext : ActionNode {
    [LabelText("��Դ��λ")]
    public NodeParamKey SourceKey;
    [LabelText("Ŀ�굥λ")]
    public NodeParamKey TargetKey;
    [LabelText("�˺�ֵ")]
    public NodeParamKey DamageValueKey;

    public override UniTask<bool> Run(Behavior behavior, Context context) {
      if(context is DamageContext damageContext) {
        behavior.SetInt(SourceKey, damageContext.Source.RuntimeId);
        behavior.SetInt(TargetKey, damageContext.Target.RuntimeId);
        behavior.SetInt(DamageValueKey, damageContext.DamageValue);
        return UniTask.FromResult(true);
      }
      return UniTask.FromResult(false);
    }
  }
}