using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore {
  public abstract class ActionNode : AVGNode {
    [LabelText("��")]
    [Input(connectionType = ConnectionType.Override)]
    public NodePort In;
  }
}