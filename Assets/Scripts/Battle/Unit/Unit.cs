using Cysharp.Threading.Tasks;
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


  public class Unit : BattleBase {
    private UnitTemplate UnitTemplate;
    public bool IsAlive => UnitStateMachine.IsAlive;
    public Player Player { get; private set; }
    public PlayerCamp PlayerCamp => Player.PlayerCamp;
    public int RuntimeId { get; private set; }
    public string TemplateId => UnitData.TemplateId;
    public int Lv => UnitData.Lv;
    public int MaxLv => UnitTemplate.MaxLevel;
    public UnitData UnitData { get; private set; }
    public Blackboard Blackboard { get; private set; }
    public Attrib[] Attribs { get; private set; }
    public string Name => UnitTemplate != null ? UnitTemplate.Name : null;
    public bool IsMaster => Player.Master == this;
    private bool BehaviorInited;
    public UnitStateMachine UnitStateMachine { get; private set; }
    public BattleCardControl BattleCardControl { get; private set; }

    public Unit(Battle battle, int runtimeId, Player player, UnitData unitData) : base(battle) {
      Blackboard = Battle.ObjectPool.Get<Blackboard>();
      RuntimeId = runtimeId;
      Player = player;
      UnitData = unitData;
      UnitStateMachine = new UnitStateMachine(this);
      BattleCardControl = new BattleCardControl(this, UnitData.CardData);

      Battle.UnitManager.TryGetTemplate(TemplateId, out UnitTemplate);
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

    public int AddAttrib(AttribType type, int value, AttribField attribField = AttribField.VALUE) {
      ref Attrib attrib = ref GetAttrib(type);
      int realAttribValue;
      switch (attribField) {
        case AttribField.VALUE:
          realAttribValue = attrib.AddValue(value);
          break;
        case AttribField.MAX_VALUE:
          realAttribValue = attrib.AddMaxValue(value);
          break;
        default:
          realAttribValue = 0;
          break;
      }
      return realAttribValue;
    }

    public int SetAttrib(AttribType type, int value, AttribField attribField = AttribField.VALUE) {
      Attrib attrib = GetAttrib(type);
      int addValue;
      switch (attribField) {
        case AttribField.VALUE:
          addValue = value - attrib.Value;
          break;
        case AttribField.MAX_VALUE:
          addValue = value - attrib.MaxValue;
          break;
        default:
          addValue = 0;
          break;
      }
      return AddAttrib(type, addValue, attribField);
    }

    public async UniTask TryDie(Unit source, int damageValue) {
      DamageContext damageContext = Battle.ObjectPool.Get<DamageContext>();
      damageContext.Source = source;
      damageContext.Target = this;
      damageContext.DamageValue = damageValue;

      await Battle.BehaviorManager.RunRoot(TickTime.ON_UNIT_WILL_DIE, this, damageContext);
      if(GetAttrib(AttribType.HP).Value <= 0) {
        await UnitStateMachine.SwitchState((int)UnitState.DYING);
        Battle.UnitManager.OnUnitDie(this);
      }

      Battle.ObjectPool.Release(damageContext);
    }

    public ref Attrib GetAttrib(AttribType type) => ref Attribs[(int)type];

    public void RefreshEnergy() {
      var energyAttrib = GetAttrib(AttribType.ENERGY);
      SetAttrib(AttribType.ENERGY, energyAttrib.MaxValue);
    }

    public bool EndTurn() {
      var endTurnOp = Battle.ObjectPool.Get<EndTurnOp>();
      endTurnOp.Unit = this;
      Player.AddOperation(endTurnOp);
      return true;
    }

    public bool PlayCard(Card card, Unit mainTarget) {
      if (card.CardHeapType != CardHeapType.HAND || !card.CheckTargetCamp(mainTarget) || !card.TryPlay(mainTarget)) {
        return false;
      }

      card.CardHeapType = CardHeapType.DISCARD;
      BattleCardControl[CardHeapType.HAND].Remove(card);
      BattleCardControl[CardHeapType.DISCARD].Add(card);

      var playCardOp = Battle.ObjectPool.Get<PlayCardOp>();
      playCardOp.Unit = this;
      playCardOp.MainTarget = mainTarget;
      playCardOp.Card = card;

      Player.AddOperation(playCardOp);

      return true;
    }
  }
}