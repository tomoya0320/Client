using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BehaviorTree.Battle {
	public abstract class BehaviorNode : Node {
		protected BehaviorGraph behavior;

		protected override void Init() {
			behavior = graph as BehaviorGraph;
		}
	}
}