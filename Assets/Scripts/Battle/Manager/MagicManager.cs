using GameCore.MagicFuncs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameCore {
  public class MagicManager : TemplateManager<MagicFuncBase> {
    public MagicManager(Battle battle) : base(battle) { }

    public async UniTask DoMagic(string magicId, Unit source, Unit target, Context context = null, bool isEnd = false) {
      if(target == null) {
        Debug.LogError("MagicManager.DoMagic error, target is null");
        return;
      }
      
      if(!Templates.TryGetValue(magicId, out var magicFunc)) {
        Debug.LogError($"MagicManager.DoMagic error, magicFunc is not preload. Id:{magicId}");
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