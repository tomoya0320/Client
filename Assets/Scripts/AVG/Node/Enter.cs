using Sirenix.OdinInspector;

namespace GameCore {
  [CreateNodeMenu("节点/控制/开始")]
  public class Enter : AVGNode {
		[LabelText("出")]
		[Output(connectionType = ConnectionType.Override)]
		public NodePort Out;

    public override void Run(AVG avg) {
      var connection = GetOutputPort(nameof(Out)).Connection;
      avg.AVGNode = connection.node as AVGNode;
      avg.Run();
    }
  }
}