using Battle;
using UnityEngine;

namespace BehaviorTree.Battle {
	public abstract class DecoratorNode : BehaviorNode {
    [Input(connectionType = ConnectionType.Override)]
    public BehaviorPort In;
    [Output(connectionType = ConnectionType.Override)]
    public BehaviorPort Out;
  }
}