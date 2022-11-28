using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/数学/乘Int")]
  public class MultiplyInt : ActionNode {
    [LabelText("左操作数")]
    public NodeIntParam LeftInt;
    [LabelText("右操作数")]
    public NodeIntParam RightInt;
    [LabelText("存值")]
    public NodeParamKey TargetKey;

    public override UniTask<NodeResult> Run(Behavior behavior, Context context) {
      behavior.SetInt(TargetKey, behavior.GetInt(LeftInt) * behavior.GetInt(RightInt));
      return UniTask.FromResult(NodeResult.True);
    }
  }
}