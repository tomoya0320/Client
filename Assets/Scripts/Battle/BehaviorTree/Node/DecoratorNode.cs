using Battle;
using UnityEngine;

namespace BehaviorTree.Battle {
	public abstract class DecoratorNode : BehaviorNode {
    [Input(connectionType = ConnectionType.Override)]
    public BehaviorNodePort In;
    [Output(connectionType = ConnectionType.Override)]
    public BehaviorNodePort Out;
  }
}