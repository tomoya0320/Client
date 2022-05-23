namespace Battle {
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
    public int RuntimeId { get; private set; }
    public int Level { get; private set; }
    public Player Player { get; private set; }
    public UnitTemplate UnitTemplate { get; private set; }
    public Blackboard Blackboard { get; private set; }
    public Attrib[] Attribs { get; private set; }
    public string Name => UnitTemplate?.Name;

    public Unit(BattleManager battleManager, int runtimeId, int level, Player player, UnitTemplate unitTemplate) : base(battleManager) {
      RuntimeId = runtimeId;
      Level = level;
      Player = player;
      UnitTemplate = unitTemplate;
      Blackboard = BattleManager.ObjectPool.Get<Blackboard>();
      //  Ù–‘œ‡πÿ
      Attribs = BattleManager.AttribManager.GetAttribs(unitTemplate.AttribId, Level);
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