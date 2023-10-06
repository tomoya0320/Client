using Cysharp.Threading.Tasks;
using GameCore.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/卡牌/手动改变卡牌堆类型")]
  public class ManuallyChangeCardHeapType : ActionNode {
    [LabelText("目标单位")]
    public NodeParamKey TargetUnit;
    [LabelText("原类型")]
    public CardHeapType SourceType;
    [LabelText("目标类型")]
    public CardHeapType TargetType;
    [LabelText("修改数量最大值")]
    public NodeIntParam MaxCountKey;
    [LabelText("实际修改数存值")]
    public NodeParamKey RealCountKey;

    public override async UniTask<NodeResult> Run(Behavior behavior, Context context) {
      var targetUnit = behavior.GetUnit(TargetUnit);
      if (targetUnit == null) {
        return NodeResult.False;
      }
      var cardList = TempList<Card>.Get();
      targetUnit.BattleCardControl.GetCardList(SourceType, cardList);
      var ui = await UIManager.Instance.OpenChild<UICardSelect>(behavior.Battle.UIBattle, "UICardSelect", behavior.Battle, cardList, true, new Vector2Int(0, behavior.GetInt(MaxCountKey)));
      var selectedCardList = await ui.WaitForSelectingCards();
      foreach (var selectedCard in selectedCardList) {
        await selectedCard.SetCardHeapType(TargetType);
        await UniTask.Delay(100, cancellationToken: behavior.Battle.CancellationToken);
      }
      behavior.SetInt(RealCountKey, selectedCardList.Count);
      TempList<Card>.Release(cardList);
      return NodeResult.True;
    }
  }
}