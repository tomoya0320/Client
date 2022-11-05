using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/数学/比较Float")]
  public class CompareFloat : ActionNode {
    [LabelText("左操作数")]
    public NodeFloatParam LeftFloat;
    [LabelText("右操作数")]
    public NodeFloatParam RightFloat;
    [LabelText("比较方式")]
    public CompareMethod CompareMethod;

    public override UniTask<NodeResult> Run(Behavior behavior, Context context) {
      float left = behavior.GetFloat(LeftFloat);
      float right = behavior.GetFloat(RightFloat);
      return UniTask.FromResult(BoolToNodeResult(MathUtil.Compare(left, right, CompareMethod)));
    }
  }
}