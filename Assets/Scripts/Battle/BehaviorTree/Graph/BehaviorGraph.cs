using UnityEngine;
using XNode;
using Battle;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace BehaviorTree.Battle {
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
    [InspectorName("回合开始时")]
    ON_BEFORE_TURN,
    [InspectorName("回合结束时")]
    ON_LATE_TURN,
  }

  [CreateAssetMenu(menuName = "行为树/战斗")]
  public class BehaviorGraph : NodeGraph {
    [ReadOnly]
    public string BehaviorId;
    public BehaviorTime BehaviorTime;
    public int RuntimeId { get; private set; }
    public Unit SourceUnit { get; private set; }
    public Unit TargetUnit { get; private set; }
    public BattleManager BattleManager { get; private set; }
    public Blackboard Blackboard { get; private set; }

    private Blackboard GetBlackboard(DictType type) {
      Blackboard blackboard;
      switch (type) {
        case DictType.Behavior:
          blackboard = Blackboard;
          break;
        case DictType.Unit:
          blackboard = TargetUnit?.Blackboard;
          break;
        case DictType.Player:
          blackboard = TargetUnit?.Player?.Blackboard;
          break;
        case DictType.Battle:
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

    public int GetInt(NodeParam nodeParam) => (int)GetFloat(nodeParam);


    public void SetInt(NodeParamKey nodeParamKey, int i) => SetFloat(nodeParamKey, i);

    public float GetFloat(NodeParam nodeParam) {
      return nodeParam.IsDict ? GetBlackboardValue(nodeParam.ParamKey.Type, nodeParam.ParamKey.Key) : nodeParam.Value;
    }

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
        return BattleManager.UnitManager.GetUnit((int)runtimeId);
      }

      Debug.LogError($"BehaviorGraph.GetUnit error, key is not exists! key:{nodeParam.Key}");
      return null;
    }

    public void Init(BattleManager battleManager, int runtimeId, Unit sourceUnit, Unit targetUnit) {
      BattleManager = battleManager;
      RuntimeId = runtimeId;
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