using Battle;
using System;
using XNode;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace BehaviorTree.Battle {
  [Serializable]
	public sealed class BehaviorPort { }

	public abstract class BehaviorNode : Node {
    [HideInInspector]
    public int Index = -1;
    protected BehaviorGraph behavior;

		protected override void Init() {
			behavior = graph as BehaviorGraph;
		}

    public abstract UniTask<bool> Run(BattleManager battleManager, Context context);

    public void UpdateIndexInEditor() {
      var inputPort = GetInputPort("In");
      Index = inputPort?.Connection?.GetConnectionIndex(inputPort) ?? -1;
    }
  }
}