using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  public static class ResourceManager {
    public static async UniTask<T> LoadAssetAsync<T>(AssetReferenceT<T> assetRef) where T : Object {
      if (assetRef.Asset) {
        return assetRef.Asset as T;
      }

      var handle = assetRef.LoadAssetAsync();
      while (!handle.IsDone) {
        await UniTask.Yield(Game.Instance.CancellationToken);
      }
      return handle.Result;
    }

    public static async UniTask<T> LoadAssetAsync<T>(string name) where T : Object {
      var handle = Addressables.LoadAssetAsync<T>(name);
      while (!handle.IsDone) {
        await UniTask.Yield(Game.Instance.CancellationToken);
      }
      return handle.Result;
    }
  }
}