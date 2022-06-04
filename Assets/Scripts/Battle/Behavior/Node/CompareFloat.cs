using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/比较整数")]
  public class CompareFloat : ActionNode {
    [LabelText("左操作数")]
    public NodeFloatParam LeftFloat;
    [LabelText("右操作数")]
    public NodeFloatParam RightFloat;
    [LabelText("比较方式")]
    public CompareMethod CompareMethod;

    public override UniTask<bool> Run(Behavior behavior, Context context) {
      float left = behavior.GetFloat(LeftFloat);
      float right = behavior.GetFloat(RightFloat);
      return UniTask.FromResult(MathUtil.Compare(left, right, CompareMethod));
    }
  }
}