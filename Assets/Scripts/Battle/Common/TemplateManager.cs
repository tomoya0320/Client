using System.Collections.Generic;
using UnityEngine;

namespace GameCore {
  public abstract class TemplateManager<T> : BattleBase where T : ScriptableObject {
    protected Dictionary<string, T> Templates = new Dictionary<string, T>();

    protected TemplateManager(Battle battle) : base(battle) { }

    public T Preload(string id) {
      if (Templates.TryGetValue(id, out var template)) {
        return template;
      }
      template = GameResManager.LoadAssetAsync<T>(id);
      if (template) {
        Templates.Add(id, template);
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