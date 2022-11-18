using UnityEngine;

namespace GameCore {
  public abstract class CardPrePlayer {
    public abstract bool PrePlay(Card card, Unit mainTarget);
  }

  public class DefaultCardPrePlayer : CardPrePlayer {
    public override bool PrePlay(Card card, Unit mainTarget) {
      int cost = card.Cost;
      Unit owner = card.Owner;
      if (cost < 0) {
        Debug.LogError("cost < 0");
        return false;
      }
      if (cost > owner.Attribs[(int)AttribType.ENERGY].Value) {
        Debug.Log("energy is not enough");
        return false;
      }
      owner.AddAttrib(AttribType.ENERGY, -cost);
      return true;
    }
  }
}