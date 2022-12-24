using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/单位/通过阵营获取单位总数")]
  public class GetUnitCountByCamp : ActionNode {
    [LabelText("阵营")]
    public PlayerCamp PlayerCamp;
    [LabelText("存值")]
    public NodeParamKey TargetKey;

    public override UniTask<NodeResult> Run(Behavior behavior, Context context) {
      int unitCount = 0;
      foreach (var unit in behavior.Battle.UnitManager.AllUnits) {
        if (unit.PlayerCamp == PlayerCamp) {
          unitCount++;
        }
      }
      behavior.SetInt(TargetKey, unitCount);
      return UniTask.FromResult(NodeResult.True);
    }
  }
}