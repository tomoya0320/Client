using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [CreateAssetMenu(fileName = "MapNodeDatabase", menuName = "数据库/创建地图节点数据库")]
  public class MapNodeDatabase : ScriptableObject {
    [Serializable]
    [DrawWithUnity]
    public class MapNodeDatabaseItem {
      public AssetReferenceT<MapNodeBase> MapNode;
    }

    [LabelText("地图节点数据库")]
    [ListDrawerSettings(ShowIndexLabels = true)]
    public List<MapNodeDatabaseItem> MapNodeDatabaseItems;

    public AssetReferenceT<MapNodeBase> GetMapNode(int index) {
      if (index < 0 || index >= MapNodeDatabaseItems.Count) {
        return null;
      }
      return MapNodeDatabaseItems[index]?.MapNode;
    }
  }
}