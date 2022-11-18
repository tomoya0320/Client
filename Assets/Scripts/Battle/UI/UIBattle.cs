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
    private Text PlayCardCountText;
    [SerializeField]
    private Transform[] AllyNodes;
    [SerializeField]
    private Transform[] EnemyNodes;
    public Transform DrawTransform => DrawButton ? DrawButton.transform : null;
    public Transform DiscardTransform => DiscardButton ? DiscardButton.transform : null;

    public void Init(Battle battle) {
      Battle = battle;

      // Test
      FinishTurnButton?.onClick.AddListener(() => {
        if (Battle.CurPlayer == Battle.SelfPlayer) {
          Battle.CurPlayer.EndTurnFlag = true;
        }
      });
      DrawButton?.onClick.AddListener(() => LogCards(CardHeapType.DRAW));
      DiscardButton?.onClick.AddListener(() => LogCards(CardHeapType.DISCARD));
      ConsumeButton?.onClick.AddListener(() => LogCards(CardHeapType.CONSUME));
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

    // Test
    private void LogCards(CardHeapType cardHeapType) {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (var card in Battle.SelfPlayer.Master.BattleCardControl[cardHeapType]) {
        stringBuilder.Append(card);
      }
      Debug.Log(stringBuilder);
    }
  }
}