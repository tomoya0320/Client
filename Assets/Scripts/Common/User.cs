using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace GameCore {
  [Serializable]
  public class User {
    public List<(int index, int lv)> Cards = new List<(int, int)>();
    public List<(int index, int lv)> Units = new List<(int, int)>();
    public Map Map;

    // TODO:优化 TEST
    public PlayerData GetPlayerData() => new PlayerData {
      PlayerId = "测试用户",
      FirstIndex = 0,
      PlayerCamp = PlayerCamp.ALLY,
      UnitData = GetUnitData(),
    };
    // TODO:优化 TEST
    private UnitData[] GetUnitData() {
      UnitData[] unitData = new UnitData[Units.Count];
      for (int i = 0; i < unitData.Length; i++) {
        unitData[i] = new UnitData {
          Lv = Units[i].lv,
          Template = Game.Instance.UnitDatabase.GetUnitTemplate(Units[i].index),
          CardData = GetCardData(),
        };
      }
      return unitData;
    }
    // TODO:优化 TEST
    private CardData[] GetCardData() {
      CardData[] cardData = new CardData[Cards.Count];
      for (int i = 0; i < cardData.Length; i++) {
        cardData[i] = new CardData {
          Lv = Cards[i].lv,
          Template = Game.Instance.CardDatabase.GetCardTemplate(Cards[i].index),
        };
      }
      return cardData;
    }

    public void UpdateMapCurPos(int pos) {
      Map.UpdateCurPos(pos);
      SaveData();
    }

    public static User LoadFromLocal() {
      if (PlayerPrefs.HasKey(nameof(User))) {
        try {
          return JsonConvert.DeserializeObject<User>(PlayerPrefs.GetString(nameof(User)));
        } catch(Exception e) {
          Debug.LogException(e);
          return CreateNew();
        }
      }
      return null;
    }

    public static User CreateNew() {
      User user = new User {
        Map = Map.Generate()
      };
      // ----------------TEST-----------------
      user.Cards.Add((0, 0));
      user.Cards.Add((0, 0));
      user.Cards.Add((0, 0));
      user.Cards.Add((0, 0));
      user.Cards.Add((1, 0));
      user.Cards.Add((2, 0));
      user.Units.Add((0, 0));
      // -------------------------------------
      user.SaveData();
      return user;
    }

    public void SaveData() => PlayerPrefs.SetString(nameof(User), JsonConvert.SerializeObject(this));
  }
}