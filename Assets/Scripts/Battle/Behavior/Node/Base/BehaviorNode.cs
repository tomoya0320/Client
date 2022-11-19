using System;
using XNode;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [Serializable]
	public struct NodePort { }

  [Serializable]
  public struct NodeParamKey {
    [LabelText("字典类型")]
    public DictType Type;
    [LabelText("字典值名")]
    public string Key;
  }

  [Serializable]
  public struct NodeIntParam {
    [LabelText("字典值?")]
    public bool IsDict;
    [HideLabel]
    [ShowIf(nameof(IsDict))]
    public NodeParamKey ParamKey;
    [LabelText("固定值")]
    [HideIf(nameof(IsDict))]
    public int Value;
  }

  [Serializable]
  public struct NodeFloatParam {
    [LabelText("字典值?")]
    public bool IsDict;
    [HideLabel]
    [ShowIf(nameof(IsDict))]
    public NodeParamKey ParamKey;
    [LabelText("固定值")]
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