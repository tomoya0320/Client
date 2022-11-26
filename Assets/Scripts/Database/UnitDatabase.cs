using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [CreateAssetMenu(fileName = "UnitDatabase", menuName = "������λ���ݿ�")]
  public class UnitDatabase : SerializedScriptableObject {
    [Serializable]
    [DrawWithUnity]
    public class UnitDatabaseItem {
      public AssetReferenceT<UnitTemplate> Template;
      [LabelText("�Ƿ��ѽ���")]
      public bool Unlocked;
    }

    [LabelText("��λ���ݿ�"), DictionaryDrawerSettings(KeyLabel = "���", ValueLabel = "����")]
    public Dictionary<int, UnitDatabaseItem> UnitDatabaseItems;
  }
}