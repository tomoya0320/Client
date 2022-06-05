using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.BehaviorFuncs {
	public abstract class DecoratorNode : BehaviorNode {
    [LabelText("入")]
    [Input(connectionType = ConnectionType.Override)]
    public NodePort In;
    [LabelText("出")]
    [Output(connectionType = ConnectionType.Override)]
    public NodePort Out;
  }
}