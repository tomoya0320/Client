using GameCore.MagicFuncs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameCore {
  public class MagicManager : BattleBase {
    public MagicManager(Battle battle) : base(battle) { }

    public async UniTask DoMagic(MagicFuncBase magicFunc, Unit source, Unit target, Context context = null, bool isEnd = false) {
      if (!magicFunc) {
        return;
      }

      if (target == null) {
        Debug.LogError("MagicManager.DoMagic error, target is null");
        return;
      }

      if (!magicFunc.IgnoreOnEnd || !isEnd) {
        MagicArgs args = new MagicArgs {
          IsEnd = isEnd,
          Source = source,
          Target = target,
        };
        await magicFunc.Run(Battle, context, args);
      }
    }
  }
}