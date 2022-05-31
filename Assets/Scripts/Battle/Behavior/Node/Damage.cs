using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/伤害")]
  public class Damage : ActionNode {
    [LabelText("攻击力")]
    public NodeParam DamageValue;
    [LabelText("目标单位")]
    public NodeParamKey TargetUnit;

    public override async UniTask<bool> Run(Behavior behavior, Context context) {
      Unit targetUnit = behavior.GetUnit(TargetUnit);
      if (targetUnit == null) {
        return false;
      }
      float damageValue = behavior.GetFloat(DamageValue);
      await behavior.BattleManager.DamageManager.Damage(behavior.Unit, targetUnit, damageValue);
      return true;
    }
  }
}