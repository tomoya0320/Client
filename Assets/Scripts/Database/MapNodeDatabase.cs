using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [CreateAssetMenu(fileName = "MapNodeDatabase", menuName = "���ݿ�/������ͼ�ڵ����ݿ�")]
  public class MapNodeDatabase : ScriptableObject {
    [Serializable]
    [DrawWithUnity]
    public class MapNodeDatabaseItem {
      public AssetReferenceT<MapNodeBase> MapNode;
    }

    [LabelText("��ͼ�ڵ����ݿ�")]
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