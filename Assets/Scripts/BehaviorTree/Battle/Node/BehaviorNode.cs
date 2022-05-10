using Battle;
using System;
using System.Threading.Tasks;
using XNode;
using UnityEngine;

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

    public override void OnCreateConnection(NodePort from, NodePort to) {
      if (to.node is BehaviorNode behaviorNode) {
        behaviorNode.Index = from.GetConnectionIndex(to);
      }
    }

    public override void OnRemoveConnection(NodePort port) {
      if (port.IsInput) {
        Index = -1;
      } else if (port.IsOutput) {
        foreach (var nodePort in port.GetConnections()) {
          if (nodePort.node is BehaviorNode behaviorNode) {
            behaviorNode.Index = port.GetConnectionIndex(nodePort);
          }
        }
      }
    }

    public virtual async Task<bool> Run(BattleManager battleManager, Context context) => await Task.FromResult(true);
	}
}