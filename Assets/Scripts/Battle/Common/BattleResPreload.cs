using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;

namespace GameCore {
  public static class BattleResPreload {
    public static async UniTask<T> Preload<T>(AssetReferenceT<T> assetRef) where T : Object {
      if (string.IsNullOrEmpty(assetRef?.AssetGUID)) {
        return null;
      }

      var template = await ResourceManager.LoadAssetAsync(assetRef);

      return template ? template : null;
    }
  }
}