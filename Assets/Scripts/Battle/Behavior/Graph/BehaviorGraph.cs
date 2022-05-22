using UnityEngine;
using XNode;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Battle.BehaviorFuncs;

namespace Battle {
  public enum DictType {
    [InspectorName("行为树")]
    Behavior,
    [InspectorName("卡牌")]
    Card,
    [InspectorName("单位")]
    Unit,
    [InspectorName("玩家")]
    Player,
    [InspectorName("战斗")]
    Battle,
  }

  public enum BehaviorTime {
    [InspectorName("回合开始时")]
    ON_BEFORE_TURN,
    [InspectorName("回合结束时")]
    ON_LATE_TURN,
  }

  [CreateAssetMenu(menuName = "行为树/战斗")]
  public class BehaviorGraph : NodeGraph {
    [ReadOnly]
    public string BehaviorId;
    public BehaviorTime BehaviorTime;

    public async UniTask Run(Behavior behavior, Context context = null) {
      var node = nodes.Find(n => n is Root) as BehaviorNode;
      if (node) {
        await node.Run(behavior, context);
      }
    }
  }
}