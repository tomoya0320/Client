using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.BehaviorFuncs {
	public abstract class DecoratorNode : BehaviorNode {
    [LabelText("��")]
    [Input(connectionType = ConnectionType.Override)]
    public NodePort In;
    [LabelText("��")]
    [Output(connectionType = ConnectionType.Override)]
    public NodePort Out;
  }
}