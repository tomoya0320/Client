using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore {
  public class ObjectContainer : SerializedMonoBehaviour {
    [SerializeField, DictionaryDrawerSettings(KeyLabel = "名称", ValueLabel = "对象引用")]
    private Dictionary<string, GameObject> ObjectDict;

    public GameObject GetGameObject(string objName) {
      if (string.IsNullOrEmpty(objName)) {
        return null;
      }
      ObjectDict.TryGetValue(objName, out var obj);
      return obj;
    }
    
    public Transform GetTransform(string objName) {
      return GetGameObject(objName)?.transform;
    }

    public T GetComponent<T>(string objName) where T : Component {
      return GetGameObject(objName)?.GetComponent<T>();
    }
  }
}