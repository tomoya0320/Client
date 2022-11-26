using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [CreateAssetMenu(fileName = "CardDatabase", menuName = "���ݿ�/�����������ݿ�")]
  public class CardDatabase : SerializedScriptableObject {
    [Serializable]
    [DrawWithUnity]
    public class CardDatabaseItem {
      public AssetReferenceT<CardTemplate> Template;
    }

    [LabelText("�������ݿ�"), DictionaryDrawerSettings(KeyLabel = "���", ValueLabel = "����")]
    public Dictionary<int, CardDatabaseItem> CardDatabaseItems;

    public AssetReferenceT<CardTemplate> GetCardTemplate(int no) {
      CardDatabaseItems.TryGetValue(no, out var item);
      return item?.Template;
    }
  }
}