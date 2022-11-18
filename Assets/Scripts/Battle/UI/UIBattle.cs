using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore {
  public class UIBattle : MonoBehaviour {
    private Battle Battle;
    [SerializeField]
    private Text TurnText;
    public Transform DrawNode;
    public Transform DiscardNode;
    [SerializeField]
    private Transform[] AllyNodes;
    [SerializeField]
    private Transform[] EnemyNodes;

    public void Init(Battle battle) {
      Battle = battle;
      Battle.SelfPlayer.OnStartTurn += OnSelfStartTurn;
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

    public void EndTurn() => Battle.SelfPlayer.EndTurnFlag = true;

    public void OnSelfStartTurn() {
      if (TurnText) {
        TurnText.text = $"{Battle.Turn}";
      }
    }
    // Test
    public void LogDrawCards() {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (var card in Battle.SelfPlayer.Master.BattleCardControl[CardHeapType.DRAW]) {
        stringBuilder.Append(card);
      }
      Debug.Log(stringBuilder);
    }
    // Test
    public void LogDiscardCards() {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (var card in Battle.SelfPlayer.Master.BattleCardControl[CardHeapType.DISCARD]) {
        stringBuilder.Append(card);
      }
      Debug.Log(stringBuilder);
    }
    // Test
    public void LogConsumeCards() {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (var card in Battle.SelfPlayer.Master.BattleCardControl[CardHeapType.CONSUME]) {
        stringBuilder.Append(card);
      }
      Debug.Log(stringBuilder);
    }
  }
}