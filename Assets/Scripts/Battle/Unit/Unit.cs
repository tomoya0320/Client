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
    /// <summary>
    /// ≥ˆ≈∆∂—
    /// </summary>
    PLAY,
  }

  public class Unit : BattleBase {
    private UnitTemplate UnitTemplate;
    public bool IsDead { get; private set; }
    public Player Player { get; private set; }
    public int RuntimeId { get; private set; }
    public int Level { get; private set; }
    public int MaxLevel { get; private set; }
    public Blackboard Blackboard { get; private set; }
    public Attrib[] Attribs { get; private set; }
    public string Name => UnitTemplate != null ? UnitTemplate.Name : null;

    public Unit(Battle battle) : base(battle) {
      Blackboard = Battle.ObjectPool.Get<Blackboard>();
    }

    public void Init(int runtimeId, Player player, UnitData unitData) {
      RuntimeId = runtimeId;
      Level = unitData.Lv;
      Player = player;
      Battle.UnitManager.Templates.TryGetValue(unitData.TemplateId, out UnitTemplate);
      MaxLevel = UnitTemplate.MaxLevel;

      // TODO:ø®≈∆≥ı ºªØ

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

    public Attrib GetAttrib(AttribType type) => Attribs[(int)type];
  }
}