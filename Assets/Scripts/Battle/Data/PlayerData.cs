using Sirenix.OdinInspector;
using System;

namespace GameCore {
  [Serializable]
  public class PlayerData {
    public string PlayerId;
    [LabelText("角色首发位")]
    public int FirstIndex;
    [LabelText("角色数据列表")]
    public UnitData[] UnitData;
  }
}