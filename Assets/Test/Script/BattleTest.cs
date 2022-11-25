using GameCore.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.Test {
  public class BattleTest : MonoBehaviour {
    [LabelText("战斗数据")]
    public BattleData BattleData;

    public async void Test() {
      var ui = await UIManager.Instance.Open<UILoading>(UIType.TOP, "UILoading");
      Battle.Enter(BattleData, async () => await UIManager.Instance.Close(ui));
    }
  }
}