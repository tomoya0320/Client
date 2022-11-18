using System.Collections.Generic;

namespace GameCore {
  public class BattleCardControl {
    public Unit Owner { get; private set; }
    public int PlayCardCount { get; private set; }
    private List<Card> Cards = new List<Card>();

    public BattleCardControl(Unit owner, CardData[] cardData) {
      Owner = owner;
      // ø®≈∆œ‡πÿ
      foreach (var data in cardData) {
        Cards.Add(Owner.Battle.CardManager.Create(Owner, data));
      }
    }

    public bool PlayCard(Card card, Unit mainTarget) {
      if (Owner.Player.EndTurnFlag || !card.CheckTargetCamp(mainTarget) || !card.PrePlay(mainTarget)) {
        return false;
      }

      var playCardOp = Owner.Battle.ObjectPool.Get<PlayCardOp>();
      playCardOp.Unit = Owner;
      playCardOp.MainTarget = mainTarget;
      playCardOp.Card = card;

      Owner.Player.AddOperation(playCardOp);

      return true;
    }

    public void OnPlayedCard(Card card) {
      card.CardHeapType = card.Consumable ? CardHeapType.CONSUME : CardHeapType.DISCARD;
      PlayCardCount++;
    }

    public List<Card> this[CardHeapType cardHeapType] => Cards.FindAll(card => card.CardHeapType == cardHeapType);
  }
}