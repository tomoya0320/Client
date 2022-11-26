using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [CreateAssetMenu(fileName = "CardDatabase", menuName = "创建卡牌数据库")]
  public class CardDatabase : SerializedScriptableObject {
    [Serializable]
    [DrawWithUnity]
    public class CardDatabaseItem {
      public AssetReferenceT<CardTemplate> Template;
      [LabelText("是否已解锁")]
      public bool Unlocked;
    }

    [LabelText("卡牌数据库"), DictionaryDrawerSettings(KeyLabel = "编号", ValueLabel = "数据")]
    public Dictionary<int, CardDatabaseItem> CardDatabaseItems;
  }
}