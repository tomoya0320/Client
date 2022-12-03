using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI {
  public class UICardGrid : ScrollGrid<Card> {
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

    protected override void RefreshInternal(Card card) {
      LvText.text = $"{card.Lv + 1}";
      CostText.text = card.Cost >= 0 ? card.Cost.ToString() : "X";
      DescText.text = card.Desc;
      NameText.text = card.Name;
      TypeText.text = card.CardType.GetDescription();
      IconImage.sprite = card.CardTemplate.Icon?.Asset as Sprite;
    }
  }
}