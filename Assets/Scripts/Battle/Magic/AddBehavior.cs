using BehaviorTree.Battle;
using UnityEngine;

namespace Battle {
  [CreateAssetMenu(menuName = "效果/加行为树")]
  public class AddBehavior : MagicAction {
    public override bool IgnoreOnEnd => false;
    public BehaviorGraph BehaviorGraph;

    public override void Run(BattleManager battleManager, Context context, MagicArgs args) {
      if(args.IsEnd) {
        if(context is BuffContext buffContext && buffContext.BehaviorRuntimeId > 0) {
          battleManager.BehaviorManager.RemoveBehavior(buffContext.BehaviorRuntimeId);
        }
      } else {
        int behaviorRuntimeId = battleManager.BehaviorManager.AddBehavior(BehaviorGraph, args.Source, args.Target);
        if (behaviorRuntimeId > 0 && context is BuffContext buffContext) {
          buffContext.BehaviorRuntimeId = behaviorRuntimeId;
        }
      }
    }
  }
}