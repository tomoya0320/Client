using UnityEngine;

namespace GameCore {
  public class PrefabManager : BattleResManager<GameObject> {
    public PrefabManager(Battle battle) : base(battle) { }
  }
}