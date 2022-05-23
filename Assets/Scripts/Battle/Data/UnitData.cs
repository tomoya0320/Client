using System;

namespace Battle {
  [Serializable]
  public struct UnitData {
    public string TemplateId;
    public int Level;
    public CardData[] CardData;
  }
}