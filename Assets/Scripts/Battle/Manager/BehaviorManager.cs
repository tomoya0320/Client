using BehaviorTree.Battle;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
  public class BehaviorManager : BattleBase {
    private int IncId;
    private List<int> TempRuntimeIdList = new List<int>();
    private Dictionary<int, Behavior> Behaviors = new Dictionary<int, Behavior>();
    private Dictionary<BehaviorTime, List<int>> BehaviorTimes = new Dictionary<BehaviorTime, List<int>>();

    public BehaviorManager(BattleManager battleManager) : base(battleManager) {
      foreach (BehaviorTime behaviorTime in Enum.GetValues(typeof(BehaviorTime))) {
        BehaviorTimes.Add(behaviorTime, new List<int>());
      }
    }

    public async UniTask Run(BehaviorTime behaviorTime, Unit unit = null, Context context = null) {
      TempRuntimeIdList.AddRange(BehaviorTimes[behaviorTime]);
      foreach (int runtimeId in TempRuntimeIdList) {
        var behavior = Behaviors[runtimeId];
        if (unit == null || unit == behavior.TargetUnit) {
          await behavior.Run(context);
        }
      }
      TempRuntimeIdList.Clear();
    }

    public int AddBehavior(BehaviorGraph behaviorGraph, Unit source, Unit target) {
      if (!behaviorGraph) {
        Debug.LogError($"BehaviorManager.AddBehavior error, behaviorGraph is null.");
        return 0;
      }

      int runtimeId = ++IncId;
      Behavior behavior = BattleManager.ObjectPool.Get<Behavior>();
      behavior.Init(BattleManager, runtimeId, source, target, behaviorGraph);
      Behaviors.Add(runtimeId, behavior);
      BehaviorTimes[behaviorGraph.BehaviorTime].Add(runtimeId);
      return runtimeId;
    }

    public bool RemoveBehavior(int runtimeId) {
      if (!Behaviors.TryGetValue(runtimeId, out var behavior)) {
        Debug.LogError($"BehaviorManager.RemoveBehavior error, behavior is not exists. runtimeId:{runtimeId}");
        return false;
      }
      Behaviors.Remove(runtimeId);
      BehaviorTimes[behavior.BehaviorGraph.BehaviorTime].Remove(runtimeId);
      BattleManager.ObjectPool.Release(behavior);
      return true;
    }
  }
}