using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Battle {
  public class BehaviorManager : BattleBase {
    private int IncId;
    private List<int> TempRuntimeIdList = new List<int>();
    private Dictionary<string, BehaviorTemplate> BehaviorTemplates = new Dictionary<string, BehaviorTemplate>();
    private Dictionary<int, Behavior> Behaviors = new Dictionary<int, Behavior>();
    private Dictionary<BehaviorTime, List<int>> BehaviorTimes = new Dictionary<BehaviorTime, List<int>>();

    public BehaviorManager(BattleManager battleManager) : base(battleManager) {
      foreach (BehaviorTime behaviorTime in Enum.GetValues(typeof(BehaviorTime))) {
        BehaviorTimes.Add(behaviorTime, new List<int>());
      }
    }

    public async UniTask PreloadBehavior(string behaviorId) {
      if (BehaviorTemplates.ContainsKey(behaviorId)) {
        return;
      }
      BehaviorTemplate behaviorTemplate = await Addressables.LoadAssetAsync<BehaviorTemplate>(behaviorId);
      BehaviorTemplates.Add(behaviorId, behaviorTemplate);
    }

    public async UniTask Run(BehaviorTime behaviorTime, Unit unit = null, Context context = null) {
      TempRuntimeIdList.AddRange(BehaviorTimes[behaviorTime]);
      foreach (int runtimeId in TempRuntimeIdList) {
        var behavior = Behaviors[runtimeId];
        if (unit == null || behavior.Unit == null || unit == behavior.Unit) {
          await behavior.Run(context);
        }
      }
      TempRuntimeIdList.Clear();
    }

    public Behavior AddBehavior(string behaviorId, Unit source = null, Unit target = null) {
      if (!BehaviorTemplates.TryGetValue(behaviorId, out var behaviorGraph)) {
        Debug.LogError($"BehaviorManager.AddBehavior error, behaviorTemplate is not preload. Id:{behaviorId}");
        return null;
      }

      int runtimeId = ++IncId;
      Behavior behavior = BattleManager.ObjectPool.Get<Behavior>();
      behavior.Init(BattleManager, runtimeId, behaviorGraph, source, target);
      Behaviors.Add(runtimeId, behavior);
      BehaviorTimes[behaviorGraph.BehaviorTime].Add(runtimeId);
      return behavior;
    }

    public bool RemoveBehavior(int runtimeId) {
      if (!Behaviors.TryGetValue(runtimeId, out var behavior)) {
        Debug.LogError($"BehaviorManager.RemoveBehavior error, behavior is not exists. runtimeId:{runtimeId}");
        return false;
      }
      Behaviors.Remove(runtimeId);
      BehaviorTimes[behavior.BehaviorTemplate.BehaviorTime].Remove(runtimeId);
      BattleManager.ObjectPool.Release(behavior);
      return true;
    }

    public void CleanUp() {
      foreach (var behaviorGraph in BehaviorTemplates.Values) {
        Addressables.Release(behaviorGraph);
      }
    }
  }
}