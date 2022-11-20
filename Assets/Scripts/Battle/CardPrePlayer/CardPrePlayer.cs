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
        owner.Battle.UIBattle.ShowText("ÄÜÁ¿²»×ã!", owner.UIUnit.BattleTextNode.position, Color.red, false);
        return false;
      }
      owner.AddAttrib(AttribType.ENERGY, -cost);
      return true;
    }
  }
}