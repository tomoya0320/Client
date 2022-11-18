using System.Collections.Generic;

namespace GameCore {
  public class PlayerManager : BattleBase {
    private int IncId;
    private int LoopIndex = -1;
    public List<Player> PlayerList = new List<Player>();
    private Dictionary<int, Player> Players = new Dictionary<int, Player>();

    public PlayerManager(Battle battle) : base(battle) { }

    public Player MoveNext() {
      do {
        LoopIndex = (LoopIndex + 1) % PlayerList.Count;
      } while (!PlayerList[LoopIndex].Available);
      var nextPlayer = PlayerList[LoopIndex];
      nextPlayer.EndTurnFlag = false;
      return nextPlayer;
    }

    public Player Create(PlayerData playerData) {
      var player = new Player(Battle, ++IncId, playerData);
      Players.Add(player.RuntimeId, player);
      PlayerList.Add(player);
      return player;
    }
  }
}