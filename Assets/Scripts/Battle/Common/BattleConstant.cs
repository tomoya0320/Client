using System;

namespace GameCore {
  public static class BattleConstant {
    public const int THOUSAND = 1000;
    public static readonly int ATTRIB_COUNT = Enum.GetValues(typeof(AttribType)).Length;
    public static readonly int TURN_PHASE_COUNT = Enum.GetValues(typeof(BattleTurnPhase)).Length;
  }
}