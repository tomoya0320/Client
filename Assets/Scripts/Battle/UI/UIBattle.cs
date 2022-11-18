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

    public void InitBattle(Battle battle) {
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