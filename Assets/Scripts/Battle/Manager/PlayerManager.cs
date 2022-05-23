using System.Collections.Generic;

namespace GameCore {
  public class PlayerManager : BattleBase {
    private Dictionary<int, Player> Players = new Dictionary<int, Player>();

    public PlayerManager(Battle battle) : base(battle) {

    }
  }
}