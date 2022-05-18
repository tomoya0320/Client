using Battle.BehaviorFuncs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle.MagicFuncs {
  [CreateAssetMenu(menuName = "Ч��/�˺�")]
  public class Damage : MagicFuncBase {
    public override bool IgnoreOnEnd => false;
    [LabelText("������")]
    public NodeParam DamageValue;

    public override void Run(BattleManager battleManager, Context context, MagicArgs args) {
      float damageValue;
      if(context is BehaviorContext behaviorContext) {
        damageValue = behaviorContext.Behavior.GetFloat(DamageValue);
      } else {
        damageValue = DamageValue.Value;
      }
      // Test
      Debug.Log($"���{damageValue}�˺�");
    }
  }
}