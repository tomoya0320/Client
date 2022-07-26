using Cysharp.Threading.Tasks;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/行为树/移除当前行为树")]
  public class RemoveBehavior : ActionNode {
    public override async UniTask<NodeResult> Run(Behavior behavior, Context context) {
      if (await behavior.Battle.BehaviorManager.Remove(behavior.RuntimeId)) {
        return NodeResult.Break;
      }

      return NodeResult.False;
    }
  }
}