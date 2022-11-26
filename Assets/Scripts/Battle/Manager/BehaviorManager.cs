using Cysharp.Threading.Tasks;
using GameCore.BehaviorFuncs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore {
  public enum TickTime {
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
    [InspectorName("单位将要死亡时")]
    ON_UNIT_WILL_DIE,
    [InspectorName("单位死亡时")]
    ON_UNIT_DEAD,
  }

  public class BehaviorManager : BattleResManager<BehaviorGraph> {
    private int IncId;
    private Dictionary<int, Behavior> Behaviors = new Dictionary<int, Behavior>();
    private Dictionary<TickTime, List<Behavior>> BehaviorTimes = new Dictionary<TickTime, List<Behavior>>();

    public BehaviorManager(Battle battle) : base(battle) {
      foreach (TickTime behaviorTime in Enum.GetValues(typeof(TickTime))) {
        BehaviorTimes.Add(behaviorTime, new List<Behavior>());
      }
    }

    public async UniTask RunRoot(TickTime behaviorTime, Unit unit = null, Context context = null) {
      // 优先更新Buff回合数
      await Battle.BuffManager.Update(behaviorTime, unit);

      var behaviorList = TempList<Behavior>.Get();
      behaviorList.AddRange(BehaviorTimes[behaviorTime]);
      foreach (var behavior in behaviorList) {
        if (unit == null || behavior.Unit == null || unit == behavior.Unit) {
          await behavior.Run<Root>(context);
        }
      }
      TempList<Behavior>.Release(behaviorList);
    }

    public async UniTask<Behavior> Add(string behaviorId, Unit source = null, Unit target = null) {
      if (!Templates.TryGetValue(behaviorId, out var behaviorGraph)) {
        Debug.LogError($"BehaviorManager.Add error, behaviorGraph is not preload. Id:{behaviorId}");
        return null;
      }

      if(!BehaviorTimes.TryGetValue(behaviorGraph.BehaviorTime, out var behaviorList)) {
        Debug.LogError($"BehaviorManager.Add error, BehaviorTime:[{behaviorGraph.BehaviorTime}] is undefined. Id:{behaviorId} ");
        return null;
      }

      Behavior behavior = Battle.ObjectPool.Get<Behavior>();
      behavior.Init(Battle, ++IncId, behaviorGraph, source, target);
      Behaviors.Add(behavior.RuntimeId, behavior);
      behaviorList.Add(behavior);

      await behavior.Run<Init>();

      if (!Behaviors.ContainsKey(behavior.RuntimeId)) {
        return null;
      }

      // 按优先级从大到小排序
      behaviorList.Sort(CompareBehavior);

      return behavior;
    }

    private int CompareBehavior(Behavior left, Behavior right) {
      int leftPriority = left.BehaviorGraph.Priority;
      int rightPriority = right.BehaviorGraph.Priority;
      if (leftPriority != rightPriority) {
        return rightPriority.CompareTo(leftPriority);
      }

      return left.RuntimeId.CompareTo(right.RuntimeId);
    }

    public async UniTask<bool> Remove(int runtimeId) {
      if (!Behaviors.TryGetValue(runtimeId, out var behavior)) {
        Debug.LogError($"BehaviorManager.Remove error, behavior is not exists. runtimeId:{runtimeId}");
        return false;
      }

      await behavior.Run<Finalize>();

      Behaviors.Remove(runtimeId);
      BehaviorTimes[behavior.BehaviorGraph.BehaviorTime].Remove(behavior);
      Battle.ObjectPool.Release(behavior);

      return true;
    }
  }
}