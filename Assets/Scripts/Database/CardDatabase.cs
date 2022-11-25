using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [CreateAssetMenu(fileName = "CardDatabase", menuName = "创建卡牌数据库")]
  public class CardDatabase : ScriptableObject {
    [Serializable]
    public class CardDatabaseItem {
      public AssetReferenceT<CardTemplate> Template;
      [LabelText("编号")]
      public int No;
      [LabelText("是否已解锁")]
      public bool Unlocked;
    }

    [LabelText("卡牌数据列表")]
    public CardDatabaseItem[] CardDatabaseItems;
  }
}