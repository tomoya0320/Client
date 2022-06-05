using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("�ڵ�/��Ϊ/��ѧ/��Int")]
  public class SubtractInt : ActionNode {
    [LabelText("�������")]
    public NodeIntParam LeftInt;
    [LabelText("�Ҳ�����")]
    public NodeIntParam RightInt;
    [LabelText("��ֵ")]
    public NodeParamKey TargetKey;

    public override UniTask<bool> Run(Behavior behavior, Context context) {
      behavior.SetInt(TargetKey, behavior.GetInt(LeftInt) - behavior.GetInt(RightInt));
      return UniTask.FromResult(true);
    }
  }
}
