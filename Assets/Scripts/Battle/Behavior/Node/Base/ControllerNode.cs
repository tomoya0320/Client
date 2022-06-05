using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.BehaviorFuncs {
  public abstract class ControllerNode : BehaviorNode {
		[LabelText("入")]
		[Input(connectionType = ConnectionType.Override)]
		public NodePort In;
		[LabelText("出")]
		[Output]
		public NodePort Out;
  }
}