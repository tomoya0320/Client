using UnityEngine;

public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T> {
  private static T _Instance;

  public static T Instance {
    get {
      if (!_Instance) {
        GameObject obj = new GameObject(typeof(T).Name);
        _Instance = obj.AddComponent<T>();
      }
      return _Instance;
    }
  }
}