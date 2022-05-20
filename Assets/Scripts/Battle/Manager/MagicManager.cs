using Battle.MagicFuncs;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Battle {
  public class MagicManager : BattleBase {
    private Dictionary<string, MagicFuncBase> MagicFuncs = new Dictionary<string, MagicFuncBase>();

    public MagicManager(BattleManager battleManager) : base(battleManager) { 
    
    }

    public async UniTask PreloadMagic(string magicId) {
      if (MagicFuncs.ContainsKey(magicId)) {
        return;
      }
      MagicFuncBase magicFunc = await Addressables.LoadAssetAsync<MagicFuncBase>(magicId);
      MagicFuncs.Add(magicId, magicFunc);
    }

    public MagicFuncBase GetMagicFunc(string magicId) {
      MagicFuncs.TryGetValue(magicId, out MagicFuncBase magicFunc);
      return magicFunc;
    }

    public bool DoMagic(string magicId, Unit source, Unit target, Context context = null, bool isEnd = false) {
      if(target == null) {
        Debug.LogError("MagicManager.DoMagic error, target is null");
        return false;
      }
      
      if(!MagicFuncs.TryGetValue(magicId, out var magicAction)) {
        Debug.LogError($"MagicManager.DoMagic error, magic is not preload. Id:{magicId}");
        return false;
      }
      MagicArgs args = new MagicArgs {
        IsEnd = isEnd,
        Source = source,
        Target = target,
      };
      magicAction.Run(BattleManager, context, args);
      return true;
    }

    public void CleanUp() {
      foreach (var magicFunc in MagicFuncs.Values) {
        Addressables.Release(magicFunc);
      }
    }
  }
}