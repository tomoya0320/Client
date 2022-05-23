namespace Battle {
  public enum CardHeapType {
    /// <summary>
    /// ���ƶ�
    /// </summary>
    DRAW,
    /// <summary>
    /// ���ƶ�
    /// </summary>
    DISCARD,
    /// <summary>
    /// ���ƶ�
    /// </summary>
    CONSUME,
    /// <summary>
    /// ���ƶ�
    /// </summary>
    HAND,
    /// <summary>
    /// ���ƶ�
    /// </summary>
    PLAY,
  }

  public class Unit : BattleBase {
    public int RuntimeId { get; private set; }
    public Player Player { get; private set; }
    public UnitTemplate UnitTemplate { get; private set; }
    public Blackboard Blackboard { get; private set; }
    public Attrib[] Attribs { get; private set; }
    public string Name => UnitTemplate?.Name;

    public Unit(BattleManager battleManager, int runtimeId, Player player, UnitTemplate unitTemplate) : base(battleManager) {
      RuntimeId = runtimeId;
      Player = player;
      UnitTemplate = unitTemplate;
      Blackboard = BattleManager.ObjectPool.Get<Blackboard>();
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