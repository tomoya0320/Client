using GameCore.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.Test {
  public class BattleTest : MonoBehaviour {
    [LabelText("战斗数据")]
    public BattleData BattleData;

    private void Awake() => UIMain.OnStart += async () => {
      var ui = await UIManager.Instance.Open<UILoading>(UIType.TOP, "UILoading");
      Battle.Enter(BattleData, async () => await UIManager.Instance.Close(ui));
    };
  }
}