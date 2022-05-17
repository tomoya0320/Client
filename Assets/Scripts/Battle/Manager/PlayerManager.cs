using System.Collections.Generic;

namespace Battle {
  public class PlayerManager : BattleBase {
    private Dictionary<int, Player> RuntimePlayers = new Dictionary<int, Player>();

    public PlayerManager(BattleManager battleManager) : base(battleManager) {

    }
  }
}