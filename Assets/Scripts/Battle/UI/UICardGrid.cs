using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI {
  public class UICardGrid : MonoBehaviour, IScrollGrid {
    [SerializeField]
    private Text LvText;
    [SerializeField]
    private Text CostText;
    [SerializeField]
    private Text DescText;
    [SerializeField]
    private Text NameText;
    [SerializeField]
    private Text TypeText;
    [SerializeField]
    private Image IconImage;
    private RectTransform _RectTransform;
    public RectTransform RectTransform => _RectTransform ?? (_RectTransform = GetComponent<RectTransform>());
    private List<Card> CardList;

    public void Init(List<Card> cardList) => CardList = cardList;

    public void Refresh(int index) {
      if (index < 0 || index >= CardList.Count) {
        gameObject.SetActiveEx(false);
        return;
      }

      var card = CardList[index];
      LvText.text = $"{card.Lv + 1}";
      CostText.text = card.Cost >= 0 ? card.Cost.ToString() : "X";
      DescText.text = card.Desc;
      NameText.text = card.Name;
      TypeText.text = card.CardType.GetDescription();
      if (card.Battle.SpriteManager.TryGetAsset(card.IconId, out var sprite)) {
        IconImage.sprite = sprite;
      }
      gameObject.SetActiveEx(true);
    }
  }
}