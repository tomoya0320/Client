using System;
using XNode;
using UnityEngine;
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
    [HideInInspector]
    public int Index = -1;
    protected BehaviorGraph BehaviorGraph;

		protected override void Init() {
      BehaviorGraph = graph as BehaviorGraph;
		}

    public abstract UniTask<NodeResult> Run(Behavior behavior, Context context);

    public void UpdateIndex() {
      var inputPort = GetInputPort("In");
      Index = inputPort?.Connection?.GetConnectionIndex(inputPort) ?? -1;
    }

    protected NodeResult BoolToNodeResult(bool result) => result ? NodeResult.True : NodeResult.False;
  }
}