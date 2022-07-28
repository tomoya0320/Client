using System;
using XNode;

namespace GameCore.AVG {
	[Serializable]
	public struct NodePort { }

	public abstract class AVGNode : Node {
		protected AVGGraph AVGGraph;

		protected override void Init() {
			base.Init();

			AVGGraph = graph as AVGGraph;
		}

		public abstract void Run();
	}
}