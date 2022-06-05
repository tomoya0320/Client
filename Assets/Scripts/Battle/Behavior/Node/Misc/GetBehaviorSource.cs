using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("�ڵ�/��Ϊ/��ȡ��Ϊ����Դ�ĵ�λ")]
  public class GetBehaviorSource : ActionNode {
    [LabelText("��ֵ")]
    public NodeParamKey TargetKey;

    public override UniTask<bool> Run(Behavior behavior, Context context) {
      if (behavior.Unit == null) {
        return UniTask.FromResult(false);
      }
      behavior.SetInt(TargetKey, behavior.SourceUnit.RuntimeId);
      return UniTask.FromResult(true);
    }
  }
}