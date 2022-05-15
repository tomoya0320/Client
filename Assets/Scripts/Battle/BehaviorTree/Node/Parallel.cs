using Battle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BehaviorTree.Battle {
  [CreateNodeMenu("节点/控制/平行")]
  public class Parallel : ControllerNode {
    public override async UniTask<bool> Run(BattleManager battleManager, Context context) {
      var connections = GetOutputPort(nameof(Out)).GetConnections();
      foreach (var connection in connections) {
        BehaviorNode behaviorNode = connection.node as BehaviorNode;
        if (behaviorNode == null) {
          Debug.LogError($"节点基类不匹配！类型:{connection.node.GetType().Name}");
          continue;
        }
        await behaviorNode.Run(battleManager, context);
      }
      return true;
    }
  }
}
