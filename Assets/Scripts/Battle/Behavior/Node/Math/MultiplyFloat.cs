using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/数学/乘Float")]
  public class MultiplyFloat : ActionNode {
    [LabelText("左操作数")]
    public NodeFloatParam LeftFloat;
    [LabelText("右操作数")]
    public NodeFloatParam RightFloat;
    [LabelText("存值")]
    public NodeParamKey TargetKey;

    public override UniTask<NodeResult> Run(Behavior behavior, Context context) {
      behavior.SetFloat(TargetKey, behavior.GetFloat(LeftFloat) * behavior.GetFloat(RightFloat));
      return UniTask.FromResult(NodeResult.True);
    }
  }
}
