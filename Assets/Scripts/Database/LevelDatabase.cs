using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [CreateAssetMenu(fileName = "LevelDatabase", menuName = "创建关卡数据库")]
  public class LevelDatabase : SerializedScriptableObject {
    [Serializable]
    [DrawWithUnity]
    public class LevelDatabaseItem {
      public AssetReferenceT<LevelTemplate> Template;
      [LabelText("是否已解锁")]
      public bool Unlocked;
    }

    [LabelText("关卡数据库"), DictionaryDrawerSettings(KeyLabel = "编号", ValueLabel = "数据")]
    public Dictionary<int, LevelDatabaseItem> LevelDatabaseItems;
  }
}