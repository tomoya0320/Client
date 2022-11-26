using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [CreateAssetMenu(fileName = "MapNodeDatabase", menuName = "数据库/创建地图节点数据库")]
  public class MapNodeDatabase : SerializedScriptableObject {
    [Serializable]
    [DrawWithUnity]
    public class MapNodeDatabaseItem {
      public AssetReferenceT<MapNodeBase> MapNode;
    }

    [LabelText("地图节点数据库"), DictionaryDrawerSettings(KeyLabel = "编号", ValueLabel = "数据")]
    public Dictionary<int, MapNodeDatabaseItem> MapNodeDatabaseItems;

    public AssetReferenceT<MapNodeBase> GetMapNode(int no) {
      MapNodeDatabaseItems.TryGetValue(no, out var item);
      return item?.MapNode;
    }
  }
}