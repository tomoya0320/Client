using Cysharp.Threading.Tasks;
using GameCore.BehaviorFuncs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore {
  public class BehaviorManager : TemplateManager<BehaviorGraph> {
    private int IncId;
    private Dictionary<int, Behavior> Behaviors = new Dictionary<int, Behavior>();
    private Dictionary<BehaviorTime, List<int>> BehaviorTimes = new Dictionary<BehaviorTime, List<int>>();

    public BehaviorManager(Battle battle) : base(battle) {
      foreach (BehaviorTime behaviorTime in Enum.GetValues(typeof(BehaviorTime))) {
        BehaviorTimes.Add(behaviorTime, new List<int>());
      }
    }

    public async UniTask RunRoot(BehaviorTime behaviorTime, Unit[] units, Context context = null) {
      foreach (var unit in units) {
        await RunRoot(behaviorTime, unit, context);
      }
    }

    public async UniTask RunRoot(BehaviorTime behaviorTime, Unit unit = null, Context context = null) {
      var list = TempList<int>.Get();
      list.AddRange(BehaviorTimes[behaviorTime]);
      foreach (int runtimeId in list) {
        var behavior = Behaviors[runtimeId];
        if (unit == null || behavior.Unit == null || unit == behavior.Unit) {
          await behavior.Run<Root>(context);
        }
      }
      TempList<int>.CleanUp();
    }

    public async UniTask<Behavior> Add(string behaviorId, Unit source = null, Unit target = null) {
      if (!Templates.TryGetValue(behaviorId, out var behaviorGraph)) {
        Debug.LogError($"BehaviorManager.Add error, behaviorGraph is not preload. Id:{behaviorId}");
        return null;
      }

      Behavior behavior = Battle.ObjectPool.Get<Behavior>();
      behavior.Init(Battle, ++IncId, behaviorGraph, source, target);
      Behaviors.Add(behavior.RuntimeId, behavior);
      BehaviorTimes[behaviorGraph.BehaviorTime].Add(behavior.RuntimeId);

      await behavior.Run<Init>();

      // 初始化节点里可能会移除行为树
      if (!Behaviors.ContainsKey(behavior.RuntimeId)) {
        return null;
      }

      return behavior;
    }

    public async UniTask<bool> Remove(int runtimeId) {
      if (!Behaviors.TryGetValue(runtimeId, out var behavior)) {
        Debug.LogError($"BehaviorManager.Remove error, behavior is not exists. runtimeId:{runtimeId}");
        return false;
      }

      await behavior.Run<Finalize>();

      Behaviors.Remove(runtimeId);
      BehaviorTimes[behavior.BehaviorGraph.BehaviorTime].Remove(runtimeId);
      Battle.ObjectPool.Release(behavior);

      return true;
    }
  }
}