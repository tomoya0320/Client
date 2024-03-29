using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/数学/比较Int")]
  public class CompareInt : ActionNode {
    [LabelText("左操作数")]
    public NodeIntParam LeftInt;
    [LabelText("右操作数")]
    public NodeIntParam RightInt;
    [LabelText("比较方式")]
    public CompareMethod CompareMethod;

    public override UniTask<NodeResult> Run(Behavior behavior, Context context) {
      int left = behavior.GetInt(LeftInt);
      int right = behavior.GetInt(RightInt);
      return UniTask.FromResult(BoolToNodeResult(MathUtil.Compare(left, right, CompareMethod)));
    }
  }
}