using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/其他/伤害")]
  public class Damage : ActionNode {
    [LabelText("攻击力")]
    public NodeIntParam DamageValue;
    [LabelText("攻击单位")]
    public NodeParamKey AttackUnit;
    [LabelText("目标单位")]
    public NodeParamKey TargetUnit;

    public override async UniTask<NodeResult> Run(Behavior behavior, Context context) {
      Unit attackUnit = behavior.GetUnit(AttackUnit);
      Unit targetUnit = behavior.GetUnit(TargetUnit);
      if (attackUnit == null || targetUnit == null) {
        return NodeResult.False;
      }
      int damageValue = behavior.GetInt(DamageValue);
      await behavior.Battle.DamageManager.Damage(attackUnit, targetUnit, damageValue);
      return NodeResult.True;
    }
  }
}