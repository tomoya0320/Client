using Cysharp.Threading.Tasks;
using GameCore.BehaviorFuncs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.MagicFuncs {
  [CreateAssetMenu(menuName = "模板/效果/伤害")]
  public class Damage : MagicFuncBase {
    public override bool IgnoreOnEnd => true;
    [LabelText("攻击力")]
    public NodeIntParam DamageValue;

    public override async UniTask Run(Battle battleManager, Context context, MagicArgs args) {
      int damageValue;
      if(context is BehaviorContext behaviorContext) {
        damageValue = behaviorContext.Behavior.GetInt(DamageValue);
      } else {
        damageValue = DamageValue.Value;
      }
      await battleManager.DamageManager.Damage(args.Source, args.Target, damageValue);
    }
  }
}