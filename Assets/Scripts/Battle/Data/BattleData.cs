using Sirenix.OdinInspector;
using System;

namespace GameCore {
  [Serializable]
  public struct BattleData {
    [LabelText("关卡Id")]
    public string LevelId;
    [LabelText("玩家数据")]
    public PlayerData PlayerData;
  }
}