using Cysharp.Threading.Tasks;

namespace GameCore.UI {
  public class UIMap : UIBase {
    public UIMapNode[,] MapNodes = new UIMapNode[Map.WIDTH, Map.HEIGHT];
    public UIMapNode DestNode;

    public async override UniTask Init(UIType type, params object[] args) {
      for (int i = 0; i < Map.WIDTH; i++) {
        for (int j = 0; j < Map.HEIGHT; j++) {
          await MapNodes[i, j].Init(Game.Instance.User.Map.Nodes[i * Map.HEIGHT + j]);
        }
      }
      await DestNode.Init(Game.Instance.User.Map.DestNode);
      await base.Init(type, args);
    }

    public async void Close() {
      await UIManager.Instance.Close(this);
    }
  }
}