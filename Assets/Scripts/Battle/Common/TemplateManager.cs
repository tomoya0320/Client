using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  public abstract class TemplateManager<T> : BattleBase where T : ScriptableObject {
    protected Dictionary<string, T> Templates = new Dictionary<string, T>();

    protected TemplateManager(Battle battle) : base(battle) { }

    public T Preload(AssetReferenceT<T> assetRef) {
      if (string.IsNullOrEmpty(assetRef?.AssetGUID)) {
        return null;
      }

      if (Templates.TryGetValue(assetRef.AssetGUID, out var template)) {
        return template;
      }
      template = GameResManager.LoadAsset(assetRef);
      if (template) {
        Templates.Add(assetRef.AssetGUID, template);
        return template;
      }
      return null;
    }

    public bool TryGetTemplate(string id, out T template) {
      return Templates.TryGetValue(id, out template);
    }

    public void Release() {
      Templates.Clear();
    }
  }
}