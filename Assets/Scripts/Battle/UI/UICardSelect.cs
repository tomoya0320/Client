using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI {
  public class UICardSelect : UIBase {
    [SerializeField]
    private Button OkBtn;
    [SerializeField]
    private Button CancelBtn;
    [SerializeField]
    private DynamicScrollRect DynamicScrollRect;
    private Battle Battle;
    private List<Card> CardList;
    private Vector2Int SelectCountRange;
    private bool Completed;
    private bool Cancellable;
    private List<Card> SelectedCardList = new List<Card>();

    public override UniTask Init(UIType type, params object[] args) {
      Battle = args[0] as Battle;
      CardList = args[1] as List<Card>;
      Cancellable = (bool)args[2];
      SelectCountRange = (Vector2Int)args[3];

      DynamicScrollRect.Init<UICardGrid, Card>(CardList, OnCardSelected, OnCardUnselected, CheckCardSelected);

      OkBtn.onClick.AddListener(async () => {
        await Close();
        Completed = true;
      });
      UpdateOkBtn();

      if (Cancellable) {
        CancelBtn.onClick.AddListener(async () => {
          await Close();
          SelectedCardList.Clear();
          Completed = true;
        });
        CancelBtn.gameObject.SetActiveEx(true);
      } else {
        CancelBtn.gameObject.SetActiveEx(false);
      }

      return base.Init(type, args);
    }

    private bool OnCardSelected(Card card) {
      if (SelectedCardList.Count >= SelectCountRange.y) {
        return false;
      }
      SelectedCardList.Add(card);
      UpdateOkBtn();
      return true;
    }

    private bool OnCardUnselected(Card card) {
      if (SelectedCardList.Remove(card)) {
        UpdateOkBtn();
        return true;
      }
      return false;
    }

    private bool CheckCardSelected(Card card) => SelectedCardList.Contains(card);

    private void UpdateOkBtn() {
      OkBtn.interactable = SelectedCardList.Count >= SelectCountRange.x;
    }

    public async UniTask<List<Card>> WaitForSelectingCards() {
      await UniTask.WaitUntil(() => Completed, cancellationToken: Battle.CancellationToken);
      return SelectedCardList;
    }
  }
}