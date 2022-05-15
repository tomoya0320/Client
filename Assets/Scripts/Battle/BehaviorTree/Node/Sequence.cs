using Battle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BehaviorTree.Battle {
  [CreateNodeMenu("�ڵ�/����/˳��")]
  public class Sequence : ControllerNode {
    public override async UniTask<bool> Run(BattleManager battleManager, Context context) {
      var connections = GetOutputPort(nameof(Out)).GetConnections();
      foreach (var connection in connections) {
        BehaviorNode behaviorNode = connection.node as BehaviorNode;
        if (behaviorNode == null) {
          Debug.LogError($"�ڵ���಻ƥ�䣡����:{connection.node.GetType().Name}");
          return false;
        }
        bool result = await behaviorNode.Run(battleManager, context);
        if (!result) {
          return false;
        }
      }
      return true;
    }
  }
}