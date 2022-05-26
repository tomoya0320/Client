using Sirenix.OdinInspector;
using System;

namespace GameCore {
  [Serializable]
  public class BattleData {
    [LabelText("关卡Id")]
    public string LevelId;
    [LabelText("玩家数据")]
    public PlayerData PlayerData;
  }
}