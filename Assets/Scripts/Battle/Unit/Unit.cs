using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore {
  public enum CardHeapType {
    [InspectorName("抽牌堆")]
    DRAW,
    [InspectorName("弃牌堆")]
    DISCARD,
    [InspectorName("耗牌堆")]
    CONSUME,
    [InspectorName("手牌堆")]
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
    public PlayerCamp PlayerCamp => Player.PlayerCamp;
    public int RuntimeId { get; private set; }
    public string TemplateId => UnitData.TemplateId;
    public int Lv => UnitData.Lv;
    public int MaxLv => UnitTemplate.MaxLevel;
    public UnitData UnitData { get; private set; }
    public Blackboard Blackboard { get; private set; }
    public Attrib[] Attribs { get; private set; }
    public Dictionary<CardHeapType, List<Card>> CardHeapDict { get; private set; }
    public string Name => UnitTemplate != null ? UnitTemplate.Name : null;
    public bool IsMaster => Player.Master == this;
    private bool BehaviorInited;

    public Unit(Battle battle, int runtimeId, Player player, UnitData unitData) : base(battle) {
      Blackboard = Battle.ObjectPool.Get<Blackboard>();
      RuntimeId = runtimeId;
      Player = player;
      UnitData = unitData;

      Battle.UnitManager.Templates.TryGetValue(TemplateId, out UnitTemplate);

      // 卡牌相关
      CardHeapDict = new Dictionary<CardHeapType, List<Card>>();
      foreach (CardHeapType cardHeapType in Enum.GetValues(typeof(CardHeapType))) {
        CardHeapDict.Add(cardHeapType, new List<Card>());
      }
      foreach (var cardData in UnitData.CardData) {
        CardHeapDict[CardHeapType.DRAW].Add(Battle.CardManager.Create(this, cardData));
      }

      Attribs = Battle.AttribManager.GetAttribs(UnitTemplate.AttribId, Lv, MaxLv);
      for (int i = 0; i < Attribs.Length; i++) {
        Attribs[i].AllowExceedMax = false;
        Attribs[i].AllowNegative = false;
      }
      Attribs[(int)AttribType.ATK].AllowExceedMax = true;
      Attribs[(int)AttribType.ENERGY].AllowExceedMax = true;
    }

    public async UniTask InitBehavior() {
      if (BehaviorInited) {
        Debug.LogError($"单位行为树已初始化! id:{RuntimeId}");
        return;
      }
      // 行为树相关
      foreach (var behaviorId in UnitTemplate.BehaviorIds) {
        await Battle.BehaviorManager.Add(behaviorId, this, this);
      }
      BehaviorInited = true;
    }

    public int AddAttrib(AttribType type, int value, bool onMaxValue = false) {
      ref Attrib attrib = ref GetAttrib(type);
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
      await Battle.BehaviorManager.RunRoot(TrickTime.ON_UNIT_DYING, this, damageContext);
      if(GetAttrib(AttribType.HP).Value <= 0) {
        UnitState = UnitState.DEAD;
        Battle.UnitManager.OnUnitDie(this);
        await Battle.BehaviorManager.RunRoot(TrickTime.ON_UNIT_DEAD, this, damageContext);
      }

      Battle.ObjectPool.Release(damageContext);
    }

    public ref Attrib GetAttrib(AttribType type) => ref Attribs[(int)type];

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

    public bool EndTurn() {
      var endTurnOp = Battle.ObjectPool.Get<EndTurnOp>();
      endTurnOp.Unit = this;
      if (!Player.DoOperation(endTurnOp)) {
        Battle.ObjectPool.Release(endTurnOp);
        return false;
      }
      return true;
    }

    public bool PlayCard(Card card, Unit mainTarget) {
      if (card.CardHeapType != CardHeapType.HAND || !card.CheckTargetCamp(mainTarget) || !card.TryPlay(mainTarget)) {
        return false;
      }

      card.CardHeapType = CardHeapType.DISCARD;
      CardHeapDict[CardHeapType.HAND].Remove(card);
      CardHeapDict[CardHeapType.DISCARD].Add(card);

      var playCardOp = Battle.ObjectPool.Get<PlayCardOp>();
      playCardOp.Unit = this;
      playCardOp.MainTarget = mainTarget;
      playCardOp.Card = card;

      if (!Player.DoOperation(playCardOp)) {
        Battle.ObjectPool.Release(playCardOp);
        return false;
      }

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
      RefreshCardList(CardHeapType.HAND);
    }
  }
}