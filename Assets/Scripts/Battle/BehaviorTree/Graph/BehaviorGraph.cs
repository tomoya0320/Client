using UnityEngine;
using XNode;
using Battle;
using Cysharp.Threading.Tasks;

namespace BehaviorTree.Battle {
  [CreateAssetMenu(menuName = "行为树/战斗")]
  public class BehaviorGraph : NodeGraph {
    [HideInInspector]
    public string BehaviorId;
    public BattleManager BattleManager { get; private set; }

    public void Init(BattleManager battleManager) => BattleManager = battleManager;

    public async UniTask Run(Context context = null) {
      var node = nodes.Find(n => n is Root) as BehaviorNode;
      if (node) {
        await node.Run(BattleManager, context);
      }
    }
  }
}