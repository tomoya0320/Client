using System;

namespace GameCore {
  public static class BattleConstant {
    public const int MAX_HAND_CARD_COUNT = 8;
    public static readonly int ATTRIB_COUNT = Enum.GetValues(typeof(AttribType)).Length;
  }
}