using Battle;
using System.Collections;
using BehaviorTree.Battle;
using UnityEngine;

public class GamePlayTest : MonoBehaviour {
  public BehaviorGraph BehaviorGraph;

  private void Start() {
    BehaviorGraph?.Init(BattleManager.Enter(null));
    BehaviorGraph?.Run();
  }

  private void OnApplicationQuit() {
    BehaviorGraph?.BattleManager?.Exit(true);
  }
}