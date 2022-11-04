using UnityEngine.AddressableAssets;

namespace GameCore {
  public static class GameResManager {
    public static T LoadAssetAsync<T>(string id) {
      var op = Addressables.LoadAssetAsync<T>(id);
      T asset = op.WaitForCompletion();
      Addressables.Release(op);
      return asset;
    }
  }
}