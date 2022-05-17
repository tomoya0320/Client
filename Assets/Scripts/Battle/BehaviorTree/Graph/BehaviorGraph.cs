using UnityEngine;
using XNode;
using Battle;
using Cysharp.Threading.Tasks;

namespace BehaviorTree.Battle {
  public enum SourceType {
    None,
    Behavior,
    Unit,
    Player,
    Battle,
  }

  [CreateAssetMenu(menuName = "行为树/战斗")]
  public class BehaviorGraph : NodeGraph {
    [HideInInspector]
    public string BehaviorId;
    public Unit SourceUnit { get; private set; }
    public Unit TargetUnit { get; private set; }
    public BattleManager BattleManager { get; private set; }
    public Blackboard Blackboard { get; private set; }

    private Blackboard GetBlackboard(SourceType type) {
      Blackboard blackboard;
      switch (type) {
        case SourceType.Behavior:
          blackboard = Blackboard;
          break;
        case SourceType.Unit:
          blackboard = TargetUnit?.Blackboard;
          break;
        case SourceType.Player:
          blackboard = TargetUnit?.Player?.Blackboard;
          break;
        case SourceType.Battle:
          blackboard = BattleManager.Blackboard;
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

    private float GetBlackboardValue(SourceType type, string key) {
      Blackboard blackboard = GetBlackboard(type);
      if (blackboard != null) {
        if (blackboard.TryGetValue(key, out float f)) {
          return f;
        }
        Debug.LogError($"BehaviorGraph.GetBlackboardValue error, key is not exists! key:{key}");
      }
      return 0f;
    }

    public int GetInt(BehaviorNodeParam<int> nodeParam) {
      if(nodeParam.ParamKey.Type == SourceType.None || string.IsNullOrEmpty(nodeParam.ParamKey.Key)) {
        return nodeParam.Value;
      }
      return (int)GetBlackboardValue(nodeParam.ParamKey.Type, nodeParam.ParamKey.Key);
    }

    public void SetInt(BehaviorNodeParamKey nodeParamKey, int i) => SetFloat(nodeParamKey, i);

    public float GetFloat(BehaviorNodeParam<float> nodeParam) {
      if (nodeParam.ParamKey.Type == SourceType.None || string.IsNullOrEmpty(nodeParam.ParamKey.Key)) {
        return nodeParam.Value;
      }
      return GetBlackboardValue(nodeParam.ParamKey.Type, nodeParam.ParamKey.Key);
    }

    public void SetFloat(BehaviorNodeParamKey nodeParamKey, float f) {
      if (nodeParamKey.Type == SourceType.None || string.IsNullOrEmpty(nodeParamKey.Key)) {
        Debug.LogError($"BehaviorGraph.SetFloat error, nodeParam is illegal!");
        return;
      }
      Blackboard blackboard = GetBlackboard(nodeParamKey.Type);
      if (blackboard != null) {
        blackboard[nodeParamKey.Key] = f;
      }
    }

    public Unit GetUnit(BehaviorNodeParamKey nodeParam) {
      if (nodeParam.Type == SourceType.None || string.IsNullOrEmpty(nodeParam.Key)) {
        Debug.LogError($"BehaviorGraph.GetUnit error, nodeParam is illegal!");
        return null;
      }

      Blackboard blackboard = GetBlackboard(nodeParam.Type);

      if (blackboard == null) {
        return null;
      }

      if (blackboard.TryGetValue(nodeParam.Key, out float runtimeId)) {
        return BattleManager.UnitManager.GetUnit((int)runtimeId);
      }

      Debug.LogError($"BehaviorGraph.GetUnit error, key is not exists! key:{nodeParam.Key}");
      return null;
    }

    public void Init(BattleManager battleManager, Unit sourceUnit, Unit targetUnit) {
      BattleManager = battleManager;
      SourceUnit = sourceUnit;
      TargetUnit = targetUnit;
      Blackboard = BattleManager.ObjectPool.Get<Blackboard>();
    }

    public void Release() {
      BattleManager.ObjectPool.Release(Blackboard);
      Blackboard = null;
      BattleManager = null;
      SourceUnit = null;
      TargetUnit = null;
    }

    public async UniTask Run(Context context = null) {
      var node = nodes.Find(n => n is Root) as BehaviorNode;
      if (node) {
        await node.Run(BattleManager, context);
      }
    }
  }
}