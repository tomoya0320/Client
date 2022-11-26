using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [CreateAssetMenu(fileName = "LevelDatabase", menuName = "�����ؿ����ݿ�")]
  public class LevelDatabase : SerializedScriptableObject {
    [Serializable]
    [DrawWithUnity]
    public class LevelDatabaseItem {
      public AssetReferenceT<LevelTemplate> Template;
      [LabelText("�Ƿ��ѽ���")]
      public bool Unlocked;
    }

    [LabelText("�ؿ����ݿ�"), DictionaryDrawerSettings(KeyLabel = "���", ValueLabel = "����")]
    public Dictionary<int, LevelDatabaseItem> LevelDatabaseItems;
  }
}