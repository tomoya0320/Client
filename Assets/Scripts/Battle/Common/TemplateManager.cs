using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  public abstract class TemplateManager<T> : BattleBase where T : ScriptableObject {
    public Dictionary<string, T> Templates = new Dictionary<string, T>();

    protected TemplateManager(Battle battle) : base(battle) {

    }

    public async UniTask Preload(string id) {
      if (Templates.ContainsKey(id)) {
        return;
      }
      T template = await Addressables.LoadAssetAsync<T>(id);
      if (template) {
        Templates.Add(id, template);
      }
    }

    public void Release() {
      foreach (var template in Templates.Values) {
        Addressables.Release(template);
      }
    }
  }
}