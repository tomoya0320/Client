using BehaviorTree.Battle;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Battle {
  [CreateAssetMenu(menuName = "效果/加行为树")]
  public class AddBehavior : MagicAction {
    public override bool IgnoreOnEnd => false;
    public AssetReferenceT<BehaviorGraph> BehaviorGraphRef;

    public override void Run(BattleManager battleManager, Context context, EffectArgs args) {
      if(args.IsEnd) {
        if(context is BuffContext buffContext && buffContext.BehaviorRuntimeId > 0) {
          battleManager.BehaviorManager.RemoveBehavior(buffContext.BehaviorRuntimeId);
        }
      } else {
        BehaviorGraph behaviorGraph = BehaviorGraphRef.Asset as BehaviorGraph;
        int behaviorRuntimeId = battleManager.BehaviorManager.AddBehavior(behaviorGraph, args.Source, args.Target);
        if (behaviorRuntimeId > 0 && context is BuffContext buffContext) {
          buffContext.BehaviorRuntimeId = behaviorRuntimeId;
        }
      }
    }
  }
}