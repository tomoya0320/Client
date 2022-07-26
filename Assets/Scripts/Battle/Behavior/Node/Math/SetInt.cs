using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/数学/设置Int")]
  public class SetInt : ActionNode {
    [LabelText("数据源")]
    public NodeIntParam Source;
    [LabelText("存值")]
    public NodeParamKey TargetKey;

    public override UniTask<NodeResult> Run(Behavior behavior, Context context) {
      behavior.SetInt(TargetKey, behavior.GetInt(Source));
      return UniTask.FromResult(NodeResult.True);
    }
  }
}