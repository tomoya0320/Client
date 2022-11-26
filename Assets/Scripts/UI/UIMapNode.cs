using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameCore.UI {
  public class UIMapNode : MonoBehaviour {
    private MapNodeBase MapNode;

    public async UniTask Init(int no) {
      MapNode = await ResourceManager.LoadAssetAsync(Game.Instance.MapNodeDatabase.GetMapNode(no));
    }

    public void Run() {
      if (MapNode) {
        MapNode.Run();
      }
    }
  }
}
