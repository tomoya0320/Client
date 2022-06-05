using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.BehaviorFuncs {
  public abstract class ActionNode : BehaviorNode {
    [LabelText("入")]
    [Input(connectionType = ConnectionType.Override)]
    public NodePort In;
  }
}