using Sirenix.OdinInspector;

namespace GameCore.AVGFuncs {
  public abstract class EffectNode : AVGNode {
    [LabelText("入")]
    [Input(connectionType = ConnectionType.Override)]
    public NodePort In;
  }
}