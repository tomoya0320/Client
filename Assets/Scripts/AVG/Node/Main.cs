using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.AVG {
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
    public NodePort ActionNodes;

    public override void Run() {
      var connections = GetOutputPort(nameof(ActionNodes)).GetConnections();
      foreach (var connection in connections) {
        (connection.node as ActionNode)?.Run();
      }
      AVGGraph.AVGNode = GetOutputPort(nameof(Out)).Connection?.node as AVGNode;
    }
  }
}