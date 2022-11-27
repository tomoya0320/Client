using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace GameCore {
  public class BattleCardControl {
    public Unit Owner { get; private set; }
    public int PlayCardCount { get; private set; }
    public List<Card> Cards = new List<Card>();

    public BattleCardControl(Unit owner, CardData[] cardData) {
      Owner = owner;
      // ø®≈∆œ‡πÿ
      foreach (var data in cardData) {
        Cards.Add(Owner.Battle.CardManager.Create(Owner, data));
      }
    }

    public bool PlayCard(Card card, Unit mainTarget) {
      if (Owner.Player.EndTurnFlag || !UnitManager.CheckTargetCamp(card.TargetCamp, card.Owner, mainTarget) || !card.PrePlay(mainTarget)) {
        return false;
      }

      var playCardOp = Owner.Battle.ObjectPool.Get<PlayCardOp>();
      playCardOp.Unit = Owner;
      playCardOp.MainTarget = mainTarget;
      playCardOp.Card = card;

      Owner.Player.AddOperation(playCardOp);

      return true;
    }

    public async UniTask OnPlayedCard(Card card) {
      PlayCardCount++;
      await card.SetCardHeapType(card.Consumable ? CardHeapType.CONSUME : CardHeapType.DISCARD);
    }

    public void GetCardList(CardHeapType cardHeapType, List<Card> list) {
      list.Clear();
      foreach (var card in Cards) {
        if (card.CardHeapType == cardHeapType) {
          list.Add(card);
        }
      }
    }

    public int GetCardCount(CardHeapType cardHeapType) {
      int count = 0;
      foreach (var card in Cards) {
        if (card.CardHeapType == cardHeapType) {
          count++;
        }
      }

      return count;
    }
  }
}