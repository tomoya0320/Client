using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/控制/顺序")]
  public class Sequence : ControllerNode {
    public override async UniTask<bool> Run(Behavior behavior, Context context) {
      var connections = GetOutputPort(nameof(Out)).GetConnections();
      foreach (var connection in connections) {
        BehaviorNode behaviorNode = connection.node as BehaviorNode;
        if (behaviorNode == null) {
          Debug.LogError($"节点基类不匹配！类型:{connection.node.GetType().Name}");
          return false;
        }
        bool result = await behaviorNode.Run(behavior, context);
        if (!result) {
          return false;
        }
      }
      return true;
    }
  }
}