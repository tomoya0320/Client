using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI {
  public class UIBattle : UIBase {
    #region Text
    private const float SHOW_TIME = 0.5f;
    private const float ANIM_TIME = 0.2f;
    [SerializeField]
    private GameObject PermanentTextRoot;
    [SerializeField]
    private Text PermanentText;
    [SerializeField]
    private GameObject TextPrefab;
    private Stack<Text> TextStack = new Stack<Text>();

    /// <summary>
    /// 显示文本(伤害、治疗等数值)
    /// </summary>
    public async void ShowText(string text, Vector3 pos, Color color, bool random, float animTime = ANIM_TIME, float showTime = SHOW_TIME) {
      try {
        if (!TextPrefab) {
          return;
        }

        Text textComponent;
        if (TextStack.Count > 0) {
          textComponent = TextStack.Pop();
        } else {
          textComponent = Instantiate(TextPrefab, Battle.UIBattle.TextNode).GetComponent<Text>();
        }
        textComponent.gameObject.SetActiveEx(true);

        textComponent.text = text;
        color.a = 0;
        textComponent.color = color;
        textComponent.DOFade(1.0f, animTime);

        textComponent.transform.position = pos;
        float width = textComponent.rectTransform.rect.width;
        float height = textComponent.rectTransform.rect.height;
        if (random) {
          textComponent.rectTransform.anchoredPosition += 0.5f * new Vector2(Random.Range(-width, width), Random.Range(-height, height));
        }
        float targetAnchoredPositionY = textComponent.rectTransform.anchoredPosition.y + height;
        textComponent.rectTransform.DOAnchorPosY(targetAnchoredPositionY, animTime);
        await UniTask.Delay((int)(GameConstant.THOUSAND * animTime), cancellationToken: Battle.CancellationToken);
        // 这里这样判断是因为动画播放过程中战斗结束了数字会被销毁 下同
        if (textComponent) {
          textComponent.DOFade(0, animTime).SetDelay(showTime);
          await UniTask.Delay((int)(GameConstant.THOUSAND * (animTime + showTime)), cancellationToken: Battle.CancellationToken);
          if (textComponent) {
            TextStack.Push(textComponent);
            textComponent.gameObject.SetActiveEx(false);
          }
        }
      } catch (System.OperationCanceledException) { }
    }
    
    public void ShowPermanentText(string text) {
      PermanentText.text = text;
      PermanentTextRoot.SetActiveEx(true);
    }
    #endregion

    private Battle Battle;
    [SerializeField]
    private Transform[] UnitNodes;
    [SerializeField]
    private Text TurnText;
    public RectTransform HandCardPosRefNode;
    public Transform DrawNode;
    public Transform DiscardNode;
    public Transform ConsumeNode;
    public Transform TextNode;
    public Transform CardNode;

    public override UniTask Init(UIType type, params object[] args) {
      Battle = args[0] as Battle;
      Battle.SelfPlayer.OnStartTurn += OnSelfStartTurn;

      return base.Init(type, args);
    }

    public Transform GetUnitNode(int index) {
      if (index < 0 || index >= UnitNodes.Length) {
        return null;
      }

      return UnitNodes[index];
    }

    public void EndTurn() => Battle.SelfPlayer.EndTurnFlag |= EndTurnFlag.NORMAL_END;

    public async void OpenDrawCardHeapUI() => await OpenCardHeapUI(CardHeapType.DRAW);

    public async void OpenDiscardCardHeapUI() => await OpenCardHeapUI(CardHeapType.DISCARD);

    public async void OpenConsumeCardHeapUI() => await OpenCardHeapUI(CardHeapType.CONSUME);

    private UniTask OpenCardHeapUI(CardHeapType cardHeapType) {
      var cardList = TempList<Card>.Get();
      Battle.SelfPlayer.Master.BattleCardControl.GetCardList(cardHeapType, cardList);
      return UIManager.Instance.OpenChild<UICardHeap>(this, "UICardHeap", cardList);
    }

    private void OnSelfStartTurn() {
      if (TurnText) {
        TurnText.text = $"{Battle.Turn}";
      }
    }
  }
}