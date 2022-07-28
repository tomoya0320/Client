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
    [LabelText("节点列表")]
    public AVGNode[] Nodes;

    public override void Run() {
      foreach (var node in Nodes) {
        node.Run();
      }
      AVGGraph.AVGNode = GetOutputPort(nameof(Out)).Connection?.node as AVGNode;
    }
  }
}