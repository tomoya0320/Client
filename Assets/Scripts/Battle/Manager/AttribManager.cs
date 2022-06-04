using UnityEngine;

namespace GameCore {
  public class AttribManager : TemplateManager<AttribTemplate> {
    public AttribManager(Battle battleManager) : base(battleManager) { }

    public Attrib[] GetAttribs(string attribId, int level, int maxLevel) {
      if (!Templates.TryGetValue(attribId, out var attribTemplate)) {
        Debug.LogError($"AttribTemplate is null. id:{attribId}");
        return null;
      }

      return attribTemplate.GetAttribs(level, maxLevel);
    }
  }
}