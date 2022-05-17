using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
  public class Player : BattleBase {
    public int RuntimeId { get; private set; }
    public Blackboard Blackboard { get; private set; }

    public Player(BattleManager battleManager) : base(battleManager) {

    }
  }
}