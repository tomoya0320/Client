using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/设置Float")]
  public class SetFloat : ActionNode {
    [LabelText("数据源")]
    public NodeParam Source;
    [LabelText("存值")]
    public NodeParamKey TargetKey;

    public override UniTask<bool> Run(Behavior behavior, Context context) {
      behavior.SetFloat(TargetKey, behavior.GetFloat(Source));
      return UniTask.FromResult(true);
    }
  }
}