using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [CreateAssetMenu(fileName = "UnitDatabase", menuName = "���ݿ�/������λ���ݿ�")]
  public class UnitDatabase : SerializedScriptableObject {
    [Serializable]
    [DrawWithUnity]
    public class UnitDatabaseItem {
      public AssetReferenceT<UnitTemplate> Template;
    }

    [LabelText("��λ���ݿ�"), DictionaryDrawerSettings(KeyLabel = "���", ValueLabel = "����")]
    public Dictionary<int, UnitDatabaseItem> UnitDatabaseItems;

    public AssetReferenceT<UnitTemplate> GetUnitTemplate(int no) {
      UnitDatabaseItems.TryGetValue(no, out var item);
      return item?.Template;
    }
  }
}