using Battle;
using System;
using XNode;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace BehaviorTree.Battle {
  [Serializable]
	public struct BehaviorNodePort { }

  [Serializable]
  public struct BehaviorNodeParamKey {
    public SourceType Type;
    public string Key;
  }

  [Serializable]
  public struct BehaviorNodeParam<T> {
    public BehaviorNodeParamKey ParamKey;
    public T Value;
  }

  public abstract class BehaviorNode : Node {
    [HideInInspector]
    public int Index = -1;
    protected BehaviorGraph Behavior;

		protected override void Init() {
			Behavior = graph as BehaviorGraph;
		}

    public abstract UniTask<bool> Run(BattleManager battleManager, Context context);

    public void UpdateIndexInEditor() {
      var inputPort = GetInputPort("In");
      Index = inputPort?.Connection?.GetConnectionIndex(inputPort) ?? -1;
    }
  }
}