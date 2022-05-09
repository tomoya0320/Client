using UnityEngine;

public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T> {
  private static T s_instance;

  public static T Instance {
    get {
      if (s_instance == null) {
        GameObject obj = new GameObject(typeof(T).Name);
        s_instance = obj.AddComponent<T>();
      }
      return s_instance;
    }
  }
  public static bool hasInstance => s_instance != null;

  private void Awake() {
    if (!hasInstance) {
      s_instance = this as T;
      OnInit();
    } else {
      Debug.LogWarning("Duplicate Instance:" + nameof(T));
      Destroy(gameObject);
    }
  }

  protected virtual void OnInit() { }
}