using Sirenix.OdinInspector;

namespace GameCore.AVG {
  [CreateNodeMenu("节点/控制/开始")]
  public class Enter : AVGNode {
		[LabelText("出")]
		[Output(connectionType = ConnectionType.Override)]
		public NodePort Out;

    public override void Run() {
      var connection = GetOutputPort(nameof(Out)).Connection;
      AVGGraph.AVGNode = connection.node as Main;
      AVGGraph.Run();
    }
  }
}