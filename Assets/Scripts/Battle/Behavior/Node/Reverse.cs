using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.BehaviorFuncs {
  [CreateNodeMenu("节点/修饰/取反")]
  public class Reverse : DecoratorNode {
    public override async UniTask<bool> Run(Behavior behavior, Context context) {
      var connection = GetOutputPort(nameof(Out)).Connection;
      BehaviorNode behaviorNode = connection.node as BehaviorNode;
      if (behaviorNode == null) {
        Debug.LogError($"节点基类不匹配！类型:{connection.node.GetType().Name}");
        return false;
      }
      bool result = await behaviorNode.Run(behavior, context);
      return !result;
    }
  }
}