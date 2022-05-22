using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle.BehaviorFuncs {
  public abstract class ControllerNode : BehaviorNode {
		[LabelText("��")]
		[Input(connectionType = ConnectionType.Override)]
		public NodePort In;
		[LabelText("��")]
		[Output]
		public NodePort Out;
  }
}