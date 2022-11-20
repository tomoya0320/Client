using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.Test {
  public class BattleTest : MonoBehaviour {
    [LabelText("战斗数据")]
    public BattleData BattleData;

    private void Start() {
      Application.targetFrameRate = 120;
      Battle.Enter(BattleData);
    }
  }
}