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

  [CreateAssetMenu(menuName = "模板/行为树/战斗通用")]
  public class BehaviorGraph : NodeGraph {
    [LabelText("触发时机")]
    public TickTime BehaviorTime;
    [LabelText("优先级(越大越先执行)")]
    public int Priority;

    public async UniTask Run<T>(Behavior behavior, Context context = null) where T : SingleOutNode {
      var node = nodes.Find(n => n is T) as T;
      if (node) {
        await node.Run(behavior, context);
      }
    }
  }
}