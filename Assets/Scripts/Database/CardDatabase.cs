using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [CreateAssetMenu(fileName = "CardDatabase", menuName = "�����������ݿ�")]
  public class CardDatabase : ScriptableObject {
    [Serializable]
    public class CardDatabaseItem {
      public AssetReferenceT<CardTemplate> Template;
      [LabelText("���")]
      public int No;
      [LabelText("�Ƿ��ѽ���")]
      public bool Unlocked;
    }

    [LabelText("���������б�")]
    public CardDatabaseItem[] CardDatabaseItems;
  }
}