using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore.MagicFuncs {
  [CreateAssetMenu(menuName = "模板/效果/加行为树")]
  public class AddBehavior : MagicFuncBase {
    public override bool IgnoreOnEnd => false;
    public AssetReferenceT<BehaviorGraph> Behavior;

    public override async UniTask Run(Battle battle, Context context, MagicArgs args) {
      if (args.IsEnd) {
        if (context is BuffContext buffContext && buffContext.Behavior != null) {
          if (await battle.BehaviorManager.Remove(buffContext.Behavior.RuntimeId)) {
            buffContext.Behavior = null;
          }
        }
      } else {
        var behavior = await battle.BehaviorManager.Add(Behavior?.Asset as BehaviorGraph, args.Source, args.Target);
        if (behavior != null && context is BuffContext buffContext) {
          buffContext.Behavior = behavior;
        }
      }
    }
  }
}