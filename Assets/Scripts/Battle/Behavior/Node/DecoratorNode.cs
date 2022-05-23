using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.BehaviorFuncs {
	public abstract class DecoratorNode : BehaviorNode {
    [LabelText("Èë")]
    [Input(connectionType = ConnectionType.Override)]
    public NodePort In;
    [LabelText("³ö")]
    [Output(connectionType = ConnectionType.Override)]
    public NodePort Out;
  }
}