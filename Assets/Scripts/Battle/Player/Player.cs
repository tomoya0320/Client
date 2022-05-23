using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore {
  public class Player : BattleBase {
    public int RuntimeId { get; private set; }
    public Blackboard Blackboard { get; private set; }
    public Unit Master { get; private set; }
    public Unit[] Units { get; private set; }

    public Player(Battle battle) : base(battle) {
      Blackboard = Battle.ObjectPool.Get<Blackboard>();
    }

    public void Init() {

    }
  }
}