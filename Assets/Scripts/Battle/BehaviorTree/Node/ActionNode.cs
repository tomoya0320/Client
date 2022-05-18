using UnityEngine;
using XNode;

namespace BehaviorTree.Battle {
  public abstract class ActionNode : BehaviorNode {
    [Input(connectionType = ConnectionType.Override)]
    public NodePort In;
  }
}