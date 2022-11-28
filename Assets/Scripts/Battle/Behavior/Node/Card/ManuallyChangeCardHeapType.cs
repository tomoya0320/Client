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
    public CardHeapType SourceCardHeapTypeType;
    [LabelText("Ŀ������")]
    public CardHeapType TargetCardHeapTypeType;
    [LabelText("�޸��������ֵ")]
    public NodeIntParam ChangeMaxCountKey;
    [LabelText("ʵ���޸�����ֵ")]
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