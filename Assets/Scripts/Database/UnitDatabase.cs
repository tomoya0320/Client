using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [CreateAssetMenu(fileName = "UnitDatabase", menuName = "���ݿ�/������λ���ݿ�")]
  public class UnitDatabase : ScriptableObject {
    [Serializable]
    [DrawWithUnity]
    public class UnitDatabaseItem {
      public AssetReferenceT<UnitTemplate> Template;
    }

    [LabelText("��λ���ݿ�")]
    [ListDrawerSettings(ShowIndexLabels = true)]
    public List<UnitDatabaseItem> UnitDatabaseItems;

    public AssetReferenceT<UnitTemplate> GetUnitTemplate(int index) {
      if (index < 0 || index >= UnitDatabaseItems.Count) {
        return null;
      }
      return UnitDatabaseItems[index]?.Template;
    }
  }
}