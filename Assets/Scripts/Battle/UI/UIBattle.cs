using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI {
  public class UIBattle : UIBase {
    private Battle Battle;
    [SerializeField]
    private Transform[] AllyNodes;
    [SerializeField]
    private Transform[] EnemyNodes;
    [SerializeField]
    private Text TurnText;
    public Transform DrawNode;
    public Transform DiscardNode;
    public Transform ConsumeNode;
    public RectTransform InHandNode;
    public Transform TextNode;
    public Transform CardNode;

    public override UIBase Init(params object[] args) {
      Battle = args[0] as Battle;
      Battle.SelfPlayer.OnStartTurn += OnSelfStartTurn;

      return base.Init(args);
    }

    public Transform GetUnitNode(PlayerCamp playerCamp, int index) {
      switch (playerCamp) {
        case PlayerCamp.ALLY:
          return GetNode(AllyNodes, index);
        case PlayerCamp.ENEMY:
          return GetNode(EnemyNodes, index);
        default:
          return null;
      }
    }

    private Transform GetNode(Transform[] nodes, int index) {
      if(index < 0 || index >= nodes.Length) {
        return null;
      }

      return nodes[index];
    }

    public void EndTurn() {
      if (Battle.BattleState == BattleState.Run) {
        Battle.SelfPlayer.EndTurnFlag = true;
      }
    }

    public void OnSelfStartTurn() {
      if (TurnText) {
        TurnText.text = $"{Battle.Turn}";
      }
    }
  }
}