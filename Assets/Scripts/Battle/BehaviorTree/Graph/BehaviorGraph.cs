using UnityEngine;
using XNode;
using Battle;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace BehaviorTree.Battle {
  public enum DictType {
    Behavior,
    Unit,
    Player,
    Battle,
  }

  [CreateAssetMenu(menuName = "行为树/战斗")]
  public class BehaviorGraph : NodeGraph {
    [ReadOnly]
    public string BehaviorId;
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