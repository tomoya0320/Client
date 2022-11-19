using UnityEngine;

namespace GameCore {
  public class SpriteManager : AssetManager<Sprite> {
    public SpriteManager(Battle battle) : base(battle) { }
  }
}