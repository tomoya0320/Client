using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace GameCore {
  public enum CardHeapType {
    /// <summary>
    /// ≥È≈∆∂—
    /// </summary>
    DRAW,
    /// <summary>
    /// ∆˙≈∆∂—
    /// </summary>
    DISCARD,
    /// <summary>
    /// ∫ƒ≈∆∂—
    /// </summary>
    CONSUME,
    /// <summary>
    ///  ÷≈∆∂—
    /// </summary>
    HAND,
  }

  public enum UnitState {
    ALIVE,
    DYING,
    DEAD,
  }

  public class Unit : BattleBase {
    private UnitTemplate UnitTemplate;
    public UnitState UnitState { get; private set; } = UnitState.ALIVE;
    public Player Player { get; private set; }
    public int RuntimeId { get; private set; }
    public int Level { get; private set; }
    public int MaxLevel { get; private set; }
    public Blackboard Blackboard { get; private set; }
    public Attrib[] Attribs { get; private set; }
    public Dictionary<CardHeapType, List<Card>> CardHeapDict { get; private set; }
    public string Name => UnitTemplate != null ? UnitTemplate.Name : null;
    public bool IsMaster => Player.Master == this;

    public Unit(Battle battle, int runtimeId, Player player, UnitData unitData) : base(battle) {
      Blackboard = Battle.ObjectPool.Get<Blackboard>();
      RuntimeId = runtimeId;
      Level = unitData.Lv;
      Player = player;
      Battle.UnitManager.Templates.TryGetValue(unitData.TemplateId, out UnitTemplate);
      MaxLevel = UnitTemplate.MaxLevel;

      // ø®≈∆œ‡πÿ
      foreach (CardHeapType cardHeapType in Enum.GetValues(typeof(CardHeapType))) {
        CardHeapDict.Add(cardHeapType, new List<Card>());
      }

      foreach (var cardData in unitData.CardData) {
        CardHeapDict[CardHeapType.DRAW].Add(new Card(Battle, this, cardData));
      }

      //  Ù–‘œ‡πÿ
      Attribs = Battle.AttribManager.GetAttribs(UnitTemplate.AttribId, Level, MaxLevel);
      for (int i = 0; i < Attribs.Length; i++) {
        Attribs[i].AllowExceedMax = false;
        Attribs[i].AllowNegative = false;
      }
      Attribs[(int)AttribType.ATK].AllowExceedMax = true;
      Attribs[(int)AttribType.ENERGY].AllowExceedMax = true;
    }

    public int AddAttrib(AttribType type, int value, bool onMaxValue = false) {
      Attrib attrib = GetAttrib(type);
      int realAttribValue;
      if (onMaxValue) {
        realAttribValue = attrib.AddMaxValue(value);
      } else {
        realAttribValue = attrib.AddValue(value);
      }
      return realAttribValue;
    }

    public int SetAttrib(AttribType type, int value, bool onMaxValue = false) {
      Attrib attrib = GetAttrib(type);
      int addValue;
      if (onMaxValue) {
        addValue = value - attrib.MaxValue;
      } else {
        addValue = value - attrib.Value;
      }
      return AddAttrib(type, addValue, onMaxValue);
    }

    public async UniTask TryDie(Unit source, int damageValue) {
      DamageContext damageContext = Battle.ObjectPool.Get<DamageContext>();
      damageContext.Source = source;
      damageContext.Target = this;
      damageContext.DamageValue = damageValue;

      UnitState = UnitState.DYING;
      await Battle.BehaviorManager.Run(BehaviorTime.ON_UNIT_DYING, this, damageContext);
      if(GetAttrib(AttribType.HP).Value <= 0) {
        UnitState = UnitState.DEAD;
        Battle.UnitManager.OnUnitDie(RuntimeId);
        await Battle.BehaviorManager.Run(BehaviorTime.ON_UNIT_DEAD, this, damageContext);
      }

      Battle.ObjectPool.Release(damageContext);
    }

    public Attrib GetAttrib(AttribType type) => Attribs[(int)type];

    public void RefreshEnergy() {
      var energyAttrib = GetAttrib(AttribType.ENERGY);
      SetAttrib(AttribType.ENERGY, energyAttrib.MaxValue);
    }

    private void RefreshCardList(CardHeapType cardHeapType) {
      var cardList = CardHeapDict[cardHeapType];
      for (int i = cardList.Count - 1; i >= 0; i--) {
        var card = cardList[i];
        if (card.CardHeapType != cardHeapType) {
          cardList.RemoveAt(i);
        }
      }
    }

    public bool PlayCard(Card card, Unit mainTarget) {
      // Test
      if (card.CardHeapType != CardHeapType.HAND) {
        return false;
      }

      if (!card.TryPlay(mainTarget)) {
        return false;
      }

      card.CardHeapType = CardHeapType.DISCARD;
      CardHeapDict[CardHeapType.HAND].Remove(card);
      CardHeapDict[CardHeapType.DISCARD].Add(card);

      var playCardOp = Battle.ObjectPool.Get<PlayCardOp>();
      playCardOp.Unit = this;
      playCardOp.MainTarget = mainTarget;
      playCardOp.Card = card;

      Player.AddOperation(playCardOp);

      return true;
    }

    public void DrawCard() {
      // Test
      var handCardList = CardHeapDict[CardHeapType.HAND];
      int drawCardCount = BattleConstant.MAX_HAND_CARD_COUNT - handCardList.Count;
      if (drawCardCount > 0) {
        var drawCardList = CardHeapDict[CardHeapType.DRAW];
        if(drawCardList.Count < drawCardCount) {
          var discardCardList = CardHeapDict[CardHeapType.DISCARD];
          for (int i = 0; i < discardCardList.Count; i++) {
            var card = discardCardList[i];
            drawCardList.Add(card);
            card.CardHeapType = CardHeapType.DRAW;
          }
          RefreshCardList(CardHeapType.DISCARD);
        }
        MathUtil.FisherYatesShuffle(drawCardList);
        for (int i = 0; i < drawCardList.Count && i < drawCardCount; i++) {
          var card = drawCardList[i];
          handCardList.Add(card);
          card.CardHeapType = CardHeapType.HAND;
        }
        RefreshCardList(CardHeapType.DRAW);
      }
    }

    public void DiscardCard() {
      // Test
      var handCardList = CardHeapDict[CardHeapType.HAND];
      var discardCardList = CardHeapDict[CardHeapType.DISCARD];
      for (int i = 0; i < handCardList.Count; i++) {
        var card = handCardList[i];
        discardCardList.Add(card);
        card.CardHeapType = CardHeapType.DISCARD;
      }
    }
  }
}