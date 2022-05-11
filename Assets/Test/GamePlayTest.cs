using Battle;
using System.Collections;
using BehaviorTree.Battle;
using UnityEngine;

public class GamePlayTest : MonoBehaviour {
  public BattleData BattleData;

  private void Start() {
    BattleManager.Enter(BattleData);

  }
}