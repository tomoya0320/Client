using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  public static class ResourceManager {
    public static async UniTask<T> LoadAssetAsync<T>(AssetReferenceT<T> assetRef) where T : Object {
      T asset;
      if (assetRef.Asset) {
        asset = assetRef.Asset as T;
      } else {
        var handle = assetRef.LoadAssetAsync();
        while (!handle.IsDone) {
          await UniTask.Yield(Game.Instance.CancellationToken);
        }
        asset = handle.Result;
      }
      return asset;
    }
  }
}