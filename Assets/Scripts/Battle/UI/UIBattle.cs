using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI {
  public class UIBattle : UIBase {
    #region Text
    private const float TEXT_SHOW_TIME = 0.5f;
    private const float ANIM_TIME = 0.2f;
    [SerializeField]
    private GameObject TextPrefab;
    private Stack<Text> TextStack = new Stack<Text>();

    /// <summary>
    /// 显示文本(伤害、治疗等数值)
    /// </summary>
    public async void ShowText(string text, Vector3 pos, Color color, bool random) {
      if (!TextPrefab) {
        return;
      }

      Text textComponent;
      if (TextStack.Count > 0) {
        textComponent = TextStack.Pop();
        textComponent.gameObject.SetActive(true);
      } else {
        textComponent = Object.Instantiate(TextPrefab, Battle.UIBattle.TextNode).GetComponent<Text>();
      }

      textComponent.text = text;
      color.a = 0;
      textComponent.color = color;
      textComponent.transform.position = pos;
      var rectTransform = textComponent.GetComponent<RectTransform>();
      var width = rectTransform.rect.width;
      var height = rectTransform.rect.height;
      if (random) {
        rectTransform.anchoredPosition += 0.5f * new Vector2(Random.Range(-width, width), Random.Range(-height, height));
      }
      float targetAnchoredPositionY = rectTransform.anchoredPosition.y + height;
      await UniTask.WhenAll(rectTransform.DOAnchorPosY(targetAnchoredPositionY, ANIM_TIME).AwaitForComplete(), textComponent.DOFade(1.0f, ANIM_TIME).AwaitForComplete());
      // 这里这样判断是因为动画播放过程中战斗结束了数字会被销毁 下同
      if (textComponent) {
        await textComponent.DOFade(0, ANIM_TIME).SetDelay(TEXT_SHOW_TIME).AwaitForComplete();
        if (textComponent) {
          TextStack.Push(textComponent);
          textComponent.gameObject.SetActive(false);
        }
      }
    }
    #endregion

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