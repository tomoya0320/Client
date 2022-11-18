using Sirenix.OdinInspector;
using System;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [Serializable]
  public class BattleData {
    [LabelText("关卡")]
    public AssetReferenceT<LevelTemplate> Level;
    [LabelText("玩家数据")]
    public PlayerData PlayerData;
  }
}