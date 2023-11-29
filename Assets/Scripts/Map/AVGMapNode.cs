using GameCore.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [CreateAssetMenu(fileName = "AVGMapNode", menuName = "��ͼ�ڵ�/����")]
  public class AVGMapNode : MapNodeBase {
    [LabelText("����")]
    public AssetReferenceT<AVGGraph> AVGGraph;

    public override async void Run(int pos) {
      var ui = await AVG.Enter(AVGGraph);
      ui.AVG.OnExit = async () => {
        Game.Instance.User.UpdateMapCurPos(pos);
        await UIManager.Instance.Close(ui);
      };
    }
  }
}