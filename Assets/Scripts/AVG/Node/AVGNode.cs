using System;
using XNode;

namespace GameCore.AVGFuncs {
	[Serializable]
	public struct NodePort { }

	public abstract class AVGNode : Node {
		protected override string IndexPortName => "In";

		public abstract void Run(AVG avg);
	}
}