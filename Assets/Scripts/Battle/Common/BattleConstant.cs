using System;

namespace GameCore {
  public static class BattleConstant {
    public const int THOUSAND = 1000;
    public const int MAX_HAND_CARD_COUNT = 12;
    public static readonly int ATTRIB_COUNT = Enum.GetValues(typeof(AttribType)).Length;
    public static readonly int TURN_PHASE_COUNT = Enum.GetValues(typeof(BattleTurnPhase)).Length;
  }
}