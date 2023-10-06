using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore.MagicFuncs {
  [CreateAssetMenu(menuName = "模板/效果/加Buff")]
  public class AddBuff : MagicFuncBase {
    public override bool IgnoreOnEnd => true;
    public AssetReferenceT<BuffTemplate> Buff;

    public override async UniTask Run(Battle battle, Context context, MagicArgs args) {
      await battle.BuffManager.AddBuff(Buff?.Asset as BuffTemplate, args.Source, args.Target);
    }
  }
}