using Battle.MagicFuncs;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Battle {
  public class MagicManager : BattleBase {
    private Dictionary<string, MagicTemplate> MagicTemplates = new Dictionary<string, MagicTemplate>();

    public MagicManager(BattleManager battleManager) : base(battleManager) { 
    
    }

    public async UniTask PreloadMagic(string magicId) {
      if (MagicTemplates.ContainsKey(magicId)) {
        return;
      }
      MagicTemplate magicTemplate = await Addressables.LoadAssetAsync<MagicTemplate>(magicId);
      MagicTemplates.Add(magicId, magicTemplate);
    }

    public bool TryGetMagicTemplate(string magicId, out MagicTemplate magicTemplate) {
      return MagicTemplates.TryGetValue(magicId, out magicTemplate);
    }

    public bool DoMagic(string magicId, Unit source, Unit target, Context context = null, bool isEnd = false) {
      if(target == null) {
        Debug.LogError("MagicManager.DoMagic error, target is null");
        return false;
      }
      
      if(!MagicTemplates.TryGetValue(magicId, out var magicTemplate)) {
        Debug.LogError($"MagicManager.DoMagic error, magicTemplate is not preload. Id:{magicId}");
        return false;
      }
      MagicArgs args = new MagicArgs {
        IsEnd = isEnd,
        Source = source,
        Target = target,
      };
      magicTemplate.Run(BattleManager, context, args);
      return true;
    }

    public void CleanUp() {
      foreach (var magicTemplate in MagicTemplates.Values) {
        Addressables.Release(magicTemplate);
      }
    }
  }
}