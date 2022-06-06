using UnityEngine;
using XNode;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using GameCore.BehaviorFuncs;

namespace GameCore {
  public enum DictType {
    [InspectorName("行为树")]
    Behavior,
    [InspectorName("单位")]
    Unit,
    [InspectorName("玩家")]
    Player,
    [InspectorName("战斗")]
    Battle,
  }

  public enum BehaviorTime {
    [InspectorName("无")]
    NONE,
    [InspectorName("回合开始")]
    ON_START_TURN,
    [InspectorName("回合结束")]
    ON_END_TURN,
    [InspectorName("回合中等待指令")]
    ON_TURN_WAIT_OP,
    [InspectorName("造成伤害前")]
    ON_BEFORE_DAMAGE,
    [InspectorName("造成伤害后")]
    ON_LATE_DAMAGE,
    [InspectorName("被造成伤害前")]
    ON_BEFORE_DAMAGED,
    [InspectorName("被造成伤害后")]
    ON_LATE_DAMAGED,
    [InspectorName("单位濒死时")]
    ON_UNIT_DYING,
    [InspectorName("单位死亡时")]
    ON_UNIT_DEAD,
  }

  [CreateAssetMenu(menuName = "模板/行为树/战斗通用")]
  public class BehaviorGraph : NodeGraph {
    [LabelText("触发时机")]
    public BehaviorTime BehaviorTime;

    public async UniTask Run<T>(Behavior behavior, Context context = null) where T : SingleOutNode {
      var node = nodes.Find(n => n is T) as T;
      if (node) {
        await node.Run(behavior, context);
      }
    }
  }
}