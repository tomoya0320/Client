using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore {
  public abstract class ActionNode : AVGNode {
    [LabelText("»Î")]
    [Input(connectionType = ConnectionType.Override)]
    public NodePort In;
  }
}