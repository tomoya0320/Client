using UnityEngine;

namespace GameCore {
  public class PrefabManager : AssetManager<GameObject> {
    public PrefabManager(Battle battle) : base(battle) { }
  }
}