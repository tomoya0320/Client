using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

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
    public string TemplateId => UnitData.Template?.AssetGUID;
    public int Lv => UnitData.Lv;
    public int MaxLv => UnitTemplate.MaxLevel;
    public UnitData UnitData { get; private set; }
    public Blackboard Blackboard { get; private set; }
    public Attrib[] Attribs { get; private set; }
    private Action<int, int>[] AttribChangedCallbacks;
    public string Name => UnitTemplate != null ? UnitTemplate.Name : null;
    public bool IsMaster => Player.Master == this;
    private bool BehaviorInited;
    public UnitStateMachine UnitStateMachine { get; private set; }
    public BattleCardControl BattleCardControl { get; private set; }
    public UIUnit UIUnit { get; private set; }
    public float DieAnimTime => UnitTemplate.DieAnimTime;

    public Unit(Battle battle, int runtimeId, Player player, UnitData unitData) : base(battle) {
      Blackboard = Battle.ObjectPool.Get<Blackboard>();
      RuntimeId = runtimeId;
      Player = player;
      UnitData = unitData;
      UnitStateMachine = new UnitStateMachine(this);
      BattleCardControl = new BattleCardControl(this, UnitData.CardData);

      Battle.UnitManager.TryGetAsset(TemplateId, out UnitTemplate);
      Attribs = Battle.AttribManager.GetAttribs(UnitTemplate.Attrib?.AssetGUID, Lv, MaxLv);
      for (int i = 0; i < Attribs.Length; i++) {
        Attribs[i].AllowExceedMax = false;
        Attribs[i].AllowNegative = false;
      }
      Attribs[(int)AttribType.ATK].AllowExceedMax = true;
      Attribs[(int)AttribType.ENERGY].AllowExceedMax = true;
      AttribChangedCallbacks = new Action<int, int>[Attribs.Length];
    }

    public async UniTask InitUI(int index) {
      if (Battle.PrefabManager.TryGetAsset(UnitTemplate.Prefab?.AssetGUID, out var prefab)) {
        UIUnit = Object.Instantiate(prefab, Battle.UIBattle.GetUnitNode(Player.PlayerCamp, index)).GetComponent<UIUnit>();
        if (UIUnit) {
          UIUnit.Init(this);
        }
      }

      if (Player.IsSelf) {
        foreach (var card in BattleCardControl.Cards) {
          await card.InitUI();
        }
      }
    }

    public async UniTask OnSettle() {
      foreach (var card in BattleCardControl.Cards) {
        await card.OnSettle();
      }
    }

    public void AddAttribChangedCallback(AttribType type, Action<int, int> callback) => AttribChangedCallbacks[(int)type] += callback;

    public async UniTask InitBehavior() {
      if (BehaviorInited) {
        Debug.LogError($"单位行为树已初始化! id:{RuntimeId}");
        return;
      }
      // 行为树相关
      foreach (var behavior in UnitTemplate.Behaviors) {
        await Battle.BehaviorManager.Add(behavior?.AssetGUID, this, this);
      }
      BehaviorInited = true;
    }

    public int AddAttrib(AttribType type, int value, AttribField attribField = AttribField.VALUE) {
      ref Attrib attrib = ref Attribs[(int)type];
      int beforeValue = attrib.Value;
      int beforeMaxValue = attrib.MaxValue;
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
          Debug.LogError($"undefined attribField! AttribField:{attribField}");
          break;
      }
      AttribChangedCallbacks[(int)type]?.Invoke(beforeValue, beforeMaxValue);
      return realAttribValue;
    }

    public int SetAttrib(AttribType type, AttribField attribField, int value) {
      int addValue = value - GetAttribField(type, attribField);
      return AddAttrib(type, addValue, attribField);
    }

    public async UniTask TryDie(DamageContext damageContext) {
      await Battle.BehaviorManager.RunRoot(TickTime.ON_UNIT_WILL_DIE, this, damageContext);
      if (Attribs[(int)AttribType.HP].Value <= 0) {
        await UnitStateMachine.SwitchState((int)UnitState.DEAD, damageContext);
      }
    }

    public int GetAttribField(AttribType type, AttribField attribField) {
      Attrib attrib = Attribs[(int)type];
      int value;
      switch (attribField) {
        case AttribField.VALUE:
          value = attrib.Value;
          break;
        case AttribField.MAX_VALUE:
          value = attrib.MaxValue;
          break;
        default:
          value = 0;
          Debug.LogError($"undefined attribField! AttribField:{attribField}");
          break;
      }

      return value;
    }
  }
}