using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.BehaviorFuncs {
  public abstract class SingleOutNode : BehaviorNode {
    [LabelText("出")]
    [Output(connectionType = ConnectionType.Override)]
    public NodePort Out;

    public override async UniTask<bool> Run(Behavior behavior, Context context) {
      var connection = GetOutputPort(nameof(Out)).Connection;
      BehaviorNode behaviorNode = connection.node as BehaviorNode;
      if (behaviorNode == null) {
        Debug.LogError($"节点基类不匹配！类型:{connection.node.GetType().Name}");
        return false;
      }
      return await behaviorNode.Run(behavior, context);
    }
  }
}