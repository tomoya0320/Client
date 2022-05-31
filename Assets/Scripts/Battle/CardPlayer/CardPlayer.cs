using UnityEngine;

namespace GameCore {
  public abstract class CardPlayer {
    public abstract bool TryPlay(Card card, Unit mainTarget);
  }

  public class DefaultCardPlayer : CardPlayer {
    public override bool TryPlay(Card card, Unit mainTarget) {
      int cost = card.Cost;
      Unit owner = card.Owner;
      if (cost < 0) {
        Debug.LogError("cost < 0");
        return false;
      }
      if (cost > owner.GetAttrib(AttribType.ENERGY).Value) {
        Debug.LogError("energy is not enough");
        return false;
      }
      owner.AddAttrib(AttribType.ENERGY, -cost);
      return true;
    }
  }
}