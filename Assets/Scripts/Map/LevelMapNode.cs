using GameCore.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [CreateAssetMenu(fileName = "LevelMapNode", menuName = "地图节点/关卡")]
  public class LevelMapNode : MapNodeBase {
    [LabelText("关卡")]
    public AssetReferenceT<LevelTemplate> Level;

    public override async void Run(int pos) {
      var ui = await UIManager.Instance.Open<UILoading>(UIType.TOP, "UILoading");
      Battle.Enter(CreateBattleData(),
        async () => await UIManager.Instance.Close(ui),
        isWin => {
          if (isWin) {
            Game.Instance.User.UpdateMapCurPos(pos);
          }
        });
    }

    private BattleData CreateBattleData() => new BattleData {
      Level = Level,
      PlayerData = Game.Instance.User.GetPlayerData(),
    };
  }
}