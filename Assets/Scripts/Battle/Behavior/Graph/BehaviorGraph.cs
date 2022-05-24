using UnityEngine;
using XNode;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using GameCore.BehaviorFuncs;

namespace GameCore {
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
    [InspectorName("造成伤害前")]
    ON_BEFORE_DAMAGE,
    [InspectorName("造成伤害后")]
    ON_LATE_DAMAGE,
    [InspectorName("被造成伤害前")]
    ON_BEFORE_DAMAGED,
    [InspectorName("被造成伤害后")]
    ON_LATE_DAMAGED,
  }

  [CreateAssetMenu(menuName = "模板/行为树/战斗通用")]
  public class BehaviorGraph : NodeGraph {
    [LabelText("触发时机")]
    public BehaviorTime BehaviorTime;

    public async UniTask Run(Behavior behavior, Context context = null) {
      var node = nodes.Find(n => n is Root) as BehaviorNode;
      if (node) {
        await node.Run(behavior, context);
      }
    }
  }
}