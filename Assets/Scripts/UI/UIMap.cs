using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameCore.UI {
  public class UIMap : UIBase {
    [SerializeField]
    private Transform MapNodeRoot;
    private UIMapNode[] MapNodes;

    private void Awake() {
      MapNodes = MapNodeRoot.GetComponentsInChildren<UIMapNode>(true);
    }

    public async override UniTask Init(UIType type, params object[] args) {
      EventCenter.AddListener(EventType.ON_MAP_CUR_POS_UPDATE, OnMapCurPosUpdate);
      var map = Game.Instance.User.Map;
      for (int i = 0; i < MapNodes.Length; i++) {
        await MapNodes[i].Init(map.CheckNodeEnable(i), map.Nodes[i], i);
      }
      await base.Init(type, args);
    }

    public override void OnRemove() {
      EventCenter.RemoveListener(EventType.ON_MAP_CUR_POS_UPDATE, OnMapCurPosUpdate);
      base.OnRemove();
    }

    public void OnMapCurPosUpdate() {
      var map = Game.Instance.User.Map;
      for (int i = 0; i < MapNodes.Length; i++) {
        MapNodes[i].Button.interactable = map.CheckNodeEnable(i);
      }
    }
  }
}