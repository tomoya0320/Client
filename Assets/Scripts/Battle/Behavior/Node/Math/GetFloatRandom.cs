using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/数学/获取随机Float")]
  public class GetFloatRandom : ActionNode {
    [LabelText("最小随机值")]
    public NodeFloatParam MinRange;
    [LabelText("最大随机值")]
    public NodeFloatParam MaxRange;
    [LabelText("存值")]
    public NodeParamKey TargetKey;

    public override UniTask<NodeResult> Run(Behavior behavior, Context context) {
      float minRange = behavior.GetFloat(MinRange);
      float maxRange = behavior.GetFloat(MaxRange);
      behavior.SetFloat(TargetKey, Random.Range(minRange, maxRange));
      return UniTask.FromResult(NodeResult.True);
    }
  }
}