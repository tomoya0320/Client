using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [CreateAssetMenu(fileName = "MapNodeDatabase", menuName = "���ݿ�/������ͼ�ڵ����ݿ�")]
  public class MapNodeDatabase : SerializedScriptableObject {
    [Serializable]
    [DrawWithUnity]
    public class MapNodeDatabaseItem {
      public AssetReferenceT<MapNodeBase> MapNode;
    }

    [LabelText("��ͼ�ڵ����ݿ�"), DictionaryDrawerSettings(KeyLabel = "���", ValueLabel = "����")]
    public Dictionary<int, MapNodeDatabaseItem> MapNodeDatabaseItems;

    public AssetReferenceT<MapNodeBase> GetMapNode(int no) {
      MapNodeDatabaseItems.TryGetValue(no, out var item);
      return item?.MapNode;
    }
  }
}