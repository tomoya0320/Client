using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/控制/平行")]
  public class Parallel : ControllerNode {
    public override async UniTask<NodeResult> Run(Behavior behavior, Context context) {
      var connections = GetOutputPort(nameof(Out)).GetConnections();
      foreach (var connection in connections) {
        BehaviorNode behaviorNode = connection.node as BehaviorNode;
        if (behaviorNode == null) {
          Debug.LogError($"节点基类不匹配！类型:{connection.node.GetType().Name}");
          return NodeResult.False;
        }
        NodeResult nodeResult = await behaviorNode.Run(behavior, context);
        if (nodeResult == NodeResult.Break) {
          return nodeResult;
        }
      }
      return NodeResult.True;
    }
  }
}