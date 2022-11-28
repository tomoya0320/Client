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
    public CardHeapType SourceCardHeapTypeType;
    [LabelText("目标类型")]
    public CardHeapType TargetCardHeapTypeType;
    [LabelText("修改数量最大值")]
    public NodeIntParam ChangeMaxCountKey;
    [LabelText("实际修改数存值")]
    public NodeParamKey ChangeCountKey;

    public async override UniTask<NodeResult> Run(Behavior behavior, Context context) {
      var targetUnit = behavior.GetUnit(TargetUnit);
      if (targetUnit == null) {
        return NodeResult.False;
      }
      var cardList = TempList<Card>.Get();
      targetUnit.BattleCardControl.GetCardList(SourceCardHeapTypeType, cardList);
      var ui = await UIManager.Instance.OpenChild<UICardSelect>(behavior.Battle.UIBattle, "UICardSelect", behavior.Battle, cardList, true, new Vector2Int(0, behavior.GetInt(ChangeMaxCountKey)));
      var selectedCardList = await ui.WaitForSelectingCards();
      foreach (var selectedCard in selectedCardList) {
        await selectedCard.SetCardHeapType(TargetCardHeapTypeType);
        await UniTask.Delay(100, cancellationToken: behavior.Battle.CancellationToken);
      }
      behavior.SetInt(ChangeCountKey, selectedCardList.Count);
      TempList<Card>.Release(cardList);
      return NodeResult.True;
    }
  }
}