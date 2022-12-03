using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/控制/循环")]
  public class Loop : ControllerNode {
    [LabelText("循环次数")]
    public NodeIntParam CountKey;

    public override async UniTask<NodeResult> Run(Behavior behavior, Context context) {
      int count = behavior.GetInt(CountKey);
      var connections = GetOutputPort(nameof(Out)).GetConnections();
      while (count-- > 0) {
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
      }
      return NodeResult.True;
    }
  }
}