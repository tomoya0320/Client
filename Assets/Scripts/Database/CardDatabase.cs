using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [CreateAssetMenu(fileName = "CardDatabase", menuName = "���ݿ�/�����������ݿ�")]
  public class CardDatabase : ScriptableObject {
    [Serializable]
    public class CardDatabaseItem {
      public AssetReferenceT<CardTemplate> Template;
    }

    [LabelText("�������ݿ�")]
    [ListDrawerSettings(ShowIndexLabels = true)]
    public List<CardDatabaseItem> CardDatabaseItems;

    public AssetReferenceT<CardTemplate> GetCardTemplate(int index) {
      if (index < 0 || index >= CardDatabaseItems.Count) {
        return null;
      }
      return CardDatabaseItems[index]?.Template;
    }
  }
}