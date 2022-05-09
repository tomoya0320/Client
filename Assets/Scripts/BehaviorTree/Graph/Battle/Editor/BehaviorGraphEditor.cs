using System;
using XNodeEditor;

namespace BehaviorTree.Battle {
	[CustomNodeGraphEditor(typeof(BehaviorGraph))]
	public class BehaviorGraphEditor : NodeGraphEditor {

		/// <summary> 
		/// Overriding GetNodeMenuName lets you control if and how nodes are categorized.
		/// In this example we are sorting out all node types that are not in the XNode.Examples namespace.
		/// </summary>
		public override string GetNodeMenuName(Type type) {
			if (type.Namespace == "BehaviorTree.Battle") {
				return base.GetNodeMenuName(type);
			} else return null;
		}
	}
}