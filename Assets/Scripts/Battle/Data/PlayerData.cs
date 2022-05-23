using System;

namespace Battle {
  [Serializable]
  public struct PlayerData {
    public string PlayerId;
    public UnitData[] UnitData;
  }
}