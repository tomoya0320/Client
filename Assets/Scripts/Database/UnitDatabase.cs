using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [CreateAssetMenu(fileName = "UnitDatabase", menuName = "创建单位数据库")]
  public class UnitDatabase : SerializedScriptableObject {
    [Serializable]
    [DrawWithUnity]
    public class UnitDatabaseItem {
      public AssetReferenceT<UnitTemplate> Template;
      [LabelText("是否已解锁")]
      public bool Unlocked;
    }

    [LabelText("单位数据库"), DictionaryDrawerSettings(KeyLabel = "编号", ValueLabel = "数据")]
    public Dictionary<int, UnitDatabaseItem> UnitDatabaseItems;
  }
}