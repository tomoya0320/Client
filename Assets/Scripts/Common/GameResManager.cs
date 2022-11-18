using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  public static class GameResManager {
    public static T LoadAsset<T>(AssetReferenceT<T> assetRef) where T : Object {
      if (string.IsNullOrEmpty(assetRef?.AssetGUID)) {
        return null;
      }

      var op = assetRef.LoadAssetAsync();
      T asset = op.WaitForCompletion();
      Addressables.Release(op);
      return asset;
    }

    public static T LoadAsset<T>(string id) where T : class {
      if (string.IsNullOrEmpty(id)) {
        return null;
      }

      var op = Addressables.LoadAssetAsync<T>(id);
      T asset = op.WaitForCompletion();
      Addressables.Release(op);
      return asset;
    }
  }
}