using UnityEngine;
using XNode;
using Battle;

namespace BehaviorTree.Battle {
  [CreateAssetMenu(menuName = "行为树/战斗")]
  public class BehaviorGraph : NodeGraph {
    public BattleManager BattleManager { get; private set; }

    public void Init(BattleManager battleManager) => BattleManager = battleManager;

    public async void Run(Context context = null) {
      var node = nodes.Find(n => n is Root) as Root;
      if (node) {
        await node.Run(BattleManager, context);
      }
    }
  }
}