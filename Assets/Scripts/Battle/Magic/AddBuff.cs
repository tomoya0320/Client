using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameCore.MagicFuncs {
  [CreateAssetMenu(menuName = "模板/效果/加Buff")]
  public class AddBuff : MagicFuncBase {
    public override bool IgnoreOnEnd => true;
    public string BuffId;

    public async override UniTask Run(Battle battle, Context context, MagicArgs args) {
      await battle.BuffManager.AddBuff(BuffId, args.Source, args.Target);
    }
  }
}