using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore {
  [Serializable]
  public class User {
    public List<(int no, int lv)> Cards = new List<(int, int)>();
    public List<(int no, int lv)> Units = new List<(int, int)>();

    public User() {
      // TEST
      Cards.Add((0, 0));
      Cards.Add((0, 0));
      Cards.Add((0, 0));
      Cards.Add((0, 0));
      Cards.Add((1, 0));
      Cards.Add((2, 0));
      Units.Add((0, 0));
    }

    public static User LoadFromLocal() {
      if (PlayerPrefs.HasKey(nameof(User))) {
        return JsonUtility.FromJson<User>(PlayerPrefs.GetString(nameof(User)));
      }
      return null;
    }

    public void SaveData() => PlayerPrefs.SetString(nameof(User), JsonUtility.ToJson(this));
  }
}