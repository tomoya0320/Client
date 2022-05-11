using Battle;
using System.Threading.Tasks;
using UnityEngine;

namespace BehaviorTree.Battle {
  [CreateNodeMenu("�ڵ�/����/ƽ��")]
  public class Parallel : ControllerNode {
    public override async Task<bool> Run(BattleManager battleManager, Context context) {
      bool result = false;
      var connections = GetOutputPort(nameof(Out)).GetConnections();
      foreach (var connection in connections) {
        BehaviorNode behaviorNode = connection.node as BehaviorNode;
        if (behaviorNode == null) {
          Debug.LogError($"�ڵ���಻ƥ�䣡����:{connection.node.GetType().Name}");
          continue;
        }
        result |= await behaviorNode.Run(battleManager, context);
      }
      return result;
    }
  }
}
