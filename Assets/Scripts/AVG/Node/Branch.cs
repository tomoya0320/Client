using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.AVGFuncs {
	[CreateNodeMenu("节点/控制/分支")]
	public class Branch : AVGNode {
		[LabelText("入")]
		[Input]
		public NodePort In;
		[LabelText("选项")]
		[TextArea]
		[Output(dynamicPortList = true)]
		public string[] Options;

		public override void Run(AVG avg) {
			avg.Block++;
			avg.UI.SetOptions(Options, index => {
				var connection = GetOutputPort($"{nameof(Options)} {index}").Connection;
				avg.AVGNode = connection.node as AVGNode;
				avg.Block--;
				avg.Run();
			});
		}
	}
}