using BehaviorTree.Battle;
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

    public BehaviorManager(BattleManager battleManager) : base(battleManager) {

    }

    public int AddBehavior(BehaviorGraph behaviorGraph, Unit source, Unit target) {
      if (!behaviorGraph) {
        Debug.LogError($"BehaviorManager.AddBehavior error, behaviorGraph is null.");
        return 0;
      }

      BehaviorGraph runtimeBehavior = behaviorGraph.Copy() as BehaviorGraph;
      runtimeBehavior.Init(BattleManager, ++IncId, source, target);
      Behaviors.Add(runtimeBehavior.RuntimeId, runtimeBehavior);
      return runtimeBehavior.RuntimeId;
    }

    public bool RemoveBehavior(int runtimeId) {
      if (!Behaviors.Remove(runtimeId)) {
        Debug.LogError($"BehaviorManager.RemoveBehavior error, behavior is not exists. runtimeId:{runtimeId}");
        return false;
      }
      return true;
    }
  }
}