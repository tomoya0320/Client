using Battle.BehaviorFuncs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle.MagicFuncs {
  [CreateAssetMenu(menuName = "ģ��/Ч��/�˺�")]
  public class Damage : MagicTemplate {
    public override bool IgnoreOnEnd => true;
    [LabelText("������")]
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