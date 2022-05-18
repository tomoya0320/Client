using UnityEngine;
using XNode;

namespace Battle.BehaviorFuncs {
  public abstract class ActionNode : BehaviorNode {
    [Input(connectionType = ConnectionType.Override)]
    public NodePort In;
  }
}