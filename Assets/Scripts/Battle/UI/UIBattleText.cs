using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace GameCore {
  public class UIBattleText : BattleBase {
    private const float TEXT_SHOW_TIME = 0.4f;
    private const float ANIM_DURATION = 0.3f;
    private GameObject TextPrefab;
    private Stack<Text> TextStack = new Stack<Text>();

    public UIBattleText(Battle battle) : base(battle) { }

    public async UniTask InitUI() {
      TextPrefab = await Addressables.LoadAssetAsync<GameObject>("BattleTextPrefab");
    }

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
      await UniTask.WhenAll(rectTransform.DOAnchorPosY(targetAnchoredPositionY, ANIM_DURATION).AwaitForComplete(), textComponent.DOFade(1.0f, ANIM_DURATION).AwaitForComplete());
      // 这里这样判断是因为动画播放过程中战斗结束了数字会被销毁 下同
      if (textComponent) {
        await textComponent.DOFade(0, ANIM_DURATION).SetDelay(TEXT_SHOW_TIME).AwaitForComplete();
        if (textComponent) {
          TextStack.Push(textComponent);
          textComponent.gameObject.SetActive(false);
        }
      }
    }
  }
}