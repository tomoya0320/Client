using UnityEngine;
using XNode;
using Battle;

namespace BehaviorTree.Battle {
	[CreateAssetMenu(menuName = "BehaviorTree/Battle/BehaviorGraph")]
	public class BehaviorGraph : NodeGraph {
		public BattleManager BattleManager { get; private set; }

		public void Init(BattleManager battleManager) => BattleManager = battleManager;
	}
}