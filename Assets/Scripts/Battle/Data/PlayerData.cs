using Sirenix.OdinInspector;
using System;

namespace GameCore {
  [Serializable]
  public struct PlayerData {
    public string PlayerId;
    [LabelText("角色数据列表")]
    public UnitData[] UnitData;
  }
}