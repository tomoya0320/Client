using Battle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BehaviorTree.Battle {
  [CreateNodeMenu("�ڵ�/����/ȡ��")]
  public class Reverse : DecoratorNode {
    public override async UniTask<bool> Run(Behavior behavior, Context context) {
      var connection = GetOutputPort(nameof(Out)).Connection;
      BehaviorNode behaviorNode = connection.node as BehaviorNode;
      if (behaviorNode == null) {
        Debug.LogError($"�ڵ���಻ƥ�䣡����:{connection.node.GetType().Name}");
        return false;
      }
      bool result = await behaviorNode.Run(behavior, context);
      return !result;
    }
  }
}