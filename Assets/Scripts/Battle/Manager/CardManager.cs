using System.Collections.Generic;

namespace GameCore {
  public class CardManager : BattleResManager<CardTemplate> {
    private int IncId;
    private Dictionary<int, Card> Cards = new Dictionary<int, Card>();

    public CardManager(Battle battle) : base(battle) { }

    public Card Create(Unit unit, CardData cardData) {
      var card = new Card(Battle, ++IncId, unit, cardData);
      Cards.Add(card.RuntimeId, card);
      return card;
    }

    public Card GetCard(int runtimeId) {
      Cards.TryGetValue(runtimeId, out var card);
      return card;
    }
  }
}
