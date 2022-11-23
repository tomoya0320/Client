using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;

namespace GameCore {
  public abstract class AssetManager<T> : BattleBase where T : Object {
    protected Dictionary<string, T> Templates = new Dictionary<string, T>();

    protected AssetManager(Battle battle) : base(battle) { }

    public async UniTask<T> Preload(AssetReferenceT<T> assetRef) {
      if (string.IsNullOrEmpty(assetRef?.AssetGUID)) {
        return null;
      }

      if (Templates.TryGetValue(assetRef.AssetGUID, out var template)) {
        return template;
      }
      if (assetRef.Asset) {
        template = assetRef.Asset as T;
      } else {
        var handle = assetRef.LoadAssetAsync();
        while (!handle.IsDone) {
          await UniTask.Yield(Battle.CancellationToken);
        }
        template = handle.Result;
      }

      if (template) {
        Templates.Add(assetRef.AssetGUID, template);
        return template;
      }
      return null;
    }

    public bool TryGetAsset(string id, out T template) {
      if (string.IsNullOrEmpty(id)) {
        template = null;
        return false;
      }

      return Templates.TryGetValue(id, out template);
    }

    public void Release() {
      Templates.Clear();
    }
  }
}