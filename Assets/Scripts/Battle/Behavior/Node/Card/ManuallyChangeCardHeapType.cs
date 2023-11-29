using Cysharp.Threading.Tasks;
using GameCore.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("�ڵ�/��Ϊ/����/�ֶ��ı俨�ƶ�����")]
  public class ManuallyChangeCardHeapType : ActionNode {
    [LabelText("Ŀ�굥λ")]
    public NodeParamKey TargetUnit;
    [LabelText("ԭ����")]
    public CardHeapType SourceType;
    [LabelText("Ŀ������")]
    public CardHeapType TargetType;
    [LabelText("�޸��������ֵ")]
    public NodeIntParam MaxCountKey;
    [LabelText("ʵ���޸�����ֵ")]
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