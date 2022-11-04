using System;
using System.Collections.Generic;

namespace GameCore {
  public class BattleCardControl {
    public Unit Owner { get; private set; }
    private Dictionary<CardHeapType, List<Card>> CardHeapDict = new Dictionary<CardHeapType, List<Card>>();

    public BattleCardControl(Unit owner, CardData[] cardData) { 
      Owner = owner;
      // �������
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

    public List<Card> this[CardHeapType cardHeapType] => CardHeapDict[cardHeapType];
  }
}