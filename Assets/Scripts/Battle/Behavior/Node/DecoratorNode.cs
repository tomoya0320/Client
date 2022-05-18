using UnityEngine;

namespace Battle.BehaviorFuncs {
	public abstract class DecoratorNode : BehaviorNode {
    [Input(connectionType = ConnectionType.Override)]
    public NodePort In;
    [Output(connectionType = ConnectionType.Override)]
    public NodePort Out;
  }
}