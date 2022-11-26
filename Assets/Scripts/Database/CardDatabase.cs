using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [CreateAssetMenu(fileName = "CardDatabase", menuName = "�����������ݿ�")]
  public class CardDatabase : SerializedScriptableObject {
    [Serializable]
    [DrawWithUnity]
    public class CardDatabaseItem {
      public AssetReferenceT<CardTemplate> Template;
      [LabelText("�Ƿ��ѽ���")]
      public bool Unlocked;
    }

    [LabelText("�������ݿ�"), DictionaryDrawerSettings(KeyLabel = "���", ValueLabel = "����")]
    public Dictionary<int, CardDatabaseItem> CardDatabaseItems;
  }
}