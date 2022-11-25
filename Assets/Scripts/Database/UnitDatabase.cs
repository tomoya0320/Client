using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [CreateAssetMenu(fileName = "UnitDatabase", menuName = "创建单位数据库")]
  public class UnitDatabase : ScriptableObject {
    [Serializable]
    public class UnitDatabaseItem {
      public AssetReferenceT<UnitTemplate> Template;
      [LabelText("编号")]
      public int No;
      [LabelText("是否已解锁")]
      public bool Unlocked;
    }

    [LabelText("单位数据列表")]
    public UnitDatabaseItem[] UnitDatabaseItems;
  }
}