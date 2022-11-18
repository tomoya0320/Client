using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore {
  public class UIBattle : MonoBehaviour {
    private Battle Battle;
    [SerializeField]
    private Button FinishTurnButton;
    [SerializeField]
    private Button DrawButton;
    [SerializeField]
    private Button DiscardButton;
    [SerializeField]
    private Button ConsumeButton;
    [SerializeField]
    private Text TurnText;
    [SerializeField]
    private Transform[] AllyNodes;
    [SerializeField]
    private Transform[] EnemyNodes;
    public Transform DrawTransform => DrawButton ? DrawButton.transform : null;
    public Transform DiscardTransform => DiscardButton ? DiscardButton.transform : null;

    public void Init(Battle battle) {
      Battle = battle;
      Battle.SelfPlayer.OnStartTurn += OnSelfStartTurn;
    }

    public Transform GetAllyNode(int index) {
      if(index < 0 || index >= AllyNodes.Length) {
        return null;
      }

      return AllyNodes[index];
    }

    public Transform GetEnemyNode(int index) {
      if (index < 0 || index >= EnemyNodes.Length) {
        return null;
      }

      return EnemyNodes[index];
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