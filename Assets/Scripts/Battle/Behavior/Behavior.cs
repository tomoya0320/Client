using GameCore.BehaviorFuncs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameCore {
  public class Behavior : IPoolObject {
    public int RuntimeId { get; private set; }
    public Unit SourceUnit { get; private set; }
    public Unit Unit { get; private set; }
    public Battle Battle { get; private set; }
    public Blackboard Blackboard { get; private set; }
    public BehaviorGraph BehaviorGraph { get; private set; }

    public void Init(Battle battle, int runtimeId, BehaviorGraph behaviorGraph, Unit sourceUnit = null, Unit targetUnit = null) {
      Battle = battle;
      RuntimeId = runtimeId;
      SourceUnit = sourceUnit;
      Unit = targetUnit;
      BehaviorGraph = behaviorGraph;
      Blackboard = Battle.ObjectPool.Get<Blackboard>();
    }

    public async UniTask Run<T>(Context context = null) where T : SingleOutNode {
      await BehaviorGraph.Run<T>(this, context);
    }

    private Blackboard GetBlackboard(DictType type) {
      Blackboard blackboard;
      switch (type) {
        case DictType.Behavior:
          blackboard = Blackboard;
          break;
        case DictType.Unit:
          blackboard = Unit?.Blackboard;
          break;
        case DictType.Player:
          blackboard = Unit?.Player?.Blackboard;
          break;
        case DictType.Battle:
          blackboard = Battle.Blackboard;
          break;
        default:
          blackboard = null;
          break;
      }
      if (blackboard == null) {
        Debug.LogError($"BehaviorGraph.GetBlackboard error, type is not supported! type:{type}");
      }
      return blackboard;
    }

    private float GetBlackboardValue(DictType type, string key) {
      Blackboard blackboard = GetBlackboard(type);
      if (blackboard != null) {
        if (blackboard.TryGetValue(key, out float f)) {
          return f;
        }
        Debug.LogError($"BehaviorGraph.GetBlackboardValue error, key is not exists! key:{key}");
      }
      return 0f;
    }

    public int GetInt(NodeIntParam nodeParam) => nodeParam.IsDict ? (int)GetBlackboardValue(nodeParam.ParamKey.Type, nodeParam.ParamKey.Key) : nodeParam.Value;


    public void SetInt(NodeParamKey nodeParamKey, int i) => SetFloat(nodeParamKey, i);

    public float GetFloat(NodeFloatParam nodeParam) => nodeParam.IsDict ? GetBlackboardValue(nodeParam.ParamKey.Type, nodeParam.ParamKey.Key) : nodeParam.Value;

    public void SetFloat(NodeParamKey nodeParamKey, float f) {
      Blackboard blackboard = GetBlackboard(nodeParamKey.Type);
      if (blackboard != null) {
        blackboard[nodeParamKey.Key] = f;
      }
    }

    public Unit GetUnit(NodeParamKey nodeParam) {
      Blackboard blackboard = GetBlackboard(nodeParam.Type);

      if (blackboard == null) {
        return null;
      }

      if (blackboard.TryGetValue(nodeParam.Key, out float runtimeId)) {
        return Battle.UnitManager.GetUnit((int)runtimeId);
      }

      Debug.LogError($"BehaviorGraph.GetUnit error, key is not exists! key:{nodeParam.Key}");
      return null;
    }

    public void Release() {
      Battle.ObjectPool.Release(Blackboard);
      RuntimeId = 0;
      Blackboard = null;
      Battle = null;
      SourceUnit = null;
      Unit = null;
      BehaviorGraph = null;
    }
  }
}