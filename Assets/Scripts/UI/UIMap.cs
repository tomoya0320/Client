using Cysharp.Threading.Tasks;

namespace GameCore.UI {
  public class UIMap : UIBase {
    public UIMapNode[,] MapNodes = new UIMapNode[Map.WIDTH, Map.HEIGHT];
    public UIMapNode DestNode;

    public async override UniTask Init(UIType type, params object[] args) {
      EventCenter.AddListener(EventType.ON_MAP_CUR_POS_UPDATE, OnMapCurPosUpdate);
      var map = Game.Instance.User.Map;
      for (int i = 0; i < Map.WIDTH; i++) {
        for (int j = 0; j < Map.HEIGHT; j++) {
          int pos = i * Map.HEIGHT + j;
          await MapNodes[i, j].Init(map.CheckNodeEnable(pos), map.Nodes[pos], pos);
        }
      }
      await DestNode.Init(map.CheckNodeEnable(Map.DEST_POS), map.DestNode, Map.DEST_POS);
      await base.Init(type, args);
    }

    public override void OnRemove() {
      EventCenter.RemoveListener(EventType.ON_MAP_CUR_POS_UPDATE, OnMapCurPosUpdate);
      base.OnRemove();
    }

    public void OnMapCurPosUpdate() {
      var map = Game.Instance.User.Map;
      for (int i = 0; i < Map.WIDTH; i++) {
        for (int j = 0; j < Map.HEIGHT; j++) {
          int pos = i * Map.HEIGHT + j;
          MapNodes[i, j].Button.interactable = map.CheckNodeEnable(pos);
        }
      }
      DestNode.Button.interactable = map.CheckNodeEnable(Map.DEST_POS);
    }
  }
}