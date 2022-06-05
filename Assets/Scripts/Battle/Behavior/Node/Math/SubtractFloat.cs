using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("�ڵ�/��Ϊ/��ѧ/��Float")]
  public class SubtractFloat : ActionNode {
    [LabelText("�������")]
    public NodeFloatParam LeftFloat;
    [LabelText("�Ҳ�����")]
    public NodeFloatParam RightFloat;
    [LabelText("��ֵ")]
    public NodeParamKey TargetKey;

    public override UniTask<bool> Run(Behavior behavior, Context context) {
      behavior.SetFloat(TargetKey, behavior.GetFloat(LeftFloat) - behavior.GetFloat(RightFloat));
      return UniTask.FromResult(true);
    }
  }
}
