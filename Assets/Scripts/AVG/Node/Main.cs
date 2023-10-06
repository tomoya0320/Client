using Sirenix.OdinInspector;

namespace GameCore.AVGFuncs {
  [CreateNodeMenu("节点/控制/主流程")]
  public class Main : AVGNode {
    [LabelText("入")]
    [Input]
    public NodePort In;
    [LabelText("出")]
    [Output(connectionType = ConnectionType.Override)]
    public NodePort Out;
    [LabelText("效果")]
    [Output]
    public NodePort Effect;

    public override void Run(AVG avg) {
      var connections = GetOutputPort(nameof(Effect)).GetConnections();
      foreach (var connection in connections) {
        (connection.node as AVGNode)?.Run(avg);
      }
      avg.AVGNode = GetOutputPort(nameof(Out)).Connection?.node as AVGNode;
    }
  }
}