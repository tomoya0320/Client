using System;
using System.Collections.Generic;

namespace GameCore {
  public class BattleCardControl {
    public Unit Owner { get; private set; }
    public int PlayCardCount { get; private set; }
    private Dictionary<CardHeapType, List<Card>> CardHeapDict = new Dictionary<CardHeapType, List<Card>>();

    public BattleCardControl(Unit owner, CardData[] cardData) { 
      Owner = owner;
      // ø®≈∆œ‡πÿ
      foreach (CardHeapType cardHeapType in Enum.GetValues(typeof(CardHeapType))) {
        CardHeapDict.Add(cardHeapType, new List<Card>());
      }
      foreach (var data in cardData) {
        CardHeapDict[CardHeapType.DRAW].Add(Owner.Battle.CardManager.Create(Owner, data));
      }
    }

    public void RefreshCardList(CardHeapType cardHeapType) {
      var cardList = CardHeapDict[cardHeapType];
      for (int i = cardList.Count - 1; i >= 0; i--) {
        var card = cardList[i];
        if (card.CardHeapType != cardHeapType) {
          cardList.RemoveAt(i);
        }
      }
    }

    public bool PlayCard(Card card, Unit mainTarget) {
      if (card.CardHeapType != CardHeapType.HAND || !card.CheckTargetCamp(mainTarget) || !card.TryPlay(mainTarget)) {
        return false;
      }

      card.CardHeapType = CardHeapType.DISCARD;
      this[CardHeapType.HAND].Remove(card);
      this[CardHeapType.DISCARD].Add(card);

      var playCardOp = Owner.Battle.ObjectPool.Get<PlayCardOp>();
      playCardOp.Unit = Owner;
      playCardOp.MainTarget = mainTarget;
      playCardOp.Card = card;

      Owner.Player.AddOperation(playCardOp);

      PlayCardCount++;

      return true;
    }

    public List<Card> this[CardHeapType cardHeapType] => CardHeapDict[cardHeapType];
  }
}