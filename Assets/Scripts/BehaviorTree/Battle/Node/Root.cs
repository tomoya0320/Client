using Battle;
using System.Threading.Tasks;
using UnityEngine;

namespace BehaviorTree.Battle {
  [CreateNodeMenu("节点/根节点")]
  public class Root : BehaviorNode {
    [Output(connectionType = ConnectionType.Override)]
    public int Out;

    public override async Task<bool> Run(BattleManager battleManager, Context context) {
      var connection = GetOutputPort(nameof(Out)).Connection;
      BehaviorNode behaviorNode = connection.node as BehaviorNode;
      if (behaviorNode == null) {
        Debug.LogError($"节点基类不匹配！类型:{connection.node.GetType().Name}");
        return false;
      }
      bool result = await behaviorNode.Run(battleManager, context);
      return result;
    }
  }
}