using UnityEngine;

namespace Battle.MagicFuncs {
  [CreateAssetMenu(menuName = "ģ��/Ч��/����Ϊ��")]
  public class AddBehavior : MagicTemplate {
    public override bool IgnoreOnEnd => false;
    public string BehaviorId;

    public override void Run(BattleManager battleManager, Context context, MagicArgs args) {
      if (args.IsEnd) {
        if (context is BuffContext buffContext && buffContext.Behavior != null) {
          if (battleManager.BehaviorManager.RemoveBehavior(buffContext.Behavior.RuntimeId)) {
            buffContext.Behavior = null;
          }
        }
      } else {
        var behavior = battleManager.BehaviorManager.AddBehavior(BehaviorId, args.Source, args.Target);
        if (behavior != null && context is BuffContext buffContext) {
          buffContext.Behavior = behavior;
        }
      }
    }
  }
}