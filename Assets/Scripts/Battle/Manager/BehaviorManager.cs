using BehaviorTree.Battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
  public enum BehaviorTime {
    ON_BEFORE_TURN,
    ON_LATE_TURN,
  }

  public class BehaviorManager : BattleBase {
    private int IncId;
    private Dictionary<int, BehaviorGraph> Behaviors = new Dictionary<int, BehaviorGraph>();
    private Dictionary<BehaviorTime, List<int>> BehaviorTimes = new Dictionary<BehaviorTime, List<int>>();

    public BehaviorManager(BattleManager battleManager) : base(battleManager) {
      foreach (BehaviorTime behaviorTime in Enum.GetValues(typeof(BehaviorTime))) {
        BehaviorTimes.Add(behaviorTime, new List<int>());
      }
    }

    public int AddBehavior(BehaviorGraph behaviorGraph, Unit source, Unit target) {
      if (!behaviorGraph) {
        Debug.LogError($"BehaviorManager.AddBehavior error, behaviorGraph is null.");
        return 0;
      }

      int runtimeId = ++IncId;
      BehaviorGraph runtimeBehavior = behaviorGraph.Copy() as BehaviorGraph;
      runtimeBehavior.Init(BattleManager, runtimeId, source, target);
      Behaviors.Add(runtimeId, runtimeBehavior);
      BehaviorTimes[runtimeBehavior.BehaviorTime].Add(runtimeId);
      return runtimeId;
    }

    public bool RemoveBehavior(int runtimeId) {
      if (!Behaviors.TryGetValue(runtimeId, out var behavior)) {
        Debug.LogError($"BehaviorManager.RemoveBehavior error, behavior is not exists. runtimeId:{runtimeId}");
        return false;
      }

      return Behaviors.Remove(runtimeId) && BehaviorTimes[behavior.BehaviorTime].Remove(runtimeId);
    }
  }
}