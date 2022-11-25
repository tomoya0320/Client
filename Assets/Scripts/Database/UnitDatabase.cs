using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [CreateAssetMenu(fileName = "UnitDatabase", menuName = "������λ���ݿ�")]
  public class UnitDatabase : ScriptableObject {
    [Serializable]
    public class UnitDatabaseItem {
      public AssetReferenceT<UnitTemplate> Template;
      [LabelText("���")]
      public int No;
      [LabelText("�Ƿ��ѽ���")]
      public bool Unlocked;
    }

    [LabelText("��λ�����б�")]
    public UnitDatabaseItem[] UnitDatabaseItems;
  }
}