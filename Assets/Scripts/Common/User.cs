using System;
using UnityEngine;

namespace GameCore {
  [Serializable]
  public class User {
    public static User LoadFromLocal() {
      if (PlayerPrefs.HasKey(nameof(User))) {
        return JsonUtility.FromJson<User>(PlayerPrefs.GetString(nameof(User)));
      }
      return null;
    }
  }
}