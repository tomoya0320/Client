using System;
using XNode;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [Serializable]
	public struct NodePort { }

  [Serializable]
  public struct NodeParamKey {
    public DictType Type;
    public string Key;
  }

  [Serializable]
  public struct NodeIntParam {
    public bool IsDict;
    [ShowIf(nameof(IsDict)), HideLabel]
    public NodeParamKey ParamKey;
    [HideIf(nameof(IsDict))]
    public int Value;
  }

  [Serializable]
  public struct NodeFloatParam {
    public bool IsDict;
    [ShowIf(nameof(IsDict)), HideLabel]
    public NodeParamKey ParamKey;
    [HideIf(nameof(IsDict))]
    public float Value;
  }

  public enum NodeResult {
    False = 0,
    Break = 1,
    True = 2,
    Max = True,
  }

  public abstract class BehaviorNode : Node {
    protected override string IndexPortName => "In";

    public abstract UniTask<NodeResult> Run(Behavior behavior, Context context);

    protected NodeResult BoolToNodeResult(bool result) => result ? NodeResult.True : NodeResult.False;
  }
}