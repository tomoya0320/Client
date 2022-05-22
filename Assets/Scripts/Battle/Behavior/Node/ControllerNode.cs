using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle.BehaviorFuncs {
  public abstract class ControllerNode : BehaviorNode {
		[LabelText("Èë")]
		[Input(connectionType = ConnectionType.Override)]
		public NodePort In;
		[LabelText("³ö")]
		[Output]
		public NodePort Out;
  }
}