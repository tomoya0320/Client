using Battle.BehaviorFuncs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle.MagicFuncs {
  [CreateAssetMenu(menuName = "Ð§¹û/ÉËº¦")]
  public class Damage : MagicFuncBase {
    public override bool IgnoreOnEnd => true;
    [LabelText("¹¥»÷Á¦")]
    public NodeParam DamageValue;

    public override void Run(BattleManager battleManager, Context context, MagicArgs args) {
      float damageValue;
      if(context is BehaviorContext behaviorContext) {
        damageValue = behaviorContext.Behavior.GetFloat(DamageValue);
      } else {
        damageValue = DamageValue.Value;
      }
      battleManager.DamageManager.Damage(args.Source, args.Target, damageValue);
    }
  }
}