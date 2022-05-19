using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace Battle.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/伤害")]
  public class Damage : ActionNode {
    [LabelText("攻击力")]
    public NodeParam DamageValue;
    [LabelText("目标单位")]
    public NodeParamKey TargetUnit;

    public override async UniTask<bool> Run(Behavior behavior, Context context) {
      // Test
      await UniTask.Delay(1000);
      Unit targetUnit = behavior.GetUnit(TargetUnit);
      if (targetUnit == null) {
        return false;
      }
      float damageValue = behavior.GetFloat(DamageValue);
      return behavior.BattleManager.DamageManager.Damage(behavior.Unit, targetUnit, damageValue);
    }
  }
}