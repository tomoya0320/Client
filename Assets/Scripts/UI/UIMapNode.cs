using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI {
  public class UIMapNode : MonoBehaviour {
    private int Pos = -1;
    private MapNodeBase MapNode;
    public Button Button { get; private set; }

    private void Awake() {
      Button = GetComponent<Button>();
      Button.onClick.AddListener(() => {
        if (MapNode) {
          MapNode.Run(Pos);
        }
      });
    }

    public async UniTask Init(bool enable, int no, int pos) {
      Button.interactable = enable;
      MapNode = await ResourceManager.LoadAssetAsync(Game.Instance.MapNodeDatabase.GetMapNode(no));
      Pos = pos;
    }
  }
}
