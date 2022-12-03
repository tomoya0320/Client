using UnityEngine;

namespace GameCore {
  public class AttribManager : BattleBase {
    public AttribManager(Battle battleManager) : base(battleManager) { }

    public Attrib[] GetAttribs(AttribTemplate attribTemplate, int level, int maxLevel) {
      if (!attribTemplate) {
        Debug.LogError($"AttribTemplate is null!");
        return null;
      }

      return attribTemplate.GetAttribs(level, maxLevel);
    }
  }
}