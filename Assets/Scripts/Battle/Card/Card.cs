using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameCore {
  public class Card : BattleBase, IComparable<Card> {
    public int Order;
    public CardTemplate CardTemplate { get; private set; }
    public int RuntimeId { get; private set; }
    public PlayerCamp TargetCamp => CardTemplate.TargetCamp;
    public CardData CardData { get; private set; }
    public int Lv => CardData.Lv;
    public Unit Owner { get; private set; }
    public Skill[] Skills { get; private set; }
    private Skill Skill => Skills[Lv];
    public int Cost => CardTemplate.LvCardItems[Lv].Cost;
    public bool Consumable => CardTemplate.LvCardItems[Lv].Consumable;
    public string Desc => CardTemplate.LvCardItems[Lv].Desc;
    public string Name => CardTemplate.LvCardItems[Lv].Name;
    public CardType CardType => CardTemplate.CardType;
    private CardPrePlayer CardPlayer => CardTemplate.LvCardItems[Lv].CardPlayer;
    public CardHeapType CardHeapType { get; private set; } = CardHeapType.DRAW;
    public async UniTask SetCardHeapType(CardHeapType cardHeapType) {
      if (CardHeapType == cardHeapType) {
        return;
      }

      var beforeType = CardHeapType;
      CardHeapType = cardHeapType;
      if (CardHeapType == CardHeapType.HAND) {
        Owner.BattleCardControl.RefreshCardOrder(this);
      }

      if (UICard) {
        if (beforeType == CardHeapType.HAND || CardHeapType == CardHeapType.HAND) {
          var handCardList = TempList<Card>.Get();
          Owner.BattleCardControl.GetCardList(CardHeapType.HAND, handCardList);
          for (int i = 0; i < handCardList.Count; i++) {
            handCardList[i].UICard.InHandIndex = i;
            handCardList[i].UICard.HandCardCount = handCardList.Count;
          }
        }

        switch (CardHeapType) {
          case CardHeapType.DRAW:
            await UICard.UICardStateMachine.SwitchState((int)UICardState.IN_DRAW);
            break;
          case CardHeapType.DISCARD:
            await UICard.UICardStateMachine.SwitchState((int)UICardState.IN_DISCARD);
            break;
          case CardHeapType.CONSUME:
            await UICard.UICardStateMachine.SwitchState((int)UICardState.IN_CONSUME);
            break;
          case CardHeapType.HAND:
            await UICard.UICardStateMachine.SwitchState((int)UICardState.IN_HAND);
            break;
        }
      }
    }

    public UICard UICard { get; private set; }

    public Card(Battle battle, int runtimeId, Unit owner, CardData cardData) : base(battle) {
      RuntimeId = runtimeId;
      Owner = owner;
      CardData = cardData;
      CardTemplate = CardData.Template?.Asset as CardTemplate;

      Skills = new Skill[CardTemplate.LvCardItems.Length];
      for (int i = 0; i < Skills.Length; i++) {
        Skills[i] = new Skill(Battle, Owner, CardTemplate.LvCardItems[i].Skill?.Asset as SkillTemplate);
      }
    }

    public async UniTask InitUI() {
      UICard = Object.Instantiate(await ResourceManager.LoadAssetAsync<GameObject>("UICard"), Battle.UIBattle.CardNode).GetComponent<UICard>();
      UICard.Init(this);
    }

    public async UniTask OnSettle() {
      if (UICard) {
        await UICard.UICardStateMachine.SwitchState((int)UICardState.SETTLE);
      }
    }

    public async UniTask Cast(Unit mainTarget) => await Skill.Cast(mainTarget);

    public bool PrePlay(Unit mainTarget) => CardPlayer.PrePlay(this, mainTarget);

    public override string ToString() {
      return $"{RuntimeId}:{CardTemplate.LvCardItems[Lv].Name}\nLv:{Lv}\nCost:{CardTemplate.LvCardItems[Lv].Cost}"; // Test
    }

    public int CompareTo(Card other) => Order.CompareTo(other.Order);
  }
}