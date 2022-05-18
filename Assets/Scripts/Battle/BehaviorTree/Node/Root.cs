using Battle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BehaviorTree.Battle {
  [CreateNodeMenu("�ڵ�/���ڵ�/Ĭ��")]
  public class Root : BehaviorNode {
    [Output(connectionType = ConnectionType.Override)]
    public NodePort Out;

    public override async UniTask<bool> Run(BattleManager battleManager, Context context) {
      var connection = GetOutputPort(nameof(Out)).Connection;
      BehaviorNode behaviorNode = connection.node as BehaviorNode;
      if (behaviorNode == null) {
        Debug.LogError($"�ڵ���಻ƥ�䣡����:{connection.node.GetType().Name}");
        return false;
      }
      bool result = await behaviorNode.Run(battleManager, context);
      return result;
    }
  }
}